#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## PortMapInfo Class

Represents information about a NAT port mapping between a public endpoint and a local port\.

```csharp
public record PortMapInfo : System.IEquatable<Pmmux.Abstractions.PortMapInfo>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PortMapInfo

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[PortMapInfo](index.md 'Pmmux\.Abstractions\.PortMapInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [PortMapInfo\(Protocol, IPEndPoint, int, NatProtocol, Nullable&lt;int&gt;\)](PortMapInfo(Protocol,IPEndPoint,int,NatProtocol,Nullable_int_).md 'Pmmux\.Abstractions\.PortMapInfo\.PortMapInfo\(Mono\.Nat\.Protocol, System\.Net\.IPEndPoint, int, Mono\.Nat\.NatProtocol, System\.Nullable\<int\>\)') | Represents information about a NAT port mapping between a public endpoint and a local port\. |

| Properties | |
| :--- | :--- |
| [Index](Index.md 'Pmmux\.Abstractions\.PortMapInfo\.Index') | The index that correlates this mapping to a listener binding configuration\. |
| [LocalPort](LocalPort.md 'Pmmux\.Abstractions\.PortMapInfo\.LocalPort') | The local port that receives traffic forwarded from the public port\. |
| [NatProtocol](NatProtocol.md 'Pmmux\.Abstractions\.PortMapInfo\.NatProtocol') | The NAT protocol used to create the mapping\. |
| [NetworkProtocol](NetworkProtocol.md 'Pmmux\.Abstractions\.PortMapInfo\.NetworkProtocol') | The network protocol being forwarded\. |
| [PublicEndpoint](PublicEndpoint.md 'Pmmux\.Abstractions\.PortMapInfo\.PublicEndpoint') | The public endpoint accessible from outside the LAN\. |
