using System;
using System.Net;

using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Information about a discovered NAT device on the network.
/// </summary>
/// <param name="NatProtocol">The protocol used to discover and communicate with the NAT device.</param>
/// <param name="Endpoint">The network endpoint of the NAT device.</param>
/// <param name="PublicAddress">The NAT device's public IP address visible to the internet.</param>
/// <param name="Discovered">The timestamp when the NAT device was discovered.</param>
public sealed record NatDeviceInfo(
    NatProtocol NatProtocol,
    IPEndPoint Endpoint,
    IPAddress PublicAddress,
    DateTime Discovered);
