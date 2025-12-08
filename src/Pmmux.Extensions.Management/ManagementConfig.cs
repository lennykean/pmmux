using System.Net;

namespace Pmmux.Extensions.Management;

internal record ManagementConfig
{
    public bool ManagementEnable { get; init; }
    public int ManagementPort { get; init; } = 8900;
    public IPAddress ManagementBindAddress { get; init; } = IPAddress.Loopback;
}
