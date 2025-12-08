using System;
using System.Threading.Tasks;

using Mono.Nat;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal sealed class EventBroker : IEventNotifier, IEventSender
{
    public event EventHandler<BackendSpec>? BackendAdded;
    public event EventHandler<BackendSpec>? BackendRemoved;
    public event EventHandler<Mapping>? PortMapAdded;
    public event EventHandler<Mapping>? PortMapRemoved;
    public event EventHandler<Mapping>? PortMapChanged;
    public event EventHandler<HealthCheckSpec>? HealthCheckAdded;
    public event EventHandler<HealthCheckSpec>? HealthCheckRemoved;

    public void RaiseBackendAdded(object sender, BackendSpec backend)
    {
        BackendAdded?.Invoke(sender, backend);
    }

    public void RaiseBackendRemoved(object sender, BackendSpec backend)
    {
        BackendRemoved?.Invoke(sender, backend);
    }

    public void RaisePortMapAdded(object sender, Mapping mapping)
    {
        PortMapAdded?.Invoke(sender, mapping);
    }

    public void RaisePortMapRemoved(object sender, Mapping mapping)
    {
        PortMapRemoved?.Invoke(sender, mapping);
    }

    public void RaisePortMapChanged(object sender, Mapping mapping)
    {
        PortMapChanged?.Invoke(sender, mapping);
    }

    public void RaiseHealthCheckAdded(object sender, HealthCheckSpec healthCheck)
    {
        HealthCheckAdded?.Invoke(sender, healthCheck);
    }

    public void RaiseHealthCheckRemoved(object sender, HealthCheckSpec healthCheck)
    {
        HealthCheckRemoved?.Invoke(sender, healthCheck);
    }

    public ValueTask DisposeAsync()
    {
        BackendAdded = null;
        BackendRemoved = null;
        PortMapAdded = null;
        PortMapRemoved = null;
        PortMapChanged = null;
        HealthCheckAdded = null;
        HealthCheckRemoved = null;

        return default;
    }
}
