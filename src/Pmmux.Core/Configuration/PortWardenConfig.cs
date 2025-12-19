using System;
using System.Collections.Generic;
using System.Net;

using Mono.Nat;

namespace Pmmux.Core.Configuration;

/// <summary>Configuration for NAT port mapping (UPnP/PMP).</summary>
public record PortWardenConfig
{
    /// <summary>
    /// Represents a NAT port mapping configuration.
    /// </summary>
    /// <param name="NetworkProtocol">Network protocol (TCP or UDP) for the port mapping.</param>
    /// <param name="LocalPort">Local port to map.</param>
    /// <param name="PublicPort">Public port to request on the NAT device.</param>
    /// <param name="Index">The index that correlates this mapping to a listener binding configuration.</param>
    public record PortMapConfig(
        Protocol NetworkProtocol,
        int? LocalPort = null,
        int? PublicPort = null,
        int? Index = null);

    /// <summary>List of port mappings to create.</summary>
    public IEnumerable<PortMapConfig> PortMaps { get; init; } = [];
    /// <summary>NAT protocol to use (UPnP or PMP).</summary>
    public NatProtocol? NatProtocol { get; init; }
    /// <summary>Requested lifetime for port mappings.</summary>
    public TimeSpan? Lifetime { get; init; }
    /// <summary>Time before expiration to renew port mappings.</summary>
    public TimeSpan RenewalLead { get; init; } = TimeSpan.FromSeconds(10);
    /// <summary>Gateway address to search for NAT devices.</summary>
    public IPAddress GatewayAddress { get; init; } = IPAddress.Any;
    /// <summary>Network interface to use for NAT discovery.</summary>
    public string? NetworkInterface { get; init; }
    /// <summary>Timeout for NAT device discovery.</summary>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromMilliseconds(5000);
}
