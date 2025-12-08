#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IRouter](index.md 'Pmmux\.Abstractions\.IRouter')

## IRouter\.ReplaceBackendAsync\(Protocol, BackendInfo, BackendSpec, CancellationToken\) Method

Replace an existing backend with a new backend atomically\.

```csharp
System.Threading.Tasks.Task<Pmmux.Abstractions.BackendInfo?> ReplaceBackendAsync(Mono.Nat.Protocol networkProtocol, Pmmux.Abstractions.BackendInfo existingBackend, Pmmux.Abstractions.BackendSpec newBackendSpec, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IRouter.ReplaceBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol that the backends handle\.

<a name='Pmmux.Abstractions.IRouter.ReplaceBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).existingBackend'></a>

`existingBackend` [BackendInfo](../BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')

The backend to replace\.

<a name='Pmmux.Abstractions.IRouter.ReplaceBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).newBackendSpec'></a>

`newBackendSpec` [BackendSpec](../BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The specification for the new backend\.

<a name='Pmmux.Abstractions.IRouter.ReplaceBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[BackendInfo](../BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
Newly created backend information if successful\.

### Remarks
Replaces existing backend with new backend atomically to avoid downtime\.
All new traffic is routed to the new backend while the old backend is drained
of active connections\.