#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PortMultiplexer](index.md 'Pmmux\.Core\.PortMultiplexer')

## PortMultiplexer\.AddListener\(Protocol, int, IPAddress\) Method

Add a new listener on the specified port\.

```csharp
public Pmmux.Abstractions.ListenerInfo? AddListener(Mono.Nat.Protocol networkProtocol, int port, System.Net.IPAddress? bindAddress=null);
```
#### Parameters

<a name='Pmmux.Core.PortMultiplexer.AddListener(Mono.Nat.Protocol,int,System.Net.IPAddress).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol that the listener will accept\.

<a name='Pmmux.Core.PortMultiplexer.AddListener(Mono.Nat.Protocol,int,System.Net.IPAddress).port'></a>

`port` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The port number to listen on\.

<a name='Pmmux.Core.PortMultiplexer.AddListener(Mono.Nat.Protocol,int,System.Net.IPAddress).bindAddress'></a>

`bindAddress` [System\.Net\.IPAddress](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipaddress 'System\.Net\.IPAddress')

The IP address to bind the listener to\.

Implements [AddListener\(Protocol, int, IPAddress\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortMultiplexer/AddListener(Protocol,int,IPAddress).md 'Pmmux\.Abstractions\.IPortMultiplexer\.AddListener\(Mono\.Nat\.Protocol,System\.Int32,System\.Net\.IPAddress\)')

#### Returns
[ListenerInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo')  
Created [ListenerInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo') if successful; otherwise, `null`\.