#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IPortMultiplexer](index.md 'Pmmux\.Abstractions\.IPortMultiplexer')

## IPortMultiplexer\.AddListener\(Protocol, int, IPAddress\) Method

Add a new listener on the specified port\.

```csharp
Pmmux.Abstractions.ListenerInfo? AddListener(Mono.Nat.Protocol networkProtocol, int port, System.Net.IPAddress? bindAddress=null);
```
#### Parameters

<a name='Pmmux.Abstractions.IPortMultiplexer.AddListener(Mono.Nat.Protocol,int,System.Net.IPAddress).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol that the listener will accept\.

<a name='Pmmux.Abstractions.IPortMultiplexer.AddListener(Mono.Nat.Protocol,int,System.Net.IPAddress).port'></a>

`port` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The port number to listen on\.

<a name='Pmmux.Abstractions.IPortMultiplexer.AddListener(Mono.Nat.Protocol,int,System.Net.IPAddress).bindAddress'></a>

`bindAddress` [System\.Net\.IPAddress](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipaddress 'System\.Net\.IPAddress')

The IP address to bind the listener to\.

#### Returns
[ListenerInfo](../ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo')  
Created [ListenerInfo](../ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo') if successful; otherwise, `null`\.