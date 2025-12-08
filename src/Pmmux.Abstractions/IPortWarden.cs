using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Manages NAT port mappings (UPnP/PMP).
/// </summary>
/// <remarks>
/// Port warden discovers NAT devices on the network, then creates and maintains port mappings that forward external
/// traffic to the multiplexer's listeners.
/// </remarks>
public interface IPortWarden : IAsyncDisposable
{
    /// <summary>
    /// Discovered NAT device, if any.
    /// </summary>
    NatDeviceInfo? NatDevice { get; }

    /// <summary>
    /// Discover NAT devices and create port mappings from the initial configuration.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous startup operation.</returns>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the current port mappings.
    /// </summary>
    IEnumerable<PortMapInfo> GetPortMaps();

    /// <summary>
    /// Add a new port mapping.
    /// </summary>
    /// <param name="networkProtocol">The network protocol to forward.</param>
    /// <param name="localPort">The local port to map from the public port.</param>
    /// <param name="publicPort">The public port to map to the local port.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Created <see cref="Mapping"/> if successful; otherwise, <c>null</c>.</returns>
    Task<PortMapInfo?> AddPortMapAsync(
        Protocol networkProtocol,
        int? localPort,
        int? publicPort,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove an existing port mapping.
    /// </summary>
    /// <param name="networkProtocol">The network protocol of the mapping to remove.</param>
    /// <param name="localPort">The local port of the mapping to remove.</param>
    /// <param name="publicPort">The public port of the mapping to remove.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><c>true</c> if the mapping was successfully removed; otherwise, <c>false</c>.</returns>
    Task<bool> RemovePortMapAsync(
        Protocol networkProtocol,
        int localPort,
        int publicPort,
        CancellationToken cancellationToken = default);
}
