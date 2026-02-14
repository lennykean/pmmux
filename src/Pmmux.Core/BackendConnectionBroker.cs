using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal class BackendConnectionBroker(
    IConnectionOrientedBackend backend,
    IBackendMonitor monitor,
    ILoggerFactory loggerFactory,
    IMetricReporter metricReporter)
    : BackendBroker(Protocol.Tcp, backend, monitor, loggerFactory.CreateLogger("connection-broker"))
{
    private enum State
    {
        Initial = 0,
        Initializing = 1,
        Initialized = 2,
        Drain = 3,
        Dispose = 4
    }

    private readonly ConcurrentDictionary<IClientConnection, Task<BackendConnectionClientBroker>> _connections = [];
    private readonly TaskCompletionSource<bool> _disposedTsc = new();
    private readonly StateManager<State> _state = new(State.Initial);
    private readonly Dictionary<string, string?> _baseMetadata = new()
    {
        ["backend"] = backend.Backend.Spec.Name,
        ["protocol"] = backend.Backend.Spec.ProtocolName,
        ["network_protocol"] = Protocol.Tcp.ToString()
    };

    public override async Task InitializeAsync(
        IClientWriterFactory clientWriterFactory,
        CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(BackendConnectionBroker));
        }
        if (!_state.TryTransition(to: State.Initializing, from: State.Initial))
        {
            throw new InvalidOperationException();
        }

        using (Logger.BeginScope("{BackendName}:{BackendProtocol}", Backend.Spec.Name, Backend.Spec.ProtocolName))
        {
            try
            {
                Logger.LogInformation(
                     "initializing with parameters [{BackendParameters}]",
                     string.Join(",", Backend.Spec.Parameters.Select(x => $"{x.Key}={x.Value}")));

                await backend.InitializeAsync(cancellationToken).ConfigureAwait(false);

                await base.InitializeAsync(clientWriterFactory, cancellationToken).ConfigureAwait(false);

                Logger.LogTrace("initialized");

                _state.TryTransition(to: State.Initialized, from: State.Initializing);
            }
            catch (OperationCanceledException)
            {
                _state.TryTransition(to: State.Initial, from: State.Initializing);
            }
            catch
            {
                _state.TryTransition(to: State.Initial, from: State.Initializing);
                throw;
            }
        }
    }

    public int GetConnectionCount()
    {
        return _connections.Count;
    }

    public async Task<bool> CanHandleConnectionAsync(
        IClientConnectionPreview clientConnection,
        CancellationToken cancellationToken = default)
    {
        if (!_state.Is(State.Initialized))
        {
            return false;
        }
        using (Logger.BeginScope("{BackendName}:{BackendProtocol}", Backend.Spec.Name, Backend.Spec.ProtocolName))
        {
            Logger.LogTrace("backend probing connection");

            var canHandle = await metricReporter.MeasureDurationAsync(
                "backend.probe.duration",
                "multiplexer",
                _baseMetadata,
                async () => await backend.CanHandleConnectionAsync(
                    clientConnection,
                    cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

            if (canHandle)
            {
                Logger.LogTrace(
                    "backend can handle connection {RemoteEndpoint}",
                    clientConnection.Client.RemoteEndpoint);
            }
            return canHandle;
        }
    }

    public async Task HandleConnectionAsync(
        IClientConnection clientConnection,
        CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(BackendConnectionBroker));
        }
        if (!_state.Is(State.Initialized))
        {
            throw new InvalidOperationException();
        }
        using (Logger.BeginScope("{BackendName}:{BackendProtocol}", Backend.Spec.Name, Backend.Spec.ProtocolName))
        {
            var connectionBroker = await _connections.AddOrUpdate(clientConnection, async _ =>
            {
                var backendConnection = await backend.CreateBackendConnectionAsync(
                    clientConnection,
                    cancellationToken).ConfigureAwait(false);

                var connectionBroker = new BackendConnectionClientBroker(
                    backendConnection,
                    clientConnection,
                    loggerFactory);

                connectionBroker.ClientConnectionClosed += async (_, e) =>
                {
                    Logger.LogTrace("client connection closed");

                    _connections.TryRemove(clientConnection, out var _);

                    metricReporter.ReportEvent("backend.connection.closed", "multiplexer", new(_baseMetadata)
                    {
                        ["initiator"] = "client"
                    });
                    metricReporter.ReportGauge(
                        "backend.connections.active",
                        "multiplexer",
                        _connections.Count,
                        _baseMetadata);

                    await connectionBroker.DisposeAsync();
                };
                connectionBroker.BackendConnectionClosed += async (_, e) =>
                {
                    Logger.LogTrace("backend connection closed");

                    _connections.TryRemove(clientConnection, out var _);

                    metricReporter.ReportEvent("backend.connection.closed", "multiplexer", new(_baseMetadata)
                    {
                        ["initiator"] = "backend"
                    });
                    metricReporter.ReportGauge(
                        "backend.connections.active",
                        "multiplexer",
                        _connections.Count,
                        _baseMetadata);

                    await connectionBroker.DisposeAsync();
                };
                return connectionBroker;
            }, (_, _) => throw new InvalidOperationException()).ConfigureAwait(false);

            Logger.LogTrace("client broker created");

            connectionBroker.Start();

            metricReporter.ReportEvent("backend.connection.accepted", "multiplexer", _baseMetadata);
            metricReporter.ReportGauge("backend.connections.active", "multiplexer", _connections.Count, _baseMetadata);

            Logger.LogTrace("backend accepted connection");
        }
    }

    public async Task DrainAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_state.Is(State.Dispose), nameof(BackendConnectionBroker));

        if (!_state.TryTransition(to: State.Drain, from: State.Initialized))
        {
            throw new InvalidOperationException();
        }
        await StopMonitorAsync().ConfigureAwait(false);
        Status = new(Backend, BackendStatus.Draining);
        Logger.LogDebug("status changed: {BackendStatus}", Status.Status);

        await Task.WhenAll(_connections.Values.Select(async brokerTask =>
        {
            var broker = await brokerTask.ConfigureAwait(false);
            await broker.WaitAsync(cancellationToken).ConfigureAwait(false);
        })).ConfigureAwait(false);
    }

    public override async ValueTask DisposeAsync()
    {
        using (Logger.BeginScope("{BackendName}:{BackendProtocol}", Backend.Spec.Name, Backend.Spec.ProtocolName))
        {
            if (_state.Is(State.Dispose) || !_state.TryTransition(
                to: State.Dispose,
                from: [State.Drain, State.Initialized, State.Initializing, State.Initial]))
            {
                await _disposedTsc.Task.ConfigureAwait(false);
                return;
            }

            try
            {
                await base.DisposeAsync().ConfigureAwait(false);

                await Task.WhenAll(_connections.Values.Select(async connectionBrokerTask =>
                {
                    var connectionBroker = await connectionBrokerTask.ConfigureAwait(false);

                    await connectionBroker.DisposeAsync().ConfigureAwait(false);
                })).ConfigureAwait(false);
                _connections.Clear();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex, "error disposing");

                _disposedTsc.SetException(ex);

                throw;
            }
            if (Status.Status is not BackendStatus.Stopped)
            {
                Status = new(Backend, BackendStatus.Stopped);
                Logger.LogDebug("status changed: {BackendStatus}", Status.Status);
            }

            Logger.LogTrace("connection broker disposed");

            _disposedTsc.SetResult(true);
        }
    }
}
