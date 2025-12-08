#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IPortMultiplexer](index.md 'Pmmux\.Abstractions\.IPortMultiplexer')

## IPortMultiplexer\.RemoveListenerAsync\(Protocol, int, CancellationToken\) Method

Remove an existing listener on the specified port\.

```csharp
System.Threading.Tasks.Task<bool> RemoveListenerAsync(Mono.Nat.Protocol networkProtocol, int port, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IPortMultiplexer.RemoveListenerAsync(Mono.Nat.Protocol,int,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol of the listener to remove\.

<a name='Pmmux.Abstractions.IPortMultiplexer.RemoveListenerAsync(Mono.Nat.Protocol,int,System.Threading.CancellationToken).port'></a>

`port` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The port number of the listener to remove\.

<a name='Pmmux.Abstractions.IPortMultiplexer.RemoveListenerAsync(Mono.Nat.Protocol,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
`true` if the listener was successfully removed; otherwise, `false`\.