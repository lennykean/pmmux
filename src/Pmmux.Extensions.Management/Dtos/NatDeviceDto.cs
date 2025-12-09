using Pmmux.Abstractions;

namespace Pmmux.Extensions.Management.Dtos;

/// <summary>
/// A DTO for NAT device information.
/// </summary>
public record NatDeviceDto(
    string NatProtocol,
    string DeviceAddress,
    int DevicePort,
    string PublicAddress,
    string Discovered)
{
    /// <summary>
    /// Create a DTO from a <see cref="NatDeviceInfo"/>.
    /// </summary>
    public static NatDeviceDto FromNatDeviceInfo(NatDeviceInfo info) => new(
        info.NatProtocol.ToString(),
        info.Endpoint.Address.ToString(),
        info.Endpoint.Port,
        info.PublicAddress.ToString(),
        info.Discovered.ToString("O"));
}

