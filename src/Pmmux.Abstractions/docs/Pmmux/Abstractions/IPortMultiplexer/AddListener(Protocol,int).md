#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IPortMultiplexer](index.md 'Pmmux\.Abstractions\.IPortMultiplexer')

## IPortMultiplexer\.AddListener\(Protocol, int\) Method

Add a new listener on the specified port\.

```csharp
Pmmux.Abstractions.ListenerInfo? AddListener(Mono.Nat.Protocol networkProtocol, int port);
```
#### Parameters

<a name='Pmmux.Abstractions.IPortMultiplexer.AddListener(Mono.Nat.Protocol,int).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol that the listener will accept\.

<a name='Pmmux.Abstractions.IPortMultiplexer.AddListener(Mono.Nat.Protocol,int).port'></a>

`port` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The port number to listen on\.

#### Returns
[ListenerInfo](../ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo')  
Created [ListenerInfo](../ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo') if successful; otherwise, `null`\.