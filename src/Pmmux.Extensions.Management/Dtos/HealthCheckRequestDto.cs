using System;
using System.Collections.Generic;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Management.Dtos;

/// <summary>
/// A DTO for health check requests.
/// </summary>
public record HealthCheckRequestDto(
    string? ProtocolName,
    string? BackendName,
    TimeSpan? InitialDelay,
    TimeSpan? Interval,
    TimeSpan? Timeout,
    int? FailureThreshold,
    int? RecoveryThreshold,
    Dictionary<string, string>? Parameters)
{
    /// <summary>
    /// Convert to a <see cref="HealthCheckSpec"/>.
    /// </summary>
    public HealthCheckSpec ToHealthCheckSpec() => new(Parameters ?? [])
    {
        ProtocolName = ProtocolName,
        BackendName = BackendName,
        InitialDelay = InitialDelay ?? TimeSpan.Zero,
        Interval = Interval ?? TimeSpan.FromSeconds(10),
        Timeout = Timeout ?? TimeSpan.FromSeconds(5),
        FailureThreshold = FailureThreshold ?? 3,
        RecoveryThreshold = RecoveryThreshold ?? 5,
    };
}

