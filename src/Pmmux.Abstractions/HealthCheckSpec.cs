using System;
using System.Collections.Generic;

using Pmmux.Abstractions.Utilities;

namespace Pmmux.Abstractions;

/// <summary>
/// Specifies how to perform health checks on backends.
/// </summary>
/// <remarks>
/// Health check specifications configure the behavior of <see cref="IBackendMonitor"/>.
/// Specifications can target:
/// <list type="bullet">
/// <item>All backends if neither <see cref="ProtocolName"/> nor <see cref="BackendName"/> is specified</item>
/// <item>Backends of a specific protocol if only <see cref="ProtocolName"/> is specified</item>
/// <item>A specific backend if both <see cref="ProtocolName"/> and <see cref="BackendName"/> are specified</item>
/// </list>
/// Uses fully value-based equality.
/// </remarks>
public record HealthCheckSpec
{
    /// <param name="parameters">
    /// Backend-specific health check parameters.
    /// </param>
    /// <remarks>
    /// Parameters will be converted to a value based dictionary to support value-based equality.
    /// </remarks>
    public HealthCheckSpec(IDictionary<string, string> parameters)
    {
        Parameters = new EquatableDictionary<string, string>(parameters);
    }

    /// <summary>
    /// Protocol of backends to target, or <c>null</c> to match all protocols.
    /// </summary>
    public string? ProtocolName { get; init; } = null;

    /// <summary>
    /// Name of a specific backend to target or <c>null</c> to match all backends of the specified protocol.
    /// </summary>
    public string? BackendName { get; init; } = null;

    /// <summary>
    /// Initial delay before starting health checks.
    /// </summary>
    /// <remarks>
    /// Defaults to zero.
    /// </remarks>
    public TimeSpan InitialDelay { get; init; } = TimeSpan.Zero;

    /// <summary>
    /// Interval between health checks.
    /// </summary>
    /// <remarks>
    /// Defaults to 10 seconds. Health checks run continuously at this interval while the backend is monitored.
    /// </remarks>
    public TimeSpan Interval { get; init; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Timeout for each health check operation.
    /// </summary>
    /// <remarks>
    /// Defaults to 5 seconds.
    /// </remarks>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Number of consecutive failures required to mark a backend as unhealthy.
    /// </summary>
    /// <remarks>
    /// Defaults to 3.
    /// </remarks>
    public int FailureThreshold { get; init; } = 3;

    /// <summary>
    /// Number of consecutive successes required to mark an unhealthy backend as healthy again.
    /// </summary>
    /// <remarks>
    /// Defaults to 5.
    /// </remarks>
    public int RecoveryThreshold { get; init; } = 5;

    /// <summary>
    /// Backend-specific health check parameters.
    /// </summary>
    public IReadOnlyDictionary<string, string> Parameters { get; }
}
