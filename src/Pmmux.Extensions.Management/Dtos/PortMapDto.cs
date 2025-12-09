using Mono.Nat;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Management.Dtos;

/// <summary>
/// A DTO object representing a port mapping.
/// </summary>
/// <param name="NetworkProtocol">The network protocol being forwarded.</param>
/// <param name="PublicAddress">The public IP address.</param>
/// <param name="PublicPort">The public port.</param>
/// <param name="LocalPort">The local port that receives forwarded traffic.</param>
/// <param name="NatProtocol">The NAT protocol used to create the mapping.</param>
public record PortMapDto(
    Protocol NetworkProtocol,
    string PublicAddress,
    int PublicPort,
    int LocalPort,
    NatProtocol NatProtocol)
{
    /// <summary>
    /// Create a DTO from a <see cref="PortMapInfo"/>.
    /// </summary>
    /// <param name="info">The port map info.</param>
    /// <returns>A DTO object.</returns>
    public static PortMapDto FromPortMapInfo(PortMapInfo info) => new(
        info.NetworkProtocol,
        info.PublicEndpoint.Address.ToString(),
        info.PublicEndpoint.Port,
        info.LocalPort,
        info.NatProtocol);
}

