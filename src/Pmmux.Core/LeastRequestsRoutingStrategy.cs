using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal sealed class LeastRequestsRoutingStrategy : IRoutingStrategy, IDisposable
{
    private class Tracker
    {
        public long RequestCount;
    }

    private readonly ConcurrentDictionary<BackendInfo, Tracker> _backends = new();
    private readonly IEventNotifier _eventNotifier;

    /// <summary>Creates a new least-requests routing strategy.</summary>
    /// <param name="eventNotifier">The event notifier to subscribe to for backend updates.</param>
    public LeastRequestsRoutingStrategy(IEventNotifier eventNotifier)
    {
        _eventNotifier = eventNotifier;
        _eventNotifier.BackendAdded += OnBackendAdded;
    }

    public string Name => "least-requests";

    public async Task<BackendInfo?> SelectBackendAsync(
        ClientInfo client,
        IReadOnlyDictionary<string, string> metadata,
        IAsyncEnumerable<BackendStatusInfo> matchedBackends,
        CancellationToken cancellationToken = default)
    {
        var backends = await matchedBackends.ToListAsync(cancellationToken).ConfigureAwait(false);
        if (backends.Count == 0)
        {
            return null;
        }

        var maxTier = backends.Max(b => b.Backend.PriorityTier);
        var maxTierBackends = backends.Where(b => b.Backend.PriorityTier == maxTier).ToList();

        if (maxTier == PriorityTier.Fallback)
        {
            return maxTierBackends.FirstOrDefault()?.Backend;
        }

        BackendInfo? selectedBackend = null;
        long minRequestCount = long.MaxValue;

        foreach (var backendStatus in maxTierBackends)
        {
            var tracker = _backends.GetOrAdd(backendStatus.Backend, _ => new Tracker());
            var currentRequestCount = Interlocked.Read(ref tracker.RequestCount);

            if (currentRequestCount < minRequestCount)
            {
                minRequestCount = currentRequestCount;
                selectedBackend = backendStatus.Backend;
            }
        }

        if (selectedBackend == null)
        {
            return null;
        }

        var selectedTracker = _backends[selectedBackend];
        Interlocked.Increment(ref selectedTracker.RequestCount);

        return selectedBackend;
    }

    public void Dispose()
    {
        _eventNotifier.BackendAdded -= OnBackendAdded;
        _backends.Clear();
    }

    private void OnBackendAdded(object sender, BackendSpec e)
    {
        _backends.Clear();
    }
}
