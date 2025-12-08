using System;

namespace Pmmux.Abstractions;

/// <summary>
/// Result of a health check operation.
/// </summary>
/// <param name="IsSuccess"><c>true</c> if the health check succeeded; otherwise, <c>false</c>.</param>
/// <param name="Reason">Optional human-readable reason describing the result.</param>
/// <param name="Exception">Optional exception if the health check failed due to an error.</param>
public record HealthCheckResult(bool IsSuccess, string? Reason = null, Exception? Exception = null)
{
    /// <summary>
    /// Create a successful health check result.
    /// </summary>
    /// <param name="reason">Optional description of the successful check.</param>
    /// <returns>A <see cref="HealthCheckResult"/> indicating success.</returns>
    public static HealthCheckResult Healthy(string? reason = null) => new(true, reason);

    /// <summary>
    /// Create a failed health check result.
    /// </summary>
    /// <param name="reason">The description of why the check failed.</param>
    /// <param name="exception">Optional exception that caused the failure.</param>
    /// <returns>A <see cref="HealthCheckResult"/> indicating failure.</returns>
    public static HealthCheckResult Unhealthy(string reason, Exception? exception = null) =>
        new(false, reason, exception);
}
