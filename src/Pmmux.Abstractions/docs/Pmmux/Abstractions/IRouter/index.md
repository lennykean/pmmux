#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IRouter Interface

Routes client traffic to backends and manages backend lifecycle\.

```csharp
public interface IRouter : System.IAsyncDisposable
```

Implements [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Methods | |
| :--- | :--- |
| [AddBackendAsync\(Protocol, BackendSpec, CancellationToken\)](AddBackendAsync(Protocol,BackendSpec,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.AddBackendAsync\(Mono\.Nat\.Protocol, Pmmux\.Abstractions\.BackendSpec, System\.Threading\.CancellationToken\)') | Add a backend dynamically at runtime\. |
| [GetBackends\(Protocol\)](GetBackends(Protocol).md 'Pmmux\.Abstractions\.IRouter\.GetBackends\(Mono\.Nat\.Protocol\)') | Get all backends for the network protocol with their health status\. |
| [InitializeAsync\(IClientWriterFactory, CancellationToken\)](InitializeAsync(IClientWriterFactory,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory, System\.Threading\.CancellationToken\)') | Initialize the router with listener information at startup\. |
| [RemoveBackendAsync\(Protocol, BackendInfo, bool, CancellationToken\)](RemoveBackendAsync(Protocol,BackendInfo,bool,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.RemoveBackendAsync\(Mono\.Nat\.Protocol, Pmmux\.Abstractions\.BackendInfo, bool, System\.Threading\.CancellationToken\)') | Remove a backend dynamically at runtime\. |
| [ReplaceBackendAsync\(Protocol, BackendInfo, BackendSpec, CancellationToken\)](ReplaceBackendAsync(Protocol,BackendInfo,BackendSpec,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.ReplaceBackendAsync\(Mono\.Nat\.Protocol, Pmmux\.Abstractions\.BackendInfo, Pmmux\.Abstractions\.BackendSpec, System\.Threading\.CancellationToken\)') | Replace an existing backend with a new backend atomically\. |
| [RouteConnectionAsync\(Socket, ClientInfo, CancellationToken\)](RouteConnectionAsync(Socket,ClientInfo,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.RouteConnectionAsync\(System\.Net\.Sockets\.Socket, Pmmux\.Abstractions\.ClientInfo, System\.Threading\.CancellationToken\)') | Route a client connection to a backend\. |
| [RouteMessageAsync\(byte\[\], ClientInfo, CancellationToken\)](RouteMessageAsync(byte[],ClientInfo,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.RouteMessageAsync\(byte\[\], Pmmux\.Abstractions\.ClientInfo, System\.Threading\.CancellationToken\)') | Route a client message to a backend\. |
