using Mono.Nat;

namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// Port mapping information DTO.
/// </summary>
public record PortMapInfoDto
{
    /// <summary>The network protocol being forwarded.</summary>
    public Protocol NetworkProtocol { get; set; }

    /// <summary>The public IP address.</summary>
    public required string PublicAddress { get; set; }

    /// <summary>The public port.</summary>
    public int PublicPort { get; set; }

    /// <summary>The local port that receives forwarded traffic.</summary>
    public int LocalPort { get; set; }

    /// <summary>The NAT protocol used to create the mapping.</summary>
    public required string NatProtocol { get; set; }
}
