using System;
using System.Collections.Generic;

using Pmmux.Abstractions;

namespace Pmmux.Core.Configuration;

/// <summary>Configuration for the router including routing strategy and backends.</summary>
public record RouterConfig
{
    /// <summary>Name of the routing strategy to use.</summary>
    public string RoutingStrategy { get; init; } = "first-available";
    /// <summary>Timeout for backend selection.</summary>
    public TimeSpan SelectionTimeout { get; init; } = TimeSpan.FromSeconds(5);
    /// <summary>List of backend specifications.</summary>
    public IEnumerable<BackendSpec> Backends { get; init; } = [];
    /// <summary>List of health check specifications.</summary>
    public IEnumerable<HealthCheckSpec> HealthChecks { get; init; } = [];
    /// <summary>Maximum size of the connection preview buffer.</summary>
    public long? PreviewSizeLimit { get; init; }
}
