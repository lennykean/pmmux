#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos')

## PortMapDto Class

A DTO object representing a port mapping\.

```csharp
public record PortMapDto : System.IEquatable<Pmmux.Extensions.Management.Dtos.PortMapDto>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PortMapDto

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[PortMapDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [PortMapDto\(Protocol, string, int, int, NatProtocol\)](PortMapDto(Protocol,string,int,int,NatProtocol).md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto\.PortMapDto\(Mono\.Nat\.Protocol, string, int, int, Mono\.Nat\.NatProtocol\)') | A DTO object representing a port mapping\. |

| Properties | |
| :--- | :--- |
| [LocalPort](LocalPort.md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto\.LocalPort') | The local port that receives forwarded traffic\. |
| [NatProtocol](NatProtocol.md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto\.NatProtocol') | The NAT protocol used to create the mapping\. |
| [NetworkProtocol](NetworkProtocol.md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto\.NetworkProtocol') | The network protocol being forwarded\. |
| [PublicAddress](PublicAddress.md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto\.PublicAddress') | The public IP address\. |
| [PublicPort](PublicPort.md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto\.PublicPort') | The public port\. |

| Methods | |
| :--- | :--- |
| [FromPortMapInfo\(PortMapInfo\)](FromPortMapInfo(PortMapInfo).md 'Pmmux\.Extensions\.Management\.Dtos\.PortMapDto\.FromPortMapInfo\(Pmmux\.Abstractions\.PortMapInfo\)') | Create a DTO from a [PortMapInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/PortMapInfo/index.md 'Pmmux\.Abstractions\.PortMapInfo')\. |
