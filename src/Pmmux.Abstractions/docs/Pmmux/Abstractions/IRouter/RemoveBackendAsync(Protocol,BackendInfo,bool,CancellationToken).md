#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IRouter](index.md 'Pmmux\.Abstractions\.IRouter')

## IRouter\.RemoveBackendAsync\(Protocol, BackendInfo, bool, CancellationToken\) Method

Remove a backend dynamically at runtime\.

```csharp
System.Threading.Tasks.Task<bool> RemoveBackendAsync(Mono.Nat.Protocol networkProtocol, Pmmux.Abstractions.BackendInfo backend, bool forceCloseConnections=true, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IRouter.RemoveBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,bool,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol that the backend handles\.

<a name='Pmmux.Abstractions.IRouter.RemoveBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,bool,System.Threading.CancellationToken).backend'></a>

`backend` [BackendInfo](../BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')

The backend to remove\.

<a name='Pmmux.Abstractions.IRouter.RemoveBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,bool,System.Threading.CancellationToken).forceCloseConnections'></a>

`forceCloseConnections` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Determines whether to forcibly close active connections or drain gracefully\.

<a name='Pmmux.Abstractions.IRouter.RemoveBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,bool,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
True if backend was removed\.