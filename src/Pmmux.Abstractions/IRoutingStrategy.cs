using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Strategy for selecting a backend from multiple candidates during routing.
/// </summary>
/// <remarks>
/// The router uses the configured routing strategy to select a single backend when multiple
/// backends match a client request. Backend matching is performed lazily via async enumeration,
/// allowing strategies to short-circuit once a suitable backend is found.
/// </remarks>
public interface IRoutingStrategy
{
    /// <summary>
    /// Name of the routing strategy.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Select a backend from matched candidates to handle the client request.
    /// </summary>
    /// <param name="client">The client making the request.</param>
    /// <param name="clientConnectionProperties">The connection metadata that may influence selection.</param>
    /// <param name="matchedBackends">
    /// Async stream of backends that matched the request, provided in the order in which they were defined.
    /// </param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// The selected backend's <see cref="BackendInfo"/> if a suitable backend was found,
    /// otherwise, <c>null</c> to indicate no backend is available.
    /// </returns>
    Task<BackendInfo?> SelectBackendAsync(
        ClientInfo client,
        IReadOnlyDictionary<string, string> clientConnectionProperties,
        IAsyncEnumerable<BackendStatusInfo> matchedBackends,
        CancellationToken cancellationToken = default);
}
