using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Routes client traffic to backends and manages backend lifecycle.
/// </summary>
public interface IRouter : IAsyncDisposable
{
    /// <summary>
    /// Result of a routing operation.
    /// </summary>
    /// <param name="Success"><c>true</c> if routing succeeded, otherwise <c>false</c>.</param>
    /// <param name="Backend">The selected backend if successful.</param>
    /// <param name="Reason">The failure reason if unsuccessful.</param>
    public record Result(bool Success, BackendInfo? Backend, string? Reason)
    {
        /// <summary>
        /// Create a successful routing result.
        /// </summary>
        /// <param name="backend">The selected backend.</param>
        /// <returns>Success result.</returns>
        public static Result Succeeded(BackendInfo backend) => new(true, backend, null);

        /// <summary>
        /// Create a failed routing result.
        /// </summary>
        /// <param name="reason">The failure reason.</param>
        /// <returns>Failure result.</returns>
        public static Result Failed(string reason) => new(false, null, reason);
    };

    /// <summary>
    /// Initialize the router with listener information at startup.
    /// </summary>
    /// <param name="clientWriterFactory">The factory to create client writers.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task representing the initialization operation.</returns>
    Task InitializeAsync(IClientWriterFactory clientWriterFactory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Route a client connection to a backend.
    /// </summary>
    /// <param name="connection">The client socket connection.</param>
    /// <param name="client">The client information.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Routing result with selected backend or failure reason.</returns>
    Task<Result> RouteConnectionAsync(
        Socket connection,
        ClientInfo client,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Route a client message to a backend.
    /// </summary>
    /// <param name="messageBuffer">The message data.</param>
    /// <param name="client">The client information.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Routing result with selected backend or failure reason.</returns>
    Task<Result> RouteMessageAsync(
        byte[] messageBuffer,
        ClientInfo client,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all backends for the network protocol with their health status.
    /// </summary>
    /// <param name="networkProtocol">The network protocol to filter backends by.</param>
    /// <returns>Backend status information.</returns>
    IEnumerable<BackendStatusInfo> GetBackends(Protocol networkProtocol);

    /// <summary>
    /// Add a backend dynamically at runtime.
    /// </summary>
    /// <param name="networkProtocol">The network protocol that the backend will handle.</param>
    /// <param name="backendSpec">The backend specification.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Newly created backend information.</returns>
    Task<BackendInfo> AddBackendAsync(
        Protocol networkProtocol,
        BackendSpec backendSpec,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a backend dynamically at runtime.
    /// </summary>
    /// <param name="networkProtocol">The network protocol that the backend handles.</param>
    /// <param name="backend">The backend to remove.</param>
    /// <param name="forceCloseConnections">
    /// Determines whether to forcibly close active connections or drain gracefully.
    /// </param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>True if backend was removed.</returns>
    Task<bool> RemoveBackendAsync(
        Protocol networkProtocol,
        BackendInfo backend,
        bool forceCloseConnections = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Replace an existing backend with a new backend atomically.
    /// </summary>
    /// <param name="networkProtocol">The network protocol that the backends handle.</param>
    /// <param name="existingBackend">The backend to replace.</param>
    /// <param name="newBackendSpec">The specification for the new backend.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Newly created backend information if successful.</returns>
    /// <remarks>
    /// Replaces existing backend with new backend atomically to avoid downtime.
    /// All new traffic is routed to the new backend while the old backend is drained
    /// of active connections.
    /// </remarks>
    Task<BackendInfo?> ReplaceBackendAsync(
        Protocol networkProtocol,
        BackendInfo existingBackend,
        BackendSpec newBackendSpec,
        CancellationToken cancellationToken = default);
}

