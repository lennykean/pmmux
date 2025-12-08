using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Optional backend interface for health checking support.
/// </summary>
/// <remarks>
/// Backends implementing this interface can be monitored by <see cref="IBackendMonitor"/> to
/// continuously assess health status. Health check results influence routing decisions - unhealthy
/// backends are typically excluded from receiving traffic. Health checks are configured via
/// <see cref="HealthCheckSpec"/> using the <c>--health-check</c> CLI option or configuration file.
/// </remarks>
public interface IHealthCheckBackend : IBackend
{
    /// <summary>
    /// Perform a health check to determine operational status.
    /// </summary>
    /// <param name="networkProtocol">The network protocol used to perform the health check.</param>
    /// <param name="parameters">
    /// Health check configuration parameters from <see cref="HealthCheckSpec.Parameters"/>.
    /// Common parameters include <c>timeout</c>, <c>interval</c>, and protocol-specific options.
    /// </param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="HealthCheckResult"/> indicating the backend's current health status.
    /// </returns>
    Task<HealthCheckResult> HealthCheckAsync(
        Protocol networkProtocol,
        IReadOnlyDictionary<string, string> parameters,
        CancellationToken cancellationToken = default);
}
