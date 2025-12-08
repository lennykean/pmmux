using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Backend for connectionless protocols like UDP.
/// </summary>
/// <remarks>
/// Connectionless backends handle discrete messages without persistent connections.
/// Each message is processed independently, and responses are sent via <see cref="IClientWriterFactory"/>.
/// For persistent connection protocols like TCP, <see cref="IConnectionOrientedBackend"/> should be used.
/// </remarks>
public interface IConnectionlessBackend : IBackend
{
    /// <summary>
    /// Determine whether this backend can handle the message.
    /// </summary>
    /// <param name="client">The client that sent the message.</param>
    /// <param name="messageProperties">The metadata about the message (protocol-specific).</param>
    /// <param name="message">The message content as a read-only buffer for inspection.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><c>true</c> if this backend can handle the message; otherwise, <c>false</c>.</returns>
    Task<bool> CanHandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageProperties,
        ReadOnlyMemory<byte> message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Process the message from the client.
    /// </summary>
    /// <param name="client">The client that sent the message.</param>
    /// <param name="messageProperties">The metadata about the message.</param>
    /// <param name="message">The message content.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task representing the message handling operation.</returns>
    /// <remarks>
    /// Use the <see cref="IClientWriterFactory"/> provided during initialization
    /// to send responses back to the client.
    /// </remarks>
    Task HandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageProperties,
        byte[] message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Initialize the backend with the client writer factory.
    /// </summary>
    /// <param name="clientWriterFactory">The factory to create writers for sending responses to clients.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task representing the initialization operation.</returns>
    /// <remarks>
    /// The <paramref name="clientWriterFactory"/> should be stored for use in <see cref="HandleMessageAsync"/>
    /// to send responses back to clients.
    /// </remarks>
    Task InitializeAsync(
        IClientWriterFactory clientWriterFactory,
        CancellationToken cancellationToken = default);
}
