#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IRouter](index.md 'Pmmux\.Abstractions\.IRouter')

## IRouter\.AddBackendAsync\(Protocol, BackendSpec, CancellationToken\) Method

Add a backend dynamically at runtime\.

```csharp
System.Threading.Tasks.Task<Pmmux.Abstractions.BackendInfo> AddBackendAsync(Mono.Nat.Protocol networkProtocol, Pmmux.Abstractions.BackendSpec backendSpec, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IRouter.AddBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol that the backend will handle\.

<a name='Pmmux.Abstractions.IRouter.AddBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).backendSpec'></a>

`backendSpec` [BackendSpec](../BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The backend specification\.

<a name='Pmmux.Abstractions.IRouter.AddBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[BackendInfo](../BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
Newly created backend information\.