using System;
using System.Collections.Generic;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Management.Dtos;

/// <summary>
/// A DTO object representing backend runtime information.
/// </summary>
/// <param name="Spec">The specification that defines the backend.</param>
/// <param name="Properties">Runtime properties provided by the backend implementation.</param>
/// <param name="PriorityTier">The routing priority tier.</param>
/// <param name="Status">The backend status.</param>
/// <param name="StatusReason">The reason for the current status.</param>
/// <param name="LastHealthCheck">The timestamp of the last health check.</param>
/// <param name="HealthCheckFailureCount">The number of consecutive health check failures.</param>
/// <param name="HealthCheckSuccessCount">The number of consecutive health check successes.</param>
public record BackendInfoDto(
    BackendSpecDto Spec,
    IReadOnlyDictionary<string, string> Properties,
    PriorityTier PriorityTier,
    BackendStatus Status,
    string? StatusReason,
    DateTime? LastHealthCheck,
    long? HealthCheckFailureCount,
    long? HealthCheckSuccessCount)
{
    /// <summary>
    /// Create a DTO from a <see cref="BackendStatusInfo"/>.
    /// </summary>
    /// <param name="info">The backend status info.</param>
    /// <returns>A DTO object.</returns>
    public static BackendInfoDto FromBackendStatusInfo(BackendStatusInfo info) => new(
        BackendSpecDto.FromBackendSpec(info.Backend.Spec),
        info.Backend.Properties,
        info.Backend.PriorityTier,
        info.Status,
        info.StatusReason,
        info.LastHealthCheck,
        info.HealthCheckFailureCount,
        info.HealthCheckSuccessCount);

    /// <summary>
    /// Create a DTO from a <see cref="BackendInfo"/>.
    /// </summary>
    /// <param name="info">The backend info.</param>
    /// <returns>A DTO object.</returns>
    public static BackendInfoDto FromBackendInfo(BackendInfo info) => new(
        BackendSpecDto.FromBackendSpec(info.Spec),
        info.Properties,
        info.PriorityTier,
        BackendStatus.Unknown,
        null,
        null,
        null,
        null);
}

