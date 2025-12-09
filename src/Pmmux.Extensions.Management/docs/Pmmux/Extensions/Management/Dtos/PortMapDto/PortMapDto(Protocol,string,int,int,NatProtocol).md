#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos').[PortMapDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto')

## PortMapDto\(Protocol, string, int, int, NatProtocol\) Constructor

A DTO object representing a port mapping\.

```csharp
public PortMapDto(Mono.Nat.Protocol NetworkProtocol, string PublicAddress, int PublicPort, int LocalPort, Mono.Nat.NatProtocol NatProtocol);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Dtos.PortMapDto.PortMapDto(Mono.Nat.Protocol,string,int,int,Mono.Nat.NatProtocol).NetworkProtocol'></a>

`NetworkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol being forwarded\.

<a name='Pmmux.Extensions.Management.Dtos.PortMapDto.PortMapDto(Mono.Nat.Protocol,string,int,int,Mono.Nat.NatProtocol).PublicAddress'></a>

`PublicAddress` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The public IP address\.

<a name='Pmmux.Extensions.Management.Dtos.PortMapDto.PortMapDto(Mono.Nat.Protocol,string,int,int,Mono.Nat.NatProtocol).PublicPort'></a>

`PublicPort` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The public port\.

<a name='Pmmux.Extensions.Management.Dtos.PortMapDto.PortMapDto(Mono.Nat.Protocol,string,int,int,Mono.Nat.NatProtocol).LocalPort'></a>

`LocalPort` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The local port that receives forwarded traffic\.

<a name='Pmmux.Extensions.Management.Dtos.PortMapDto.PortMapDto(Mono.Nat.Protocol,string,int,int,Mono.Nat.NatProtocol).NatProtocol'></a>

`NatProtocol` [Mono\.Nat\.NatProtocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.natprotocol 'Mono\.Nat\.NatProtocol')

The NAT protocol used to create the mapping\.