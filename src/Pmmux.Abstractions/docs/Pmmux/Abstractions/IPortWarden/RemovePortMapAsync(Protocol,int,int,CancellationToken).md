#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IPortWarden](index.md 'Pmmux\.Abstractions\.IPortWarden')

## IPortWarden\.RemovePortMapAsync\(Protocol, int, int, CancellationToken\) Method

Remove an existing port mapping\.

```csharp
System.Threading.Tasks.Task<bool> RemovePortMapAsync(Mono.Nat.Protocol networkProtocol, int localPort, int publicPort, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IPortWarden.RemovePortMapAsync(Mono.Nat.Protocol,int,int,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol of the mapping to remove\.

<a name='Pmmux.Abstractions.IPortWarden.RemovePortMapAsync(Mono.Nat.Protocol,int,int,System.Threading.CancellationToken).localPort'></a>

`localPort` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The local port of the mapping to remove\.

<a name='Pmmux.Abstractions.IPortWarden.RemovePortMapAsync(Mono.Nat.Protocol,int,int,System.Threading.CancellationToken).publicPort'></a>

`publicPort` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The public port of the mapping to remove\.

<a name='Pmmux.Abstractions.IPortWarden.RemovePortMapAsync(Mono.Nat.Protocol,int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
`true` if the mapping was successfully removed; otherwise, `false`\.