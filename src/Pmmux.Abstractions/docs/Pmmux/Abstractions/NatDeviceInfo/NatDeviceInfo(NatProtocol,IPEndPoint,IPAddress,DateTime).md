#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[NatDeviceInfo](index.md 'Pmmux\.Abstractions\.NatDeviceInfo')

## NatDeviceInfo\(NatProtocol, IPEndPoint, IPAddress, DateTime\) Constructor

Information about a discovered NAT device on the network\.

```csharp
public NatDeviceInfo(Mono.Nat.NatProtocol NatProtocol, System.Net.IPEndPoint Endpoint, System.Net.IPAddress PublicAddress, System.DateTime Discovered);
```
#### Parameters

<a name='Pmmux.Abstractions.NatDeviceInfo.NatDeviceInfo(Mono.Nat.NatProtocol,System.Net.IPEndPoint,System.Net.IPAddress,System.DateTime).NatProtocol'></a>

`NatProtocol` [Mono\.Nat\.NatProtocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.natprotocol 'Mono\.Nat\.NatProtocol')

The protocol used to discover and communicate with the NAT device\.

<a name='Pmmux.Abstractions.NatDeviceInfo.NatDeviceInfo(Mono.Nat.NatProtocol,System.Net.IPEndPoint,System.Net.IPAddress,System.DateTime).Endpoint'></a>

`Endpoint` [System\.Net\.IPEndPoint](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipendpoint 'System\.Net\.IPEndPoint')

The network endpoint of the NAT device\.

<a name='Pmmux.Abstractions.NatDeviceInfo.NatDeviceInfo(Mono.Nat.NatProtocol,System.Net.IPEndPoint,System.Net.IPAddress,System.DateTime).PublicAddress'></a>

`PublicAddress` [System\.Net\.IPAddress](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipaddress 'System\.Net\.IPAddress')

The NAT device's public IP address visible to the internet\.

<a name='Pmmux.Abstractions.NatDeviceInfo.NatDeviceInfo(Mono.Nat.NatProtocol,System.Net.IPEndPoint,System.Net.IPAddress,System.DateTime).Discovered'></a>

`Discovered` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The timestamp when the NAT device was discovered\.