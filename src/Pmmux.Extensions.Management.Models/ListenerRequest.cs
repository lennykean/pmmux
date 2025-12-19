using Mono.Nat;

namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// Request to create a listener.
/// </summary>
public class ListenerRequest
{
    /// <summary>The network protocol.</summary>
    public Protocol NetworkProtocol { get; set; }

    /// <summary>The port number.</summary>
    public int Port { get; set; }

    /// <summary>The IP address to bind to.</summary>
    public string? BindAddress { get; set; }
}
