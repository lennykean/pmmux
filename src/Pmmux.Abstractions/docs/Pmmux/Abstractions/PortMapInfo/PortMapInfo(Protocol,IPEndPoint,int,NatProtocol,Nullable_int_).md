#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[PortMapInfo](index.md 'Pmmux\.Abstractions\.PortMapInfo')

## PortMapInfo\(Protocol, IPEndPoint, int, NatProtocol, Nullable\<int\>\) Constructor

Represents information about a NAT port mapping between a public endpoint and a local port\.

```csharp
public PortMapInfo(Mono.Nat.Protocol NetworkProtocol, System.Net.IPEndPoint PublicEndpoint, int LocalPort, Mono.Nat.NatProtocol NatProtocol, System.Nullable<int> Index=null);
```
#### Parameters

<a name='Pmmux.Abstractions.PortMapInfo.PortMapInfo(Mono.Nat.Protocol,System.Net.IPEndPoint,int,Mono.Nat.NatProtocol,System.Nullable_int_).NetworkProtocol'></a>

`NetworkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol being forwarded\.

<a name='Pmmux.Abstractions.PortMapInfo.PortMapInfo(Mono.Nat.Protocol,System.Net.IPEndPoint,int,Mono.Nat.NatProtocol,System.Nullable_int_).PublicEndpoint'></a>

`PublicEndpoint` [System\.Net\.IPEndPoint](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipendpoint 'System\.Net\.IPEndPoint')

The public endpoint accessible from outside the LAN\.

<a name='Pmmux.Abstractions.PortMapInfo.PortMapInfo(Mono.Nat.Protocol,System.Net.IPEndPoint,int,Mono.Nat.NatProtocol,System.Nullable_int_).LocalPort'></a>

`LocalPort` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The local port that receives traffic forwarded from the public port\.

<a name='Pmmux.Abstractions.PortMapInfo.PortMapInfo(Mono.Nat.Protocol,System.Net.IPEndPoint,int,Mono.Nat.NatProtocol,System.Nullable_int_).NatProtocol'></a>

`NatProtocol` [Mono\.Nat\.NatProtocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.natprotocol 'Mono\.Nat\.NatProtocol')

The NAT protocol used to create the mapping\.

<a name='Pmmux.Abstractions.PortMapInfo.PortMapInfo(Mono.Nat.Protocol,System.Net.IPEndPoint,int,Mono.Nat.NatProtocol,System.Nullable_int_).Index'></a>

`Index` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The index that correlates this mapping to a listener binding configuration\.