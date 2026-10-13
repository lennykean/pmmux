using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;

using QBittorrent.Client;

namespace Pmmux.Extensions.QBittorrent;

internal sealed class QBittorrentSynchronizer(
    QBittorrentConfig config,
    IPortWarden warden,
    IEventNotifier events,
    IQBittorrentClient client,
    IMetricReporter metrics,
    ILogger<QBittorrentSynchronizer> logger,
    TimeProvider? timeProvider = null) : BackgroundService
{
    private readonly TimeProvider _time = timeProvider ?? TimeProvider.System;

    private TaskCompletionSource _signal = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private SyncStatus _status = new("starting");
    private PortMapInfo[] _maps = [];
    private int _mappingRevision;
    private int _ownedMappingRevision;

    public SyncStatus Status => Volatile.Read(ref _status);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        events.PortMapAdded += MappingChanged;
        events.PortMapChanged += MappingChanged;
        events.PortMapRemoved += MappingChanged;

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var signal = Volatile.Read(ref _signal);

                await ReconcileAsync(stoppingToken).ConfigureAwait(false);

                if (Status.State is "api-error" or "verification-failed" or "mapping-error" or "mapping-mismatch")
                {
                    await Task.Delay(config.Interval, _time, stoppingToken).ConfigureAwait(false);
                }
                else
                {
                    try
                    {
                        await signal.Task.WaitAsync(config.Interval, _time, stoppingToken).ConfigureAwait(false);
                    }
                    catch (TimeoutException)
                    {
                    }
                }

                Interlocked.CompareExchange(
                    ref _signal,
                    new(TaskCreationOptions.RunContinuationsAsynchronously),
                    signal);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        finally
        {
            events.PortMapAdded -= MappingChanged;
            events.PortMapChanged -= MappingChanged;
            events.PortMapRemoved -= MappingChanged;
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            try
            {
                await ReleaseMappingsAsync(timeout.Token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                logger.LogWarning("qBittorrent mapping cleanup failed; mappings remain managed by PortWarden");
            }

            SetStatus(new("stopped"));
        }
    }

    internal async Task ReconcileAsync(CancellationToken token)
    {
        int? observed = null;
        int? desired = null;

        try
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(token);
            timeout.CancelAfter(TimeSpan.FromSeconds(10));
            desired = await EnsureMappingsAsync(timeout.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            SetStatus(new("mapping-error"));
            return;
        }

        if (desired is null)
        {
            return;
        }

        try
        {
            if (config.Username is { } username && config.PasswordEnvironment is { } variable)
            {
                await client.LoginAsync(username, Environment.GetEnvironmentVariable(variable), token)
                    .ConfigureAwait(false);
            }

            var preferences = await client.GetPreferencesAsync(token).ConfigureAwait(false);
            observed = preferences.ListenPort;

            if (!StillCurrent(desired.Value))
            {
                return;
            }
            if (preferences.ListenPort != desired.Value || preferences.RandomPort != false ||
                preferences.UpnpEnabled != false)
            {
                await client.SetPreferencesAsync(new Preferences
                {
                    ListenPort = desired.Value,
                    RandomPort = false,
                    UpnpEnabled = false
                }, token).ConfigureAwait(false);
                preferences = await client.GetPreferencesAsync(token).ConfigureAwait(false);
                observed = preferences.ListenPort;
            }
            if (!StillCurrent(desired.Value))
            {
                return;
            }
            if (preferences.ListenPort != desired.Value || preferences.RandomPort != false ||
                preferences.UpnpEnabled != false)
            {
                SetStatus(new("verification-failed", desired, observed, Status.LastVerifiedAt));
                return;
            }

            SetStatus(new("synchronized", desired.Value, observed, _time.GetUtcNow()));
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            SetStatus(new("api-error", desired, observed, Status.LastVerifiedAt));
        }
    }

    private void MappingChanged(object? sender, Mapping mapping)
    {
        if (Volatile.Read(ref _maps).Any(m =>
            m.NetworkProtocol == mapping.Protocol && m.LocalPort == mapping.PrivatePort &&
            m.PublicEndpoint.Port != mapping.PublicPort))
        {
            Interlocked.Increment(ref _mappingRevision);
        }

        Volatile.Read(ref _signal).TrySetResult();
    }

    private bool StillCurrent(int desired)
    {
        if (_ownedMappingRevision == Volatile.Read(ref _mappingRevision) &&
            _maps.Length == 2 && _maps.All(m => m.LocalPort == desired) &&
            _maps.All(warden.GetPortMaps().Contains))
        {
            return true;
        }

        SetStatus(new("waiting-for-mapping"));
        return false;
    }

    private async Task<int?> EnsureMappingsAsync(CancellationToken token)
    {
        PortMapInfo[] maps;

        try
        {
            maps = warden.GetPortMaps().ToArray();
        }
        catch (InvalidOperationException)
        {
            SetStatus(new("waiting-for-mapping"));
            return null;
        }

        var revision = Volatile.Read(ref _mappingRevision);

        if (_ownedMappingRevision == revision && _maps.Length == 2 && _maps[0].NetworkProtocol == Protocol.Tcp &&
            _maps[1].NetworkProtocol == Protocol.Udp && _maps[0].LocalPort == _maps[1].LocalPort &&
            _maps[0].PublicEndpoint.Equals(_maps[1].PublicEndpoint) && _maps.All(maps.Contains))
        {
            return _maps[0].LocalPort;
        }

        await ReleaseMappingsAsync(token).ConfigureAwait(false);

        var tcp = await warden.AddPortMapAsync(Protocol.Tcp, config.PeerPort, config.PeerPort, token)
            .ConfigureAwait(false);

        if (tcp is null)
        {
            SetStatus(new("waiting-for-mapping"));
            return null;
        }

        Volatile.Write(ref _maps, [tcp]);

        if (tcp.NetworkProtocol != Protocol.Tcp || tcp.LocalPort is < 1 or > 65535 ||
            tcp.PublicEndpoint.Port != tcp.LocalPort || config.PeerPort is { } port && tcp.LocalPort != port)
        {
            await ReleaseMappingsAsync(token).ConfigureAwait(false);
            SetStatus(new("mapping-mismatch"));
            return null;
        }

        try
        {
            var udp = await warden.AddPortMapAsync(Protocol.Udp, tcp.LocalPort, tcp.PublicEndpoint.Port, token)
                .ConfigureAwait(false);

            if (udp is not null)
            {
                Volatile.Write(ref _maps, [tcp, udp]);

                if (udp.NetworkProtocol == Protocol.Udp && udp.LocalPort == tcp.LocalPort &&
                    udp.PublicEndpoint.Equals(tcp.PublicEndpoint))
                {
                    _ownedMappingRevision = revision;
                    return tcp.LocalPort;
                }
            }
        }
        catch
        {
            await ReleaseMappingsAsync(token).ConfigureAwait(false);
            throw;
        }

        await ReleaseMappingsAsync(token).ConfigureAwait(false);
        SetStatus(new("mapping-mismatch"));
        return null;
    }

    private async Task ReleaseMappingsAsync(CancellationToken token)
    {
        if (_maps.Length == 0)
        {
            return;
        }

        var current = warden.GetPortMaps().ToArray();

        foreach (var map in _maps)
        {
            if (current.Contains(map) && !await warden.RemovePortMapAsync(
                map.NetworkProtocol, map.LocalPort, map.PublicEndpoint.Port, token).ConfigureAwait(false))
            {
                throw new InvalidOperationException("qBittorrent mapping cleanup failed.");
            }

            Volatile.Write(ref _maps, [.. _maps.Where(m => m != map)]);
        }
    }

    private void SetStatus(SyncStatus status)
    {
        status = status with { LastVerifiedAt = status.LastVerifiedAt ?? Status.LastVerifiedAt };
        var previous = Interlocked.Exchange(ref _status, status);

        if (previous.State != status.State || previous.MappedPort != status.MappedPort)
        {
            logger.LogInformation(
                "qBittorrent synchronization: {State}; mapped port {MappedPort}; application port {ApplicationPort}",
                status.State,
                status.MappedPort,
                status.ApplicationPort);
        }

        metrics.ReportGauge("qbittorrent.synchronized", "qbittorrent", status.State == "synchronized" ? 1 : 0, []);
        metrics.ReportGauge("qbittorrent.mapped_port", "qbittorrent", status.MappedPort ?? 0, []);
        metrics.ReportGauge("qbittorrent.application_port", "qbittorrent", status.ApplicationPort ?? 0, []);
        metrics.ReportGauge(
            "qbittorrent.last_verified", "qbittorrent", status.LastVerifiedAt?.ToUnixTimeSeconds() ?? 0, []);
    }
}
