namespace Pmmux.Abstractions;

/// <summary>
/// Established client connection ready for routing and data transfer.
/// </summary>
/// <remarks>
/// Client connections are created through <see cref="IClientConnectionNegotiator"/> implementations
/// that perform protocol negotiation (e.g., TLS handshake).
/// </remarks>
public interface IClientConnection : IConnection
{
    /// <summary>
    /// Client associated with the connection.
    /// </summary>
    ClientInfo Client { get; }

    /// <summary>
    /// Create a preview of the connection for peeking at incoming data without consuming it.
    /// </summary>
    /// <returns><see cref="IClientConnectionPreview"/> providing peek operations for routing decisions.</returns>
    /// <remarks>
    /// Previews are used by the router and backends to inspect connection properties and peek at the data stream
    /// before committing to handling the connection. Multiple previews can be created sequentially from the same
    /// connection, allowing different backends to evaluate the connection during routing.
    /// </remarks>
    IClientConnectionPreview Preview();
}
