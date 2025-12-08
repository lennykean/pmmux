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
    public record BindingConfig(Protocol NetworkProtocol, int Port);

    /// <summary>IP address to bind listeners to.</summary>
    public IPAddress BindAddress { get; init; } = IPAddress.Any;
    /// <summary>List of port bindings for the listener.</summary>
    public IEnumerable<BindingConfig> PortBindings { get; init; } = [];
    /// <summary>Maximum queue length for incoming connections.</summary>
    public int QueueLength { get; init; } = 100;
}
