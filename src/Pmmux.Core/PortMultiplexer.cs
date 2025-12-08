using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;
using Pmmux.Core.Configuration;

using static Pmmux.Core.Configuration.ListenerConfig;

namespace Pmmux.Core;

/// <summary>
/// Default implementation of <see cref="IPortMultiplexer"/>.
/// </summary>
/// <param name="portWarden">The port warden service.</param>
/// <param name="router">The router service.</param>
/// <param name="config">The listener configuration.</param>
/// <param name="loggerFactory">The logger factory.</param>
public sealed class PortMultiplexer(
    IPortWarden portWarden,
    IRouter router,
    ListenerConfig config,
    ILoggerFactory loggerFactory) : IPortMultiplexer
{
    internal record BoundListener(
        ListenerInfo ListenerInfo,
        Socket Listener,
        Task Worker,
        CancellationTokenSource TokenSource);

    private enum State
    {
        Initial = 0,
        Starting = 3,
        Started = 4,
        Dispose = 5
    }

    private readonly ConcurrentDictionary<BindingConfig, BoundListener> _listeners = [];
    private readonly TaskCompletionSource<bool> _disposedTsc = new();
    private readonly CancellationTokenSource _workerCts = new();
    private readonly StateManager<State> _state = new(State.Initial);
    private readonly ILogger _logger = loggerFactory.CreateLogger("multiplexer");

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(PortMultiplexer));
        }
        if (!_state.TryTransition(to: State.Starting, from: State.Initial))
        {
            throw new InvalidOperationException();
        }
        _logger.LogInformation("starting port multiplexer");

        try
        {
            await portWarden.StartAsync(cancellationToken).ConfigureAwait(false);

            var listeners = config.PortBindings
                .Select(p => (p.NetworkProtocol, p.Port))
                .Union(portWarden.GetPortMaps().Select(p => (p.NetworkProtocol, Port: p.LocalPort)))
                .Distinct();

            foreach (var (NetworkProtocol, Port) in listeners)
            {
                AddListenerInternal(NetworkProtocol, Port);
            }
            await router.InitializeAsync(new ClientWriter.Factory(_listeners), cancellationToken).ConfigureAwait(false);

            _state.TryTransition(to: State.Started, from: State.Starting);
        }
        catch (OperationCanceledException)
        {
            _state.TryTransition(to: State.Initial, from: State.Starting);

            await TeardownAsync().ConfigureAwait(false);
            throw;
        }
        catch (Exception ex)
        {
            _state.TryTransition(to: State.Initial, from: State.Starting);

            _logger.LogError(ex, "error starting port multiplexer");

            await TeardownAsync().ConfigureAwait(false);

            throw;
        }
    }

    /// <inheritdoc />
    public Task<IEnumerable<ListenerInfo>> GetListenersAsync(CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(PortMultiplexer));
        }
        if (!_state.Is(State.Started))
        {
            throw new InvalidOperationException();
        }
        return Task.FromResult(_listeners.Values.Select(bl => bl.ListenerInfo));
    }

    /// <inheritdoc />
    public ListenerInfo? AddListener(Protocol networkProtocol, int port)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(PortMultiplexer));
        }
        if (!_state.Is(State.Started))
        {
            throw new InvalidOperationException();
        }
        var binding = new BindingConfig(networkProtocol, port);

        if (_listeners.ContainsKey(binding))
        {
            throw new ArgumentException($"listener conflict");
        }
        return AddListenerInternal(networkProtocol, port);
    }

    /// <inheritdoc />
    public async Task<bool> RemoveListenerAsync(
        Protocol networkProtocol,
        int port,
        CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(PortMultiplexer));
        }
        if (!_state.Is(State.Started))
        {
            throw new InvalidOperationException();
        }
        var binding = new BindingConfig(networkProtocol, port);
        if (!_listeners.TryRemove(binding, out var boundListener))
        {
            return false;
        }

        _logger.LogTrace("removing {NetworkProtocol} listener on port {Port}", networkProtocol, port);

        try
        {
            boundListener.TokenSource.Cancel();
            await boundListener.Worker.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            boundListener.TokenSource.Dispose();
        }

        TeardownSocket(boundListener.Listener, $"{networkProtocol} listener on port {port}");

        _logger.LogInformation("removed {NetworkProtocol} listener on port {Port}", networkProtocol, port);

        return true;
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_state.Is(State.Dispose) ||
            !_state.TryTransition(to: State.Dispose, from: [State.Started, State.Starting, State.Initial]))
        {
            await _disposedTsc.Task.ConfigureAwait(false);
            return;
        }

        _logger.LogTrace("multiplexer disposing");

        try
        {
            await TeardownAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _disposedTsc.SetException(ex);
            throw;
        }
        finally
        {
            _workerCts.Dispose();
        }
        _logger.LogDebug("multiplexer disposed");

        _disposedTsc.SetResult(true);
    }

    private ListenerInfo? AddListenerInternal(Protocol networkProtocol, int port)
    {
        var binding = new BindingConfig(networkProtocol, port);

        using (_logger.BeginScope("{NetworkProtocol} {Port}", networkProtocol, port))
        {
            try
            {
                var listener = networkProtocol switch
                {
                    Protocol.Tcp =>
                        new Socket(
                            AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp),
                    Protocol.Udp =>
                        new Socket(
                            AddressFamily.InterNetwork,
                            SocketType.Dgram,
                            ProtocolType.Udp),
                    _ =>
                        throw new ArgumentException("invalid network protocol")
                };

                _logger.LogTrace("binding port");

                listener.Bind(new IPEndPoint(config.BindAddress, port));

                if (listener.SocketType == SocketType.Stream)
                {
                    listener.Listen(config.QueueLength);
                }

                var endpoint = (IPEndPoint)listener.LocalEndPoint!;
                var properties = NetworkUtility.GetProperties(listener);
                var listenerInfo = new ListenerInfo(
                    networkProtocol,
                    endpoint,
                    new Dictionary<string, string>(properties));

                var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(_workerCts.Token);
                var boundListener = new BoundListener(
                    listenerInfo,
                    listener,
                    StartListenerWorkerAsync(networkProtocol, listener, tokenSource.Token),
                    tokenSource);

                _listeners.AddOrUpdate(
                    binding,
                    _ => boundListener,
                    (_, _) => throw new ArgumentException($"listener conflict"));

                _logger.LogDebug("listener created on {Endpoint}", endpoint);

                return listenerInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error adding listener");
                throw;
            }
        }
    }

    private Task StartListenerWorkerAsync(
        Protocol networkProtocol,
        Socket listener,
        CancellationToken cancellationToken)
    {
        var task = networkProtocol switch
        {
            Protocol.Tcp => StartConnectionListenerWorkerAsync(networkProtocol, listener, cancellationToken),
            Protocol.Udp => StartConnectionlessListenerWorkerAsync(networkProtocol, listener, cancellationToken),
            _ => throw new InvalidOperationException()
        };
        _logger.LogTrace("listener worker started");

        return task;
    }

    private async Task StartConnectionListenerWorkerAsync(
        Protocol networkProtocol,
        Socket listener,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("listener starting");
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogTrace("waiting for connection");

                    var clientConnection = await listener.AcceptAsync()
                        .WithCancellation(cancellationToken)
                        .ConfigureAwait(false);

                    if (clientConnection.RemoteEndPoint as IPEndPoint is not IPEndPoint remoteEndpoint)
                    {
                        _logger.LogWarning(
                            "received connection from invalid source: {RemoteEndpoint}",
                            clientConnection.RemoteEndPoint);

                        TeardownSocket(clientConnection, $"{networkProtocol} {clientConnection.RemoteEndPoint}");

                        continue;
                    }
                    _logger.LogTrace("received connection from {RemoteEndpoint}", remoteEndpoint);

                    _ = Task.Run(async () =>
                    {
                        var client = new ClientInfo((IPEndPoint)listener.LocalEndPoint, remoteEndpoint);
                        try
                        {
                            var (success, backend, reason) = await router.RouteConnectionAsync(
                                clientConnection,
                                client,
                                cancellationToken).ConfigureAwait(false);

                            if (success)
                            {
                                _logger.LogInformation(
                                    "{RemoteEndpoint} routed to {BackendName}:{BackendProtocol}",
                                    client.RemoteEndpoint,
                                    backend!.Spec.Name,
                                    backend!.Spec.ProtocolName);
                            }
                            else
                            {
                                _logger.LogInformation(
                                    "failed to route {RemoteEndpoint}: {Reason}",
                                    client.RemoteEndpoint,
                                    reason!);

                                TeardownSocket(clientConnection, $"{networkProtocol} {client.RemoteEndpoint}");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "error routing {RemoteEndpoint}", client.RemoteEndpoint);

                            TeardownSocket(clientConnection, $"{networkProtocol} {client.RemoteEndpoint}");
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "error listening for {NetworkProtocol} connection", networkProtocol);
                }
            }
        }
        finally
        {
            TeardownSocket(listener, $"{networkProtocol} listener");

            _logger.LogTrace("{NetworkProtocol} listener worker stopped", networkProtocol);
        }
    }

    private async Task StartConnectionlessListenerWorkerAsync(
        Protocol networkProtocol,
        Socket listener,
        CancellationToken cancellationToken)
    {
        var mtu = NetworkUtility.TryGetMtu(listener, out var mtuValue) ? mtuValue : ushort.MaxValue;
        var receiveEndpoint = new IPEndPoint(IPAddress.Any, 0);

        _logger.LogInformation("listener starting");
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var buffer = new byte[mtu];

                    _logger.LogTrace("waiting for message");
                    var result = await listener.ReceiveFromAsync(buffer, SocketFlags.None, receiveEndpoint)
                        .WithCancellation(cancellationToken)
                        .ConfigureAwait(false);

                    if (result.RemoteEndPoint as IPEndPoint is not IPEndPoint remoteEndpoint)
                    {
                        _logger.LogWarning(
                            "received {MessageLength} byte message from invalid source: {RemoteEndpoint}",
                            result.ReceivedBytes,
                            result.RemoteEndPoint);

                        continue;
                    }
                    _logger.LogTrace(
                        "received {MessageLength} byte message from {RemoteEndpoint}",
                        result.ReceivedBytes,
                        remoteEndpoint);

                    var client = new ClientInfo((IPEndPoint)listener.LocalEndPoint, remoteEndpoint);

                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var (success, backend, reason) = await router.RouteMessageAsync(
                                buffer[..result.ReceivedBytes],
                                client,
                                cancellationToken).ConfigureAwait(false);

                            if (success)
                            {
                                _logger.LogInformation(
                                    "{MessageLength} byte message from {RemoteEndpoint} routed to {Backend}",
                                    result.ReceivedBytes,
                                    client.RemoteEndpoint,
                                    backend!.Spec.Name);
                            }
                            else
                            {
                                _logger.LogInformation(
                                    "failed to route {MessageLength} byte message from {RemoteEndpoint}: {Reason}",
                                    result.ReceivedBytes,
                                    client.RemoteEndpoint,
                                    reason);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(
                                ex,
                                "error routing {MessageLength} byte message from {RemoteEndpoint}",
                                result.ReceivedBytes,
                                client.RemoteEndpoint);
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "error listening for {NetworkProtocol} message", networkProtocol);
                }
            }
        }
        finally
        {
            TeardownSocket(listener, $"{networkProtocol} listener");

            _logger.LogTrace("{NetworkProtocol} listener worker stopped", networkProtocol);
        }
    }

    private async Task TeardownAsync()
    {
        _workerCts.Cancel();

        await Task.WhenAll(
            from boundListener in _listeners.Values
            select boundListener.Worker).ConfigureAwait(false);

        foreach (var boundListener in _listeners.Values)
        {
            boundListener.TokenSource.Dispose();
        }
    }

    private void TeardownSocket(Socket socket, string name = "socket")
    {
        try
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                _logger.LogTrace("{Name} shutdown", name);

                socket.Close();
                _logger.LogTrace("{Name} closed", name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "error tearing down {Name}", name);
        }
        finally
        {
            socket.Dispose();
            _logger.LogTrace("{Name} disposed", name);
        }
    }
}
