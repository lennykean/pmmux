using System.Net;

using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Represents information about a NAT port mapping between a public endpoint and a local port.
/// </summary>
/// <param name="NetworkProtocol">The network protocol being forwarded.</param>
/// <param name="PublicEndpoint">The public endpoint accessible from outside the LAN.</param>
/// <param name="LocalPort">The local port that receives traffic forwarded from the public port.</param>
/// <param name="NatProtocol">The NAT protocol used to create the mapping.</param>
/// <param name="Index">The index that correlates this mapping to a listener binding configuration.</param>
public record PortMapInfo(
    Protocol NetworkProtocol,
    IPEndPoint PublicEndpoint,
    int LocalPort,
    NatProtocol NatProtocol,
    int? Index = null);
