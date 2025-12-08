#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IEventNotifier Interface

Allows components to subscribe to events for system changes\.

```csharp
public interface IEventNotifier : System.IAsyncDisposable
```

Implements [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Events | |
| :--- | :--- |
| [BackendAdded](BackendAdded.md 'Pmmux\.Abstractions\.IEventNotifier\.BackendAdded') | Raised when a backend is added\. |
| [BackendRemoved](BackendRemoved.md 'Pmmux\.Abstractions\.IEventNotifier\.BackendRemoved') | Raised when a backend is removed\. |
| [HealthCheckAdded](HealthCheckAdded.md 'Pmmux\.Abstractions\.IEventNotifier\.HealthCheckAdded') | Raised when a health check is added\. |
| [HealthCheckRemoved](HealthCheckRemoved.md 'Pmmux\.Abstractions\.IEventNotifier\.HealthCheckRemoved') | Raised when a health check is removed\. |
| [PortMapAdded](PortMapAdded.md 'Pmmux\.Abstractions\.IEventNotifier\.PortMapAdded') | Raised when a port map is added\. |
| [PortMapChanged](PortMapChanged.md 'Pmmux\.Abstractions\.IEventNotifier\.PortMapChanged') | Raised when a port map is changed \(renewed\)\. |
| [PortMapRemoved](PortMapRemoved.md 'Pmmux\.Abstractions\.IEventNotifier\.PortMapRemoved') | Raised when a port map is removed\. |
