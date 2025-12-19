using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Main orchestration interface for the port multiplexer.
/// </summary>
public interface IPortMultiplexer : IAsyncDisposable
{
    /// <summary>
    /// Start accepting client connections and messages on bound listeners.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the currently bound listener endpoints.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of bound listener endpoints.</returns>
    Task<IEnumerable<ListenerInfo>> GetListenersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new listener on the specified port.
    /// </summary>
    /// <param name="networkProtocol">The network protocol that the listener will accept.</param>
    /// <param name="port">The port number to listen on.</param>
    /// <param name="bindAddress">The IP address to bind the listener to.</param>
    /// <returns>Created <see cref="ListenerInfo"/> if successful; otherwise, <c>null</c>.</returns>
    ListenerInfo? AddListener(Protocol networkProtocol, int port, IPAddress? bindAddress = null);

    /// <summary>
    /// Remove an existing listener on the specified port.
    /// </summary>
    /// <param name="networkProtocol">The network protocol of the listener to remove.</param>
    /// <param name="port">The port number of the listener to remove.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><c>true</c> if the listener was successfully removed; otherwise, <c>false</c>.</returns>
    Task<bool> RemoveListenerAsync(Protocol networkProtocol, int port, CancellationToken cancellationToken = default);
}
