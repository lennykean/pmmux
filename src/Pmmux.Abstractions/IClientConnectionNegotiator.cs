using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Negotiates client connections before routing using a chain-of-responsibility pattern.
/// </summary>
/// <remarks>
/// Connection negotiators intercept raw socket connections for transport-level processing such as
/// encryption, handshakes, authentication, or packet filtering. Multiple negotiators can be chained
/// together, each deciding whether to handle the connection or pass it to the next negotiator.
/// </remarks>
public interface IClientConnectionNegotiator
{
    /// <summary>
    /// Result of a connection negotiation attempt.
    /// </summary>
    /// <param name="Success"><c>true</c> if negotiation succeeded; otherwise, <c>false</c>.</param>
    /// <param name="ClientConnection">The negotiated client connection if successful; otherwise, <c>null</c>.</param>
    /// <param name="Reason">The rejection reason if unsuccessful; otherwise, <c>null</c>.</param>
    public record Result(bool Success, IClientConnection? ClientConnection, string? Reason)
    {
        /// <summary>
        /// Create a successful negotiation result with the specified client connection.
        /// </summary>
        /// <param name="clientConnection">The established and negotiated client connection.</param>
        /// <returns>A <see cref="Result"/> indicating successful negotiation.</returns>
        public static Result Accept(IClientConnection clientConnection) => new(true, clientConnection, null);

        /// <summary>
        /// Create a failed negotiation result with the specified reason.
        /// </summary>
        /// <param name="reason">The reason why the connection was rejected or could not be negotiated.</param>
        /// <returns>A <see cref="Result"/> indicating the negotiation failed.</returns>
        public static Result Reject(string reason) => new(false, null, reason);
    };

    /// <summary>
    /// The name of the connection negotiator.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Negotiate a client connection, performing any required transport-level processing.
    /// </summary>
    /// <param name="context">The context containing client information, properties, socket, and stream.</param>
    /// <param name="next">
    /// Function to invoke the next negotiator in the chain. Returns the result from the next negotiator.
    /// </param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Result"/> with the negotiated <see cref="IClientConnection"/> if accepted,
    /// or a rejection reason if rejected.
    /// </returns>
    /// <remarks>
    /// Negotiators can accept the connection or pass it to the next negotiator in the chain.
    /// Negotiators may modify <see cref="ClientConnectionContext.Properties"/> or
    /// wrap and replace <see cref="ClientConnectionContext.ClientConnectionStream"/> before returning.
    /// </remarks>
    Task<Result> NegotiateAsync(
        ClientConnectionContext context,
        Func<Task<Result>> next,
        CancellationToken cancellationToken = default);
}
