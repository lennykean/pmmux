using System.Collections.Generic;

using Mono.Nat;

namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// Listener information DTO.
/// </summary>
public record ListenerInfoDto
{
    /// <summary>The network protocol.</summary>
    public Protocol NetworkProtocol { get; set; }

    /// <summary>The bound address.</summary>
    public required string Address { get; set; }

    /// <summary>The bound port.</summary>
    public int Port { get; set; }

    /// <summary>Listener properties.</summary>
    public Dictionary<string, string> Properties { get; set; } = [];
}
