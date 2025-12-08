#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IEventSender Interface

Raises events for system changes\.

```csharp
public interface IEventSender
```

| Methods | |
| :--- | :--- |
| [RaiseBackendAdded\(object, BackendSpec\)](RaiseBackendAdded(object,BackendSpec).md 'Pmmux\.Abstractions\.IEventSender\.RaiseBackendAdded\(object, Pmmux\.Abstractions\.BackendSpec\)') | Raise an event indicating a backend was added\. |
| [RaiseBackendRemoved\(object, BackendSpec\)](RaiseBackendRemoved(object,BackendSpec).md 'Pmmux\.Abstractions\.IEventSender\.RaiseBackendRemoved\(object, Pmmux\.Abstractions\.BackendSpec\)') | Raise an event indicating a backend was removed\. |
| [RaiseHealthCheckAdded\(object, HealthCheckSpec\)](RaiseHealthCheckAdded(object,HealthCheckSpec).md 'Pmmux\.Abstractions\.IEventSender\.RaiseHealthCheckAdded\(object, Pmmux\.Abstractions\.HealthCheckSpec\)') | Raise an event indicating a health check was added\. |
| [RaiseHealthCheckRemoved\(object, HealthCheckSpec\)](RaiseHealthCheckRemoved(object,HealthCheckSpec).md 'Pmmux\.Abstractions\.IEventSender\.RaiseHealthCheckRemoved\(object, Pmmux\.Abstractions\.HealthCheckSpec\)') | Raise an event indicating a health check was removed\. |
| [RaisePortMapAdded\(object, Mapping\)](RaisePortMapAdded(object,Mapping).md 'Pmmux\.Abstractions\.IEventSender\.RaisePortMapAdded\(object, Mono\.Nat\.Mapping\)') | Raise an event indicating a port map was added\. |
| [RaisePortMapChanged\(object, Mapping\)](RaisePortMapChanged(object,Mapping).md 'Pmmux\.Abstractions\.IEventSender\.RaisePortMapChanged\(object, Mono\.Nat\.Mapping\)') | Raise an event indicating a port map was changed \(renewed\)\. |
| [RaisePortMapRemoved\(object, Mapping\)](RaisePortMapRemoved(object,Mapping).md 'Pmmux\.Abstractions\.IEventSender\.RaisePortMapRemoved\(object, Mono\.Nat\.Mapping\)') | Raise an event indicating a port map was removed\. |
