using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;
using Pmmux.Core.Configuration;

namespace Pmmux.Core;

/// <summary>
/// Default implementation of <see cref="IPortWarden"/>.
/// </summary>
/// <param name="eventSender">The event sender service.</param>
/// <param name="metricReporter">The metric reporter service.</param>
/// <param name="config">The port warden configuration.</param>
/// <param name="loggerFactory">The logger factory.</param>
public sealed class PortWarden(
    IEventSender eventSender,
    IMetricReporter metricReporter,
    PortWardenConfig config,
    ILoggerFactory loggerFactory) : IPortWarden
{
    private enum State
    {
        Initial = 0,
        Starting = 1,
        Started = 2,
        Dispose = 3
    }

    private readonly StateManager<State> _state = new(State.Initial);
    private readonly ConcurrentDictionary<Mapping, (Task RenewalWorker, Mapping Mapping)> _portMaps = [];
    private readonly TaskCompletionSource<bool> _disposedTcs = new();
    private readonly CancellationTokenSource _workerCts = new();
    private readonly ILogger _logger = loggerFactory.CreateLogger("port-warden");

    private INatDevice? _natDevice = null;

    /// <inheritdoc />
    public NatDeviceInfo? NatDevice => _natDevice?.DeviceInfo();

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (config is null)
        {
            throw new ArgumentNullException(nameof(config));
        }
        if (!_state.TryTransition(to: State.Starting, from: State.Initial))
        {
            throw new InvalidOperationException();
        }

        using var timeoutCts = new CancellationTokenSource(config.Timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

        try
        {
            if (config.PortMaps.Any())
            {
                var gateways = NetworkUtility.GetGateways(config.GatewayAddress, config.NetworkInterface);

                _logger.LogTrace("searching [{Gateways}] for [{NatProtocols}] NAT devices",
                    string.Join(",", gateways.Select(g => g.ToString())),
                    string.Join(",", config.NatProtocol is null
                         ? [NatProtocol.Upnp, NatProtocol.Pmp]
                         : [config.NatProtocol.Value]));

                _natDevice = await NetworkUtility.FindNatDeviceAsync(gateways, config.NatProtocol, linkedCts.Token)
                    .WithTimeout(timeoutCts.Token);

                var natDeviceInfo = _natDevice.DeviceInfo();

                _logger.LogDebug(
                    "{NatProtocol} device found at {NatEndpoint} with public address {NatDeviceAddress}",
                    natDeviceInfo.NatProtocol,
                    natDeviceInfo.Endpoint,
                    natDeviceInfo.PublicAddress);

                foreach (var portMapConfig in config.PortMaps)
                {
                    var portMap = await AddPortMapInternalAsync(
                        portMapConfig.NetworkProtocol,
                        portMapConfig.LocalPort,
                        portMapConfig.PublicPort,
                        linkedCts.Token).WithTimeout(timeoutCts.Token).ConfigureAwait(false);

                    _logger.LogInformation(
                        "{NetworkProtocol} port map created {PublicEndpoint}>{LocalPort}",
                        portMap?.NetworkProtocol,
                        portMap?.PublicEndpoint,
                        portMap?.LocalPort);
                }
            }
            _state.TryTransition(to: State.Started, from: State.Starting);
        }
        catch
        {
            try
            {
                linkedCts.Cancel();
                await Task.WhenAll(_portMaps.Values.Select(p => p.RenewalWorker)).ConfigureAwait(false);
                await Task.WhenAll(_portMaps.Keys
                    .Select(p => RemovePortMapAsync(p.Protocol, p.PrivatePort, p.PublicPort))
                    .ToArray()).ConfigureAwait(false);

                _portMaps.Clear();
            }
            finally
            {
                _state.TryTransition(to: State.Initial, from: State.Starting);
            }
            throw;
        }
    }

    /// <inheritdoc />
    public IEnumerable<PortMapInfo> GetPortMaps()
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(PortWarden));
        }
        if (!_state.Is(State.Started))
        {
            throw new InvalidOperationException();
        }
        return [.. (
            from portMap in _portMaps.Keys
            select new PortMapInfo(
                portMap.Protocol,
                new(NatDevice?.PublicAddress ?? IPAddress.None, portMap.PublicPort),
                portMap.PrivatePort,
                NatDevice!.NatProtocol))];
    }

    /// <inheritdoc />
    public Task<PortMapInfo?> AddPortMapAsync(
        Protocol networkProtocol,
        int? localPort,
        int? publicPort,
        CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(PortWarden));
        }
        if (!_state.Is(State.Started) || _natDevice is null)
        {
            throw new InvalidOperationException();
        }
        if (_portMaps.Keys.Any(p =>
            p.Protocol == networkProtocol && (
            p.PrivatePort == localPort || p.PublicPort == publicPort)))
        {
            throw new ArgumentException($"port map conflict");
        }
        return AddPortMapInternalAsync(networkProtocol, localPort, publicPort, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> RemovePortMapAsync(
        Protocol networkProtocol,
        int localPort,
        int publicPort,
        CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(PortWarden));
        }
        if (!_state.Is(State.Started) || _natDevice is null)
        {
            throw new InvalidOperationException();
        }
        if (!_portMaps.TryGetValue(new(networkProtocol, localPort, publicPort), out var portMap))
        {
            return Task.FromResult(false);
        }
        return RemovePortMapInternalAsync(portMap.Mapping, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_state.Is(State.Dispose) ||
            !_state.TryTransition(to: State.Dispose, from: [State.Started, State.Starting, State.Initial]))
        {
            await _disposedTcs.Task.ConfigureAwait(false);
            return;
        }

        _logger.LogTrace("port warden disposing");

        try
        {
            try
            {
                _workerCts.Cancel();
                await Task.WhenAll(_portMaps.Values.Select(p => p.RenewalWorker)).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }

            if (_natDevice is not null)
            {
                await Task.WhenAll(_portMaps.Keys.Select(pm => RemovePortMapInternalAsync(pm))).ConfigureAwait(false);
            }
            _logger.LogDebug("port warden disposed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "error disposing");

            _disposedTcs.SetException(ex);

            throw;
        }
        finally
        {
            _portMaps.Clear();
            _workerCts.Dispose();
            _natDevice = null;
        }
        _disposedTcs.SetResult(true);
    }

    private async Task StartRenewalWorkerAsync(Mapping portMap)
    {
        _logger.LogTrace("portmap renewal worker starting");

        var metadata = new Dictionary<string, string?>
        {
            ["protocol"] = portMap.Protocol.ToString(),
            ["local_port"] = portMap.PrivatePort.ToString(),
            ["public_port"] = portMap.PublicPort.ToString()
        };
        while (config is not null && _natDevice is not null && !_workerCts.IsCancellationRequested)
        {
            try
            {
                var delay = portMap.Expiration - DateTimeOffset.UtcNow;
                var maxDelay = TimeSpan.FromSeconds(portMap.Lifetime) - config.RenewalLead;
                var actualDelay = delay.Clamp(TimeSpan.FromSeconds(5), maxDelay);

                _logger.LogTrace("port map renewing in {Delay}", actualDelay);

                await Task.Delay(actualDelay, _workerCts.Token).ConfigureAwait(false);

                if (!_portMaps.TryGetValue(portMap, out var existingPortMap))
                {
                    _logger.LogDebug("portmap removed, stopping renewal");
                    break;
                }

                using var timeoutCts = new CancellationTokenSource(config.Timeout);

                var requestedPortMap = new Mapping(
                    protocol: portMap.Protocol,
                    privatePort: portMap.PrivatePort,
                    publicPort: portMap.PublicPort,
                    lifetime: (int)(config.Lifetime?.TotalSeconds ?? 0),
                    description: $"pmmux-{portMap.Protocol}".ToLowerInvariant());

                var updatedPortMap = await metricReporter.MeasureDurationAsync(
                    "portmap.renew.duration",
                    "port-warden",
                    metadata,
                    async () => await _natDevice.CreatePortMapAsync(requestedPortMap)
                        .WithCancellation(_workerCts.Token)
                        .WithTimeout(timeoutCts.Token)
                        .ConfigureAwait(false)).ConfigureAwait(false);

                _portMaps[portMap] = existingPortMap with { Mapping = updatedPortMap };
                portMap = updatedPortMap;

                metricReporter.ReportEvent("portmap.renewed", "port-warden", metadata);

                eventSender.RaisePortMapChanged(this, portMap);

                var natDeviceInfo = _natDevice.DeviceInfo();

                _logger.LogDebug("portmap renewed (exp: {Expiration:s})", portMap.Expiration.ToUniversalTime());
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                metricReporter.ReportEvent("portmap.renew.error", "port-warden", metadata);
                _logger.LogWarning(ex, "renewal worker error");
            }
        }

        _logger.LogTrace("renewal worker stopped");
    }

    private async Task<PortMapInfo?> AddPortMapInternalAsync(
       Protocol networkProtocol,
       int? localPort,
       int? publicPort,
       CancellationToken cancellationToken = default)
    {
        if (_natDevice is null)
        {
            return null;
        }

        var metadata = new Dictionary<string, string?>
        {
            ["protocol"] = networkProtocol.ToString(),
            ["local_port"] = localPort?.ToString(),
            ["public_port"] = publicPort?.ToString()
        };
        using (_logger.BeginScope("{NetworkProtocol} {PublicPort}>{LocalPort}", networkProtocol, publicPort, localPort))
        {
            _logger.LogTrace("port is being mapped");

            Mapping? portMap = null;
            try
            {
                var natDeviceInfo = _natDevice.DeviceInfo();

                var requestedPortMap = new Mapping(
                    networkProtocol,
                    localPort ?? 0,
                    publicPort ?? 0,
                    (int)(config.Lifetime?.TotalSeconds ?? 0),
                    $"pmmux-{networkProtocol}".ToLowerInvariant());

                portMap = await metricReporter.MeasureDurationAsync(
                   "portmap.create.duration",
                   "port-warden",
                   metadata,
                   async () => await _natDevice.CreatePortMapAsync(requestedPortMap)
                       .WithCancellation(cancellationToken)
                       .ConfigureAwait(false)).ConfigureAwait(false);

                _portMaps.AddOrUpdate(portMap,
                    p => (StartRenewalWorkerAsync(p), p),
                    (_, _) => throw new InvalidOperationException("port map conflict"));

                metricReporter.ReportEvent("portmap.created", "port-warden", metadata);
                _logger.LogDebug("port mapped (exp: {Expiration:s})", portMap.Expiration.ToUniversalTime());

                eventSender.RaisePortMapAdded(this, portMap);

                return new(
                    portMap.Protocol,
                    new(natDeviceInfo.PublicAddress, portMap.PublicPort),
                    portMap.PrivatePort,
                    natDeviceInfo.NatProtocol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "port mapping error");

                metricReporter.ReportEvent("portmap.add.error", "port-warden", new(metadata)
                {
                    ["error"] = ex.GetType().Name
                });
                if (portMap is not null)
                {
                    await RemovePortMapInternalAsync(portMap, cancellationToken).ConfigureAwait(false);
                }
                throw;
            }
        }
    }

    private async Task<bool> RemovePortMapInternalAsync(Mapping portMap, CancellationToken cancellationToken = default)
    {
        if (_natDevice is null)
        {
            return false;
        }
        using (_logger.BeginScope("{NetworkProtocol}", portMap.Protocol))
        {
            _logger.LogTrace("removing port map");

            var metadata = new Dictionary<string, string?>
            {
                ["protocol"] = portMap.Protocol.ToString()
            };
            try
            {
                await metricReporter.MeasureDurationAsync(
                    "portmap.delete.duration",
                    "port-warden",
                    metadata,
                    async () =>
                    {
                        await _natDevice.DeletePortMapAsync(portMap)
                            .WithCancellation(cancellationToken)
                            .ConfigureAwait(false);

                        return true;
                    }).ConfigureAwait(false);

                metricReporter.ReportEvent("portmap.deleted", "port-warden", metadata);

                eventSender.RaisePortMapRemoved(this, portMap);

                _logger.LogDebug("port map deleted");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "error deleting port map");

                metricReporter.ReportEvent("portmap.delete.error", "port-warden", new(metadata)
                {
                    ["error"] = ex.GetType().Name
                });
            }
        }
        return true;
    }
}

