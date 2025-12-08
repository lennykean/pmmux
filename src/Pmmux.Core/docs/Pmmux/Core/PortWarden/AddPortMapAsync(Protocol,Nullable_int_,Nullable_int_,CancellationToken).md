#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PortWarden](index.md 'Pmmux\.Core\.PortWarden')

## PortWarden\.AddPortMapAsync\(Protocol, Nullable\<int\>, Nullable\<int\>, CancellationToken\) Method

Add a new port mapping\.

```csharp
public System.Threading.Tasks.Task<Pmmux.Abstractions.PortMapInfo?> AddPortMapAsync(Mono.Nat.Protocol networkProtocol, System.Nullable<int> localPort, System.Nullable<int> publicPort, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.PortWarden.AddPortMapAsync(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol to forward\.

<a name='Pmmux.Core.PortWarden.AddPortMapAsync(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).localPort'></a>

`localPort` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The local port to map from the public port\.

<a name='Pmmux.Core.PortWarden.AddPortMapAsync(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).publicPort'></a>

`publicPort` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The public port to map to the local port\.

<a name='Pmmux.Core.PortWarden.AddPortMapAsync(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [AddPortMapAsync\(Protocol, Nullable&lt;int&gt;, Nullable&lt;int&gt;, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortWarden/AddPortMapAsync(Protocol,Nullable_int_,Nullable_int_,CancellationToken).md 'Pmmux\.Abstractions\.IPortWarden\.AddPortMapAsync\(Mono\.Nat\.Protocol,System\.Nullable\{System\.Int32\},System\.Nullable\{System\.Int32\},System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[PortMapInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/PortMapInfo/index.md 'Pmmux\.Abstractions\.PortMapInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
Created [Mono\.Nat\.Mapping](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.mapping 'Mono\.Nat\.Mapping') if successful; otherwise, `null`\.