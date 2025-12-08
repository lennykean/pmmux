#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[ListenerInfo](index.md 'Pmmux\.Abstractions\.ListenerInfo')

## ListenerInfo\(Protocol, IPEndPoint, IDictionary\<string,string\>\) Constructor

```csharp
public ListenerInfo(Mono.Nat.Protocol networkProtocol, System.Net.IPEndPoint localEndPoint, System.Collections.Generic.IDictionary<string,string> properties);
```
#### Parameters

<a name='Pmmux.Abstractions.ListenerInfo.ListenerInfo(Mono.Nat.Protocol,System.Net.IPEndPoint,System.Collections.Generic.IDictionary_string,string_).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol that the listener accepts\.

<a name='Pmmux.Abstractions.ListenerInfo.ListenerInfo(Mono.Nat.Protocol,System.Net.IPEndPoint,System.Collections.Generic.IDictionary_string,string_).localEndPoint'></a>

`localEndPoint` [System\.Net\.IPEndPoint](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipendpoint 'System\.Net\.IPEndPoint')

The local endpoint where the listener is bound\.

<a name='Pmmux.Abstractions.ListenerInfo.ListenerInfo(Mono.Nat.Protocol,System.Net.IPEndPoint,System.Collections.Generic.IDictionary_string,string_).properties'></a>

`properties` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

The listener\-specific properties and metadata\.