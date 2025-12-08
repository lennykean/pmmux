using System;

namespace Pmmux.Abstractions;

/// <summary>
/// Health status and statistics for a backend.
/// </summary>
/// <param name="Backend">The backend that the status information pertains to.</param>
/// <param name="Status">The current health status.</param>
/// <param name="StatusReason">Optional reason for the current status.</param>
/// <param name="LastHealthCheck">The timestamp of the most recent health check if one has been performed.</param>
/// <param name="HealthCheckFailureCount">The number of most recent successive failed health checks.</param>
/// <param name="HealthCheckSuccessCount">The number of most recent successive successful health checks.</param>
public record BackendStatusInfo(
    BackendInfo Backend,
    BackendStatus Status,
    string? StatusReason = null,
    DateTime? LastHealthCheck = null,
    long? HealthCheckFailureCount = null,
    long? HealthCheckSuccessCount = null);
