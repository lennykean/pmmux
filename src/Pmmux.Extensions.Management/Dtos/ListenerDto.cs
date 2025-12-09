using System.Collections.Generic;

using Mono.Nat;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Management.Dtos;

/// <summary>
/// A DTO object representing a listener.
/// </summary>
/// <param name="NetworkProtocol">The network protocol.</param>
/// <param name="Address">The bound address.</param>
/// <param name="Port">The bound port.</param>
/// <param name="Properties">Listener properties.</param>
public record ListenerDto(
    Protocol NetworkProtocol,
    string Address,
    int Port,
    IReadOnlyDictionary<string, string> Properties)
{
    /// <summary>
    /// Create a DTO from a <see cref="ListenerInfo"/>.
    /// </summary>
    /// <param name="info">The listener info.</param>
    /// <returns>A DTO object.</returns>
    public static ListenerDto FromListenerInfo(ListenerInfo info) => new(
        info.NetworkProtocol,
        info.LocalEndPoint.Address.ToString(),
        info.LocalEndPoint.Port,
        info.Properties);
}

