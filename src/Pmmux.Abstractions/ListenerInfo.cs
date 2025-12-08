using System.Collections.Generic;
using System.Net;

using Mono.Nat;

using Pmmux.Abstractions.Utilities;

namespace Pmmux.Abstractions;

/// <summary>
/// Information about a network listener and its accessibility.
/// </summary>
/// <remarks>
/// Describes a bound socket listener and optionally its external accessibility through a NAT port mapping.
/// </remarks>
public record ListenerInfo
{
    /// <param name="networkProtocol">The network protocol that the listener accepts.</param>
    /// <param name="localEndPoint">The local endpoint where the listener is bound.</param>
    /// <param name="properties">The listener-specific properties and metadata.</param>
    public ListenerInfo(Protocol networkProtocol, IPEndPoint localEndPoint, IDictionary<string, string> properties)
    {
        NetworkProtocol = networkProtocol;
        LocalEndPoint = localEndPoint;
        Properties = new EquatableDictionary<string, string>(properties);
    }

    /// <summary>
    /// The network protocol that the listener accepts.
    /// </summary>
    public Protocol NetworkProtocol { get; init; }

    /// <summary>
    /// The local endpoint where the listener is bound.
    /// </summary>
    public IPEndPoint LocalEndPoint { get; init; }

    /// <summary>
    /// The listener-specific properties and metadata.
    /// </summary>
    public IReadOnlyDictionary<string, string> Properties { get; }
}
