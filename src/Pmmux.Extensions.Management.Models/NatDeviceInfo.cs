namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// NAT device information DTO.
/// </summary>
public record NatDeviceInfoDto
{
    /// <summary>The NAT protocol.</summary>
    public required string NatProtocol { get; set; }

    /// <summary>The device address.</summary>
    public required string DeviceAddress { get; set; }

    /// <summary>The device port.</summary>
    public int DevicePort { get; set; }

    /// <summary>The public address.</summary>
    public required string PublicAddress { get; set; }

    /// <summary>The discovery timestamp.</summary>
    public required string Discovered { get; set; }
}
