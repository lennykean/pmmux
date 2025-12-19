using System;
using System.Collections.Generic;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// Backend runtime information DTO.
/// </summary>
public record BackendStatusInfoDto
{
    /// <summary>The specification that defines the backend.</summary>
    public required BackendSpecDto Spec { get; set; }

    /// <summary>Runtime properties provided by the backend implementation.</summary>
    public Dictionary<string, string> Properties { get; set; } = [];

    /// <summary>The routing priority tier.</summary>
    public PriorityTier PriorityTier { get; set; }

    /// <summary>The backend status.</summary>
    public BackendStatus Status { get; set; }

    /// <summary>The reason for the current status.</summary>
    public string? StatusReason { get; set; }

    /// <summary>The timestamp of the last health check.</summary>
    public DateTime? LastHealthCheck { get; set; }

    /// <summary>The number of consecutive health check failures.</summary>
    public long? HealthCheckFailureCount { get; set; }

    /// <summary>The number of consecutive health check successes.</summary>
    public long? HealthCheckSuccessCount { get; set; }
}
