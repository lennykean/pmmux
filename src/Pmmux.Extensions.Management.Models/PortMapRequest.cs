using Mono.Nat;

namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// Request to create a port mapping.
/// </summary>
public class PortMapRequest
{
    /// <summary>The network protocol.</summary>
    public Protocol NetworkProtocol { get; set; }

    /// <summary>The local port number.</summary>
    public int? LocalPort { get; set; }

    /// <summary>The public port number.</summary>
    public int? PublicPort { get; set; }

    /// <summary>Whether to also create a listener.</summary>
    public bool CreateListener { get; set; }
}
