using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;

namespace Pmmux.Abstractions;

/// <summary>
/// Context for client connection negotiators, containing connection details and properties.
/// </summary>
public record ClientConnectionContext
{
    /// <summary>
    /// Metadata about the client attempting to connect.
    /// </summary>
    public required ClientInfo Client { get; init; }

    /// <summary>
    /// Properties to be populated by connection negotiators.
    /// These will be made available to backends for routing decisions.
    /// </summary>
    public required ConcurrentDictionary<string, string> Properties { get; init; }

    /// <summary>
    /// The raw socket connection to negotiate.
    /// </summary>
    public required Socket ClientConnection { get; init; }

    /// <summary>
    /// Data stream for the client connection.
    /// Negotiators can wrap and replace the stream to apply transformations.
    /// </summary>
    public required Stream ClientConnectionStream { get; set; }
}
