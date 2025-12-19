using System.Collections.Generic;

namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// Health check specification DTO for API requests.
/// </summary>
public class HealthCheckSpecDto
{
    /// <summary>Protocol name (null for all protocols).</summary>
    public string? ProtocolName { get; set; }

    /// <summary>Backend name (null for all backends).</summary>
    public string? BackendName { get; set; }

    /// <summary>Initial delay before first health check in milliseconds.</summary>
    public int? InitialDelay { get; set; }

    /// <summary>Interval between health checks in milliseconds.</summary>
    public int? Interval { get; set; }

    /// <summary>Timeout for health checks in milliseconds.</summary>
    public int? Timeout { get; set; }

    /// <summary>Number of failures before marking unhealthy.</summary>
    public int? FailureThreshold { get; set; }

    /// <summary>Number of successes before marking healthy.</summary>
    public int? RecoveryThreshold { get; set; }

    /// <summary>Health check parameters.</summary>
    public Dictionary<string, string>? Parameters { get; set; }
}
