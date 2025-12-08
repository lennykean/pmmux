#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IBackend Interface

Base interface for all backend implementations that handle traffic in the port multiplexer\.

```csharp
public interface IBackend : System.IAsyncDisposable
```

Derived  
&#8627; [IConnectionlessBackend](../IConnectionlessBackend/index.md 'Pmmux\.Abstractions\.IConnectionlessBackend')  
&#8627; [IConnectionOrientedBackend](../IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend')  
&#8627; [IHealthCheckBackend](../IHealthCheckBackend/index.md 'Pmmux\.Abstractions\.IHealthCheckBackend')

Implements [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
A backend represents a destination that can receive and process client traffic\. Backends are created
by [IBackendProtocol](../IBackendProtocol/index.md 'Pmmux\.Abstractions\.IBackendProtocol') implementations based on [BackendSpec](../BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec') configurations\.
Backends should implement either [IConnectionOrientedBackend](../IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend') for TCP protocols
or [IConnectionlessBackend](../IConnectionlessBackend/index.md 'Pmmux\.Abstractions\.IConnectionlessBackend') for UDP protocols\.

| Properties | |
| :--- | :--- |
| [Backend](Backend.md 'Pmmux\.Abstractions\.IBackend\.Backend') | The backend metadata and configuration information\. |
