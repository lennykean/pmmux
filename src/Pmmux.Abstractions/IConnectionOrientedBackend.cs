using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Backend for connection-oriented protocols like TCP.
/// </summary>
/// <remarks>
/// Connection-oriented backends handle persistent bidirectional connections where data flows
/// continuously between client and backend. This interface is used for TCP-based protocols.
/// For message-based protocols like UDP, <see cref="IConnectionlessBackend"/> should be used.
/// </remarks>
public interface IConnectionOrientedBackend : IBackend
{
    /// <summary>
    /// Determine whether this backend can handle the client connection.
    /// </summary>
    /// <param name="client">The client connection preview for inspecting connection metadata and peeking at data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><c>true</c> if this backend can handle the connection; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Use <see cref="IClientConnectionPreview.Properties"/> to check connection metadata or
    /// <see cref="IClientConnectionPreview.Ingress"/> to peek at initial bytes for protocol detection.
    /// </remarks>
    Task<bool> CanHandleConnectionAsync(
        IClientConnectionPreview client,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create and establish the backend connection for the client.
    /// </summary>
    /// <param name="client">The client connection with full read/write access.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// An <see cref="IConnection"/> representing the established backend connection.
    /// The multiplexer will relay data bidirectionally between client and this connection.
    /// </returns>
    Task<IConnection> CreateBackendConnectionAsync(
        IClientConnection client,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Initialize the backend at startup.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task representing the initialization operation.</returns>
    /// <remarks>
    /// Perform any one-time setup such as establishing persistent connections to upstream servers,
    /// warming caches, or validating configuration.
    /// </remarks>
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
