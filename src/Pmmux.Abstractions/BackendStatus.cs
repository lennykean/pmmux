namespace Pmmux.Abstractions;

/// <summary>
/// Health status of a backend as tracked by <see cref="IBackendMonitor"/>.
/// </summary>
/// <remarks>
/// Backend status indicates the current operational state and availability.
/// The <see cref="IRoutingStrategy"/> uses this status to decide which backends
/// should receive traffic. Status transitions are managed by the backend monitor
/// based on health check results and configured thresholds.
/// </remarks>
public enum BackendStatus
{
    /// <summary>
    /// Backend's health status is unknown (no health checks performed yet).
    /// </summary>
    /// <remarks>
    /// Backends start in this state. Most routing strategies treat unknown as healthy.
    /// </remarks>
    Unknown = 0,

    /// <summary>
    /// Backend is healthy and available for routing traffic.
    /// </summary>
    Healthy,

    /// <summary>
    /// Backend is unhealthy and should not receive new traffic.
    /// </summary>
    /// <remarks>
    /// Backends enter this state after consecutive health check failures.
    /// </remarks>
    Unhealthy,

    /// <summary>
    /// Backend is partially healthy and may handle traffic with reduced reliability.
    /// </summary>
    /// <remarks>
    /// Use with caution. Consider preferring fully healthy backends when available.
    /// </remarks>
    Degraded,

    /// <summary>
    /// Backend is recovering from an unhealthy state.
    /// </summary>
    /// <remarks>
    /// Backends enter this state after passing a health check while unhealthy.
    /// They return to healthy after consecutive successful checks.
    /// </remarks>
    Stabilizing,

    /// <summary>
    /// Backend is draining active connections and should not receive new traffic.
    /// </summary>
    /// <remarks>
    /// Used during graceful shutdown. Existing connections are allowed to complete.
    /// </remarks>
    Draining,

    /// <summary>
    /// Backend has stopped and is not available.
    /// </summary>
    Stopped
}

