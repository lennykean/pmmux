using System.Collections.Generic;
using System.Net;

using Mono.Nat;

namespace Pmmux.Core.Configuration;

/// <summary>Configuration for TCP/UDP listeners.</summary>
public record ListenerConfig
{
    /// <summary>
    /// Represents a port binding configuration for a listener.
    /// </summary>
    /// <param name="NetworkProtocol">Network protocol (TCP or UDP) for the binding.</param>
    /// <param name="Port">Local port number to bind the listener to.</param>
    /// <param name="Index">The index that correlates this binding to a port map configuration.</param>
    /// <param name="BindAddress">The IP address to bind the listener to.</param>
    /// <param name="Listen">Whether the multiplexer should create a listener for this binding.</param>
    public record BindingConfig(
        Protocol NetworkProtocol,
        int? Port,
        int? Index = null,
        IPAddress? BindAddress = null,
        bool Listen = true);

    /// <summary>List of port bindings for the listener.</summary>
    public IEnumerable<BindingConfig> PortBindings { get; init; } = [];
    /// <summary>Maximum queue length for incoming connections.</summary>
    public int QueueLength { get; init; } = 100;
}
