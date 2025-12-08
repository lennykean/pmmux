using System;

using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Allows components to subscribe to events for system changes.
/// </summary>
public interface IEventNotifier : IAsyncDisposable
{
    /// <summary>
    /// Raised when a backend is added.
    /// </summary>
    event EventHandler<BackendSpec> BackendAdded;

    /// <summary>
    /// Raised when a backend is removed.
    /// </summary>
    event EventHandler<BackendSpec> BackendRemoved;

    /// <summary>
    /// Raised when a port map is added.
    /// </summary>
    event EventHandler<Mapping> PortMapAdded;

    /// <summary>
    /// Raised when a port map is removed.
    /// </summary>
    event EventHandler<Mapping> PortMapRemoved;

    /// <summary>
    /// Raised when a port map is changed (renewed).
    /// </summary>
    event EventHandler<Mapping> PortMapChanged;

    /// <summary>
    /// Raised when a health check is added.
    /// </summary>
    event EventHandler<HealthCheckSpec> HealthCheckAdded;

    /// <summary>
    /// Raised when a health check is removed.
    /// </summary>
    event EventHandler<HealthCheckSpec> HealthCheckRemoved;
}
