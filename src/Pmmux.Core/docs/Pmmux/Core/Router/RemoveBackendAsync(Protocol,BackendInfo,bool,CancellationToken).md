#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[Router](index.md 'Pmmux\.Core\.Router')

## Router\.RemoveBackendAsync\(Protocol, BackendInfo, bool, CancellationToken\) Method

Remove a backend dynamically at runtime\.

```csharp
public System.Threading.Tasks.Task<bool> RemoveBackendAsync(Mono.Nat.Protocol networkProtocol, Pmmux.Abstractions.BackendInfo backend, bool forceCloseConnections, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Pmmux.Core.Router.RemoveBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,bool,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol that the backend handles\.

<a name='Pmmux.Core.Router.RemoveBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,bool,System.Threading.CancellationToken).backend'></a>

`backend` [BackendInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')

The backend to remove\.

<a name='Pmmux.Core.Router.RemoveBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,bool,System.Threading.CancellationToken).forceCloseConnections'></a>

`forceCloseConnections` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Determines whether to forcibly close active connections or drain gracefully\.

<a name='Pmmux.Core.Router.RemoveBackendAsync(Mono.Nat.Protocol,Pmmux.Abstractions.BackendInfo,bool,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [RemoveBackendAsync\(Protocol, BackendInfo, bool, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/RemoveBackendAsync(Protocol,BackendInfo,bool,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.RemoveBackendAsync\(Mono\.Nat\.Protocol,Pmmux\.Abstractions\.BackendInfo,System\.Boolean,System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
True if backend was removed\.