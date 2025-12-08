using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal sealed class FirstAvailableRoutingStrategy : IRoutingStrategy
{
    public string Name => "first-available";

    public async Task<BackendInfo?> SelectBackendAsync(
        ClientInfo client,
        IReadOnlyDictionary<string, string> metadata,
        IAsyncEnumerable<BackendStatusInfo> matchedBackends,
        CancellationToken cancellationToken = default)
    {
        await foreach (var backend in matchedBackends.WithCancellation(cancellationToken))
        {
            return backend.Backend;
        }

        return null;
    }
}

