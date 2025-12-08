using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;
using Pmmux.Core.Configuration;

using Result = Pmmux.Abstractions.IRouter.Result;

namespace Pmmux.Core;

/// <summary>
/// Default implementation of <see cref="IRouter"/>.
/// </summary>
/// <param name="connectionNegotiators">The collection of loaded client connection negotiators.</param>
/// <param name="routingStrategies">The collection of loaded routing strategies.</param>
/// <param name="protocols">The collection of loaded backend protocols.</param>
/// <param name="eventSender">The event sender service.</param>
/// <param name="backendMonitor">The backend monitor service.</param>
/// <param name="metricReporter">The metric reporter service.</param>
/// <param name="config">The router configuration.</param>
/// <param name="loggerFactory">The logger factory.</param>
public sealed class Router(
    IEnumerable<IClientConnectionNegotiator> connectionNegotiators,
    IEnumerable<IRoutingStrategy> routingStrategies,
    IEnumerable<IBackendProtocol> protocols,
    IEventSender eventSender,
    IBackendMonitor backendMonitor,
    IMetricReporter metricReporter,
    RouterConfig config,
    ILoggerFactory loggerFactory) : IRouter
{
    private record SocketListener(Socket Listener, ListenerInfo ListenerInfo);
    private record OrderedBroker<TBroker>(TBroker Broker, long Ordinal);

    private enum State
    {
        Initial = 0,
        Initializing = 1,
        Initialized = 2,
        Dispose = 3
    }

    private readonly ConcurrentDictionary<string, IBackendProtocol> _backendProtocols = [];
    private readonly ConcurrentDictionary<BackendInfo, OrderedBroker<BackendConnectionBroker>> _connectionBrokers = [];
    private readonly ConcurrentDictionary<BackendInfo, OrderedBroker<BackendMessageBroker>> _messageBrokers = [];
    private readonly TaskCompletionSource<bool> _disposedTcs = new();
    private readonly StateManager<State> _state = new(State.Initial);
    private readonly IClientConnectionNegotiator[] _connectionNegotiators = [.. connectionNegotiators];
    private readonly IRoutingStrategy[] _routingStrategies = [.. routingStrategies];
    private readonly IBackendProtocol[] _protocols = [.. protocols];
    private readonly ILogger _logger = loggerFactory.CreateLogger("router");

    private IRoutingStrategy _routingStrategy = null!;
    private IClientWriterFactory _clientWriterFactory = null!;
    private long _counter = 0;

    /// <inheritdoc />
    public async Task InitializeAsync(IClientWriterFactory clientWriterFactory, CancellationToken cancellationToken)
    {
        if (!_state.TryTransition(to: State.Initializing, from: State.Initial))
        {
            throw new InvalidOperationException();
        }

        foreach (var routingStrategy in _routingStrategies)
        {
            _logger.LogTrace("loaded routing strategy: {RoutingStrategyName}", routingStrategy.Name);
        }
        foreach (var protocol in _protocols)
        {
            _logger.LogTrace("loaded backend protocol: {BackendProtocolName}", protocol.Name);
        }
        foreach (var negotiator in _connectionNegotiators)
        {
            _logger.LogTrace("loaded connection negotiator: {ConnectionNegotiatorName}", negotiator.Name);
        }
        try
        {
            var matchedRoutingStrategies = routingStrategies.Where(x => x.Name == config.RoutingStrategy).ToList();
            if (matchedRoutingStrategies is not [var routingStrategy])
            {
                throw new InvalidOperationException($"invalid routing strategy: \"{config.RoutingStrategy}\"");
            }
            _routingStrategy = routingStrategy;
            _logger.LogDebug("using routing strategy {RoutingStrategy}", routingStrategy.Name);

            foreach (var protocol in protocols)
            {
                if (!_backendProtocols.TryAdd(protocol.Name, protocol))
                {
                    throw new InvalidOperationException($"duplicate protocol: {protocol.Name}");
                }
            }

            _clientWriterFactory = clientWriterFactory;

            foreach (var backendSpec in config.Backends)
            {
                await AddBackendInternalAsync(
                    Protocol.Tcp,
                    backendSpec,
                    cancellationToken: cancellationToken).ConfigureAwait(false);
                await AddBackendInternalAsync(
                    Protocol.Udp,
                    backendSpec,
                    cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            _state.TryTransition(to: State.Initialized, from: State.Initializing);
        }
        catch
        {
            _backendProtocols.Clear();
            _state.TryTransition(to: State.Initial, from: State.Initializing);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Result> RouteConnectionAsync(
        Socket connection,
        ClientInfo client,
        CancellationToken cancellationToken = default)
    {
        if (connection is null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        var baseMetadata = new Dictionary<string, string?>
        {
            ["strategy"] = _routingStrategy.Name,
            ["client"] = client.RemoteEndpoint?.ToString()
        };

        _logger.LogTrace("routing connection {RemoteEndpoint}", client.RemoteEndpoint);

        metricReporter.ReportEvent("router.connection.total", "multiplexer", baseMetadata);

        var result = await metricReporter.MeasureDurationAsync(
            "router.negotiation.duration",
            "multiplexer",
            baseMetadata,
            () => NegotiateConnectionAsync(client, connection, cancellationToken));

        if (result?.Success is not true)
        {
            metricReporter.ReportEvent("router.connection.failed", "multiplexer", new(baseMetadata)
            {
                ["reason"] = "negotiation_failed"
            });
            return Result.Failed(result?.Reason!);
        }

        using (_logger.BeginScope("{RemoteEndpoint}", client.RemoteEndpoint))
        {
            _logger.LogTrace(
                "{RemoteEndpoint} connection accepted with properties [{Properties}]",
                client.RemoteEndpoint,
                string.Join(", ", result.ClientConnection!.Properties.Select(p => $"\"{p.Key}\"=\"{p.Value}\"")));

            try
            {
                var selectedBackend = await metricReporter.MeasureDurationAsync(
                    "router.selection.duration",
                    "multiplexer",
                    baseMetadata,
                    async () => await _routingStrategy.SelectBackendAsync(
                        client,
                        result.ClientConnection.Properties,
                        MatchBackendsAsync(result.ClientConnection),
                        cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

                if (selectedBackend is null)
                {
                    metricReporter.ReportEvent("router.connection.failed", "multiplexer", new(baseMetadata)
                    {
                        ["reason"] = "no_backend_selected"
                    });
                    return Result.Failed("connection could not be routed to any backend");
                }
                if (!_connectionBrokers.TryGetValue(selectedBackend!, out var brokerEntry))
                {
                    throw new InvalidOperationException("selected backend was not found");
                }
                metricReporter.ReportEvent("router.connection.routed", "multiplexer", new(baseMetadata)
                {
                    ["backend"] = selectedBackend.Spec.Name
                });

                await brokerEntry.Broker.HandleConnectionAsync(result.ClientConnection, cancellationToken)
                    .ConfigureAwait(false);

                return Result.Succeeded(selectedBackend);
            }
            catch (TimeoutException)
            {
                metricReporter.ReportEvent("router.connection.failed", "multiplexer", new(baseMetadata)
                {
                    ["reason"] = "selection_timeout"
                });
                return Result.Failed("selection timeout exceeded");
            }
        }
    }

    /// <inheritdoc />
    public async Task<Result> RouteMessageAsync(
        byte[] messageBuffer,
        ClientInfo client,
        CancellationToken cancellationToken = default)
    {
        if (messageBuffer is null)
        {
            throw new ArgumentNullException(nameof(messageBuffer));
        }

        var baseMetadata = new Dictionary<string, string?>
        {
            ["client"] = client.RemoteEndpoint?.ToString()
        };

        _logger.LogTrace("routing {MessageLength} byte message {RemoteEndpoint}",
            messageBuffer.Length,
            client.RemoteEndpoint);

        metricReporter.ReportEvent("router.message.total", "multiplexer", baseMetadata);
        metricReporter.ReportCounter("router.message.bytes", "multiplexer", messageBuffer.Length, baseMetadata);

        var selectedBackend = await metricReporter.MeasureDurationAsync(
            "router.message.selection.duration",
            "multiplexer",
            baseMetadata,
            async () => await _routingStrategy.SelectBackendAsync(
                client,
                new Dictionary<string, string>(),
                MatchBackendsAsync(client, [], messageBuffer),
                cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

        if (selectedBackend is null)
        {
            metricReporter.ReportEvent("router.message.failed", "multiplexer", new(baseMetadata)
            {
                ["reason"] = "no_backend_selected"
            });
            return Result.Failed("message could not be routed to any backend");
        }
        if (!_messageBrokers.TryGetValue(selectedBackend, out var brokerEntry))
        {
            throw new InvalidOperationException("selected backend was not found");
        }

        metricReporter.ReportEvent("router.message.routed", "multiplexer", new(baseMetadata)
        {
            ["backend"] = selectedBackend.Spec.Name
        });

        await brokerEntry.Broker.HandleMessageAsync(client, [], messageBuffer, cancellationToken).ConfigureAwait(false);

        return Result.Succeeded(selectedBackend);
    }

    /// <inheritdoc />
    public IEnumerable<BackendStatusInfo> GetBackends(Protocol networkProtocol)
    {
        return networkProtocol switch
        {
            Protocol.Tcp => _connectionBrokers.Values.OrderBy(b => b.Ordinal).Select(b => b.Broker.Status),
            Protocol.Udp => _messageBrokers.Values.OrderBy(b => b.Ordinal).Select(b => b.Broker.Status),
            _ => throw new ArgumentException($"unknown network protocol: {networkProtocol}", nameof(networkProtocol))
        };
    }

    /// <inheritdoc />
    public async Task<BackendInfo> AddBackendAsync(
       Protocol networkProtocol,
       BackendSpec backendSpec,
       CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(Router));
        }
        if (!_state.Is(State.Initialized))
        {
            throw new InvalidOperationException();
        }

        var protocol = protocols.FirstOrDefault(p => p.Name == backendSpec.ProtocolName)
            ?? throw new ArgumentException($"protocol not found: {backendSpec.ProtocolName}", nameof(backendSpec));

        _logger.LogTrace(
            "adding {NetworkProtocol} backend {BackendName}:{BackendProtocol}",
            networkProtocol,
            backendSpec.Name,
            backendSpec.ProtocolName);

        var broker = await AddBackendInternalAsync(
            networkProtocol,
            backendSpec,
            cancellationToken: cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"invalid backend spec: {backendSpec.Name}");

        _logger.LogInformation(
            "added {NetworkProtocol} backend {BackendName}:{BackendProtocol}",
            networkProtocol,
            backendSpec.Name,
            backendSpec.ProtocolName);

        eventSender.RaiseBackendAdded(this, backendSpec);

        return broker.Backend;
    }

    /// <inheritdoc />
    public Task<bool> RemoveBackendAsync(
        Protocol networkProtocol,
        BackendInfo backend,
        bool forceCloseConnections,
        CancellationToken cancellationToken)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(Router));
        }
        if (!_state.Is(State.Initialized))
        {
            throw new InvalidOperationException();
        }
        return RemoveBackendInternalAsync(networkProtocol, backend, forceCloseConnections, cancellationToken);
    }


    /// <inheritdoc />
    public async Task<BackendInfo?> ReplaceBackendAsync(
       Protocol networkProtocol,
       BackendInfo existingBackend,
       BackendSpec newBackendSpec,
       CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(Router));
        }
        if (!_state.Is(State.Initialized))
        {
            throw new InvalidOperationException();
        }

        _logger.LogTrace(
            "replacing {NetworkProtocol} backend {ExistingBackend}:{BackendProtocol}",
            networkProtocol,
            existingBackend.Spec.Name,
            existingBackend.Spec.ProtocolName);

        var existingOrdinal = networkProtocol switch
        {
            Protocol.Tcp when _connectionBrokers.TryGetValue(existingBackend, out var e) => e.Ordinal,
            Protocol.Udp when _messageBrokers.TryGetValue(existingBackend, out var e) => e.Ordinal,
            _ => (long?)null
        };

        if (existingOrdinal is not { } ordinal)
        {
            _logger.LogDebug(
                "{NetworkProtocol} backend {BackendName}:{BackendProtocol} not found",
                networkProtocol,
                existingBackend.Spec.Name,
                existingBackend.Spec.ProtocolName);

            return null;
        }

        if (existingBackend.Spec == newBackendSpec)
        {
            _logger.LogDebug(
                "{NetworkProtocol} backend {BackendName}:{BackendProtocol} spec unchanged, nothing to do",
                networkProtocol,
                existingBackend.Spec.Name,
                existingBackend.Spec.ProtocolName);

            return existingBackend;
        }

        var addBackendTask = AddBackendInternalAsync(
            networkProtocol,
            newBackendSpec,
            ordinal,
            cancellationToken);
        var removeBackendTask = RemoveBackendInternalAsync(
            networkProtocol,
            existingBackend,
            forceCloseConnections: false,
            cancellationToken: cancellationToken);

        await Task.WhenAll(addBackendTask, removeBackendTask).ConfigureAwait(false);

        var result = await addBackendTask.ConfigureAwait(false);

        _logger.LogInformation(
            "replaced {NetworkProtocol} backend {BackendName}:{BackendProtocol}",
            networkProtocol,
            existingBackend.Spec.Name,
            newBackendSpec.Name);

        return result?.Backend;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_state.Is(State.Dispose) ||
            !_state.TryTransition(to: State.Dispose, from: [State.Initialized, State.Initializing, State.Initial]))
        {
            await _disposedTcs.Task.ConfigureAwait(false);
            return;
        }

        _logger.LogTrace("router disposing");

        try
        {
            var disposeTasks = new List<Task>();

            foreach (var (broker, _) in _connectionBrokers.Values)
            {
                disposeTasks.Add(broker.DisposeAsync().AsTask());
            }

            foreach (var (broker, _) in _messageBrokers.Values)
            {
                disposeTasks.Add(broker.DisposeAsync().AsTask());
            }

            await Task.WhenAll(disposeTasks).ConfigureAwait(false);

            _connectionBrokers.Clear();
            _messageBrokers.Clear();

            _logger.LogDebug("router disposed");
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "error disposing");

            _disposedTcs.SetException(ex);
            throw;
        }

        _disposedTcs.SetResult(true);
    }

    private async Task<BackendBroker?> AddBackendInternalAsync(
        Protocol networkProtocol,
        BackendSpec backendSpec,
        long? ordinal = null,
        CancellationToken cancellationToken = default)
    {
        var backend = _backendProtocols.TryGetValue(backendSpec.ProtocolName, out var protocol)
            ? await protocol.CreateBackendAsync(backendSpec, cancellationToken).ConfigureAwait(false)
            : throw new ArgumentException($"protocol not found: {backendSpec.ProtocolName}", nameof(backendSpec));

        if (networkProtocol is Protocol.Tcp)
        {
            if (backend is not IConnectionOrientedBackend connectionOrientedBackend)
            {
                return null;
            }
            var connectionBroker = new BackendConnectionBroker(
                connectionOrientedBackend,
                backendMonitor,
                loggerFactory,
                metricReporter);

            _logger.LogTrace("connection broker created");

            if (!_connectionBrokers.TryAdd(
                connectionBroker.Backend,
                new(connectionBroker, ordinal ?? Interlocked.Increment(ref _counter))))
            {
                throw new InvalidOperationException($"duplicate backend: {backendSpec.Name}");
            }
            await connectionBroker.InitializeAsync(_clientWriterFactory, cancellationToken).ConfigureAwait(false);

            return connectionBroker;
        }
        if (networkProtocol is Protocol.Udp)
        {
            if (backend is not IConnectionlessBackend connectionlessBackend)
            {
                return null;
            }
            var messageBroker = new BackendMessageBroker(
                connectionlessBackend,
                backendMonitor,
                loggerFactory,
                metricReporter);

            _logger.LogTrace("message broker created");

            if (!_messageBrokers.TryAdd(
                messageBroker.Backend,
                new(messageBroker, ordinal ?? Interlocked.Increment(ref _counter))))
            {
                throw new InvalidOperationException($"duplicate backend: {backendSpec.Name}");
            }
            await messageBroker.InitializeAsync(_clientWriterFactory, cancellationToken).ConfigureAwait(false);

            return messageBroker;
        }
        throw new ArgumentException($"unknown network protocol: {networkProtocol}", nameof(networkProtocol));
    }

    private async Task<bool> RemoveBackendInternalAsync(
       Protocol networkProtocol,
       BackendInfo backend,
       bool forceCloseConnections,
       CancellationToken cancellationToken)
    {
        _logger.LogTrace(
            "removing {NetworkProtocol} backend {BackendName}:{BackendProtocol}",
            networkProtocol,
            backend.Spec.Name,
            backend.Spec.ProtocolName);

        if (networkProtocol is Protocol.Tcp)
        {
            if (!_connectionBrokers.TryGetValue(backend, out var connectionBroker))
            {
                _logger.LogDebug(
                    "{NetworkProtocol} backend {BackendName}:{BackendProtocol} not found",
                    networkProtocol,
                    backend.Spec.Name,
                    backend.Spec.ProtocolName);

                return false;
            }
            if (!forceCloseConnections)
            {
                await connectionBroker.Broker.DrainAsync(cancellationToken).ConfigureAwait(false);
            }
            await connectionBroker.Broker.DisposeAsync().ConfigureAwait(false);

            _connectionBrokers.TryRemove(backend, out _);

            _logger.LogInformation(
                "removed {NetworkProtocol} backend {BackendName}:{BackendProtocol}",
                networkProtocol,
                backend.Spec.Name,
                backend.Spec.ProtocolName);

            eventSender.RaiseBackendRemoved(this, backend.Spec);

            return true;
        }
        if (networkProtocol is Protocol.Udp)
        {
            if (!_messageBrokers.TryGetValue(backend, out var messageBroker))
            {
                _logger.LogDebug(
                    "{NetworkProtocol} backend {BackendName}:{BackendProtocol} not found",
                    networkProtocol,
                    backend.Spec.Name,
                    backend.Spec.ProtocolName);

                return false;
            }
            await messageBroker.Broker.DisposeAsync().ConfigureAwait(false);

            _messageBrokers.TryRemove(backend, out _);

            _logger.LogInformation(
                "removed {NetworkProtocol} backend {BackendName}:{BackendProtocol}",
                networkProtocol,
                backend.Spec.Name,
                backend.Spec.ProtocolName);

            eventSender.RaiseBackendRemoved(this, backend.Spec);

            return true;
        }
        return false;
    }

    private async IAsyncEnumerable<BackendStatusInfo> MatchBackendsAsync(
        IClientConnection clientConnection,
        [EnumeratorCancellation] CancellationToken enumeratorCancellationToken = default)
    {
        using var timeoutCts = new CancellationTokenSource(config.SelectionTimeout);

        foreach (var (broker, _) in _connectionBrokers.Values.OrderBy(b => b.Ordinal))
        {
            if (enumeratorCancellationToken.IsCancellationRequested)
            {
                break;
            }
            if (broker.Status.Status is BackendStatus.Unhealthy or BackendStatus.Draining or BackendStatus.Stopped)
            {
                continue;
            }

            await using var preview = clientConnection.Preview();

            var canHandle = await broker.CanHandleConnectionAsync(preview, timeoutCts.Token)
                .WithTimeout(timeoutCts.Token)
                .ConfigureAwait(false);

            if (canHandle)
            {
                yield return broker.Status;
            }
        }
    }

    private async IAsyncEnumerable<BackendStatusInfo> MatchBackendsAsync(
        ClientInfo client,
        Dictionary<string, string> metadata,
        byte[] messageBuffer,
        CancellationToken cancellationToken = default,
        [EnumeratorCancellation] CancellationToken enumeratorCancellationToken = default)
    {
        var message = new ReadOnlyMemory<byte>(messageBuffer);

        foreach (var (broker, _) in _messageBrokers.Values.OrderBy(b => b.Ordinal))
        {
            if (enumeratorCancellationToken.IsCancellationRequested)
            {
                break;
            }
            var canHandle = await broker.CanHandleMessageAsync(client, metadata, message, cancellationToken)
                .ConfigureAwait(false);

            if (canHandle)
            {
                yield return broker.Status;
            }
        }
    }

    private async Task<IClientConnectionNegotiator.Result?> NegotiateConnectionAsync(
        ClientInfo client,
        Socket connection,
        CancellationToken cancellationToken)
    {
        var context = new ClientConnectionContext()
        {
            Client = client,
            ClientConnection = connection,
            ClientConnectionStream = new NetworkStream(connection, ownsSocket: false),
            Properties = new ConcurrentDictionary<string, string>()
        };

        var next = RejectUnhandledConnectionAsync;

        _logger.LogTrace("starting connection negotiator chain");

        for (int i = 0; i < _connectionNegotiators.Length; i++)
        {
            var currentNegotiator = _connectionNegotiators[i];
            var currentNext = next;

            next = async () =>
            {
                _logger.LogTrace("negotiating {ConnectionNegotiatorName} connection", currentNegotiator.Name);

                return await currentNegotiator.NegotiateAsync(context, currentNext, cancellationToken)
                    .ConfigureAwait(false);
            };
        }
        return await next().ConfigureAwait(false);
    }

    private static Task<IClientConnectionNegotiator.Result> RejectUnhandledConnectionAsync()
    {
        return Task.FromResult(IClientConnectionNegotiator.Result.Reject("negotiator did not accept the connection"));
    }
}
