using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Acme;

internal sealed class HttpChallengeBatch(
    IRouter router,
    IPortMultiplexer portMultiplexer,
    IPortWarden portWarden,
    HttpChallengeBackend.Protocol backendProtocol,
    ILogger logger) : IAsyncDisposable
{
    private const int HttpPort = 80;

    private int _localPort = HttpPort;
    private bool _createdListener;
    private PortMapInfo? _createdPortMap;
    private BackendInfo? _backendInfo;

    public HttpChallengeBackend Backend { get; private set; } = null!;

    public async Task EnsureInfrastructureAsync(
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken)
    {
        if (_backendInfo is not null)
        {
            return;
        }

        await EnsurePortMapAsync(properties, cancellationToken).ConfigureAwait(false);
        await EnsureListenerAsync(cancellationToken).ConfigureAwait(false);
        await EnsureBackendAsync(cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_backendInfo is not null)
        {
            try
            {
                await router.RemoveBackendAsync(Protocol.Tcp, _backendInfo, forceCloseConnections: true)
                    .ConfigureAwait(false);
                logger.LogDebug("removed acme-http-challenge backend");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "failed to remove acme-http-challenge backend");
            }
        }

        if (_createdPortMap is not null)
        {
            try
            {
                await portWarden.RemovePortMapAsync(
                    _createdPortMap.NetworkProtocol,
                    _createdPortMap.LocalPort,
                    _createdPortMap.PublicEndpoint.Port)
                    .ConfigureAwait(false);
                logger.LogDebug("removed port {Port} NAT mapping", HttpPort);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "failed to remove port {Port} NAT mapping", HttpPort);
            }
        }

        if (_createdListener)
        {
            try
            {
                await portMultiplexer.RemoveListenerAsync(Protocol.Tcp, _localPort).ConfigureAwait(false);
                logger.LogDebug("removed port {Port} listener", _localPort);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "failed to remove port {Port} listener", _localPort);
            }
        }
    }

    private async Task EnsurePortMapAsync(
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken)
    {
        var autoMap = !properties.TryGetValue("auto-map", out var autoMapValue) ||
            !string.Equals(autoMapValue, "false", StringComparison.OrdinalIgnoreCase);

        if (!autoMap)
        {
            logger.LogDebug("auto-map disabled, skipping port mapping");
            return;
        }

        PortMapInfo? portMap;
        try
        {
            var existingMaps = portWarden.GetPortMaps();
            var hasPort80Map = existingMaps.Any(m =>
                m.NetworkProtocol == Protocol.Tcp &&
                m.PublicEndpoint.Port == HttpPort);

            if (hasPort80Map)
            {
                logger.LogDebug("port {Port} mapping already exists", HttpPort);
                return;
            }

            var freePort = FindFreePort();

            logger.LogInformation(
                "creating port {Port} NAT mapping for http-01 challenges (local:{LocalPort})",
                HttpPort,
                freePort);
            portMap = await portWarden.AddPortMapAsync(
                Protocol.Tcp,
                localPort: freePort,
                publicPort: HttpPort,
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is InvalidOperationException or MappingException)
        {
            logger.LogWarning(ex, "port mapping unavailable, continuing without NAT mapping");
            return;
        }

        if (portMap is null)
        {
            logger.LogWarning("failed to create port {Port} NAT mapping, continuing without it", HttpPort);
            return;
        }

        _createdPortMap = portMap;
        _localPort = portMap.LocalPort;
        logger.LogDebug("created port mapping {PublicEndpoint} -> local:{LocalPort}", portMap.PublicEndpoint, portMap.LocalPort);
    }

    private async Task EnsureListenerAsync(CancellationToken cancellationToken)
    {
        var listeners = await portMultiplexer.GetListenersAsync(cancellationToken).ConfigureAwait(false);
        var hasListener = listeners.Any(l =>
            l.NetworkProtocol == Protocol.Tcp &&
            l.LocalEndPoint.Port == _localPort);

        if (hasListener)
        {
            logger.LogDebug("port {Port} listener already exists", _localPort);
            return;
        }

        logger.LogInformation("creating port {Port} listener for http-01 challenges", _localPort);
        var listener = portMultiplexer.AddListener(Protocol.Tcp, _localPort);

        if (listener is null)
        {
            throw new InvalidOperationException($"failed to create listener on port {_localPort}");
        }

        _createdListener = true;
        logger.LogDebug("created listener on {Endpoint}", listener.LocalEndPoint);
    }

    private async Task EnsureBackendAsync(CancellationToken cancellationToken)
    {
        var spec = new BackendSpec(
            "acme-http-challenge",
            HttpChallengeBackend.ProtocolName,
            new Dictionary<string, string>
            {
                ["priority"] = "vip",
                ["local-port"] = _localPort.ToString()
            });

        var backendInfo = await router.AddBackendAsync(Protocol.Tcp, spec, cancellationToken)
            .ConfigureAwait(false);

        _backendInfo = backendInfo;
        Backend = backendProtocol.LastCreated
            ?? throw new InvalidOperationException("backend was not created by the protocol factory");
        logger.LogDebug("registered acme-http-challenge backend on local port {Port}", _localPort);
    }

    private static int FindFreePort()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));

        return ((IPEndPoint)socket.LocalEndPoint!).Port;
    }
}
