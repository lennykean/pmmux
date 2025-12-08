using Mono.Nat;

namespace Pmmux.Abstractions;

/// <summary>
/// Raises events for system changes.
/// </summary>
public interface IEventSender
{
    /// <summary>
    /// Raise an event indicating a backend was added.
    /// </summary>
    /// <param name="sender">The component raising the event.</param>
    /// <param name="backend">The backend spec that was added.</param>
    void RaiseBackendAdded(object sender, BackendSpec backend);

    /// <summary>
    /// Raise an event indicating a backend was removed.
    /// </summary>
    /// <param name="sender">The component raising the event.</param>
    /// <param name="backend">The backend spec that was removed.</param>
    void RaiseBackendRemoved(object sender, BackendSpec backend);

    /// <summary>
    /// Raise an event indicating a port map was added.
    /// </summary>
    /// <param name="sender">The component raising the event.</param>
    /// <param name="mapping">The port mapping that was added.</param>
    void RaisePortMapAdded(object sender, Mapping mapping);

    /// <summary>
    /// Raise an event indicating a port map was removed.
    /// </summary>
    /// <param name="sender">The component raising the event.</param>
    /// <param name="mapping">The port mapping that was removed.</param>
    void RaisePortMapRemoved(object sender, Mapping mapping);

    /// <summary>
    /// Raise an event indicating a port map was changed (renewed).
    /// </summary>
    /// <param name="sender">The component raising the event.</param>
    /// <param name="mapping">The updated port mapping.</param>
    void RaisePortMapChanged(object sender, Mapping mapping);

    /// <summary>
    /// Raise an event indicating a health check was added.
    /// </summary>
    /// <param name="sender">The component raising the event.</param>
    /// <param name="healthCheck">The health check spec that was added.</param>
    void RaiseHealthCheckAdded(object sender, HealthCheckSpec healthCheck);

    /// <summary>
    /// Raise an event indicating a health check was removed.
    /// </summary>
    /// <param name="sender">The component raising the event.</param>
    /// <param name="healthCheck">The health check spec that was removed.</param>
    void RaiseHealthCheckRemoved(object sender, HealthCheckSpec healthCheck);
}
