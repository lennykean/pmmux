using System;
using System.Collections.Generic;
using System.Threading;

using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Monitors backend health status based on configured health check specifications.
/// </summary>
/// <remarks>
/// Backend monitor performs health checks on backends that implement <see cref="IHealthCheckBackend"/>.
/// Health check results can help determine whether a backend is available for routing.
/// </remarks>
public interface IBackendMonitor : IAsyncDisposable
{
    /// <summary>
    /// Monitor the health of a backend and provide status updates as an async stream.
    /// </summary>
    /// <param name="backend">The backend to monitor.</param>
    /// <param name="networkProtocol">The network protocol used for health checks.</param>
    /// <param name="cancellationToken">Token to stop monitoring this backend.</param>
    /// <param name="enumeratorCancellationToken">Token for controlling enumeration of the stream.</param>
    IAsyncEnumerable<BackendStatusInfo> MonitorAsync(
        IHealthCheckBackend backend,
        Protocol networkProtocol,
        CancellationToken cancellationToken = default,
        CancellationToken enumeratorCancellationToken = default);

    /// <summary>
    /// Get all currently configured health check specifications.
    /// </summary>
    IEnumerable<HealthCheckSpec> GetHealthChecks();

    /// <summary>
    /// Add a new health check specification at runtime.
    /// </summary>
    /// <param name="healthCheckSpec">The health check specification to add.</param>
    /// <returns>
    /// <c>true</c> if the specification was added, or <c>false</c> if the specification already exists.
    /// </returns>
    bool TryAddHealthCheck(HealthCheckSpec healthCheckSpec);

    /// <summary>
    /// Remove an existing health check specification at runtime.
    /// </summary>
    /// <param name="healthCheckSpec">The health check specification to remove.</param>
    /// <returns>
    /// <c>true</c> if the specification was removed, <c>false</c> if no matching specification exists.
    /// </returns>
    bool TryRemoveHealthCheck(HealthCheckSpec healthCheckSpec);
}
