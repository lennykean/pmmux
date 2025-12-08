#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## NatDeviceInfo Class

Information about a discovered NAT device on the network\.

```csharp
public sealed record NatDeviceInfo : System.IEquatable<Pmmux.Abstractions.NatDeviceInfo>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; NatDeviceInfo

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[NatDeviceInfo](index.md 'Pmmux\.Abstractions\.NatDeviceInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [NatDeviceInfo\(NatProtocol, IPEndPoint, IPAddress, DateTime\)](NatDeviceInfo(NatProtocol,IPEndPoint,IPAddress,DateTime).md 'Pmmux\.Abstractions\.NatDeviceInfo\.NatDeviceInfo\(Mono\.Nat\.NatProtocol, System\.Net\.IPEndPoint, System\.Net\.IPAddress, System\.DateTime\)') | Information about a discovered NAT device on the network\. |

| Properties | |
| :--- | :--- |
| [Discovered](Discovered.md 'Pmmux\.Abstractions\.NatDeviceInfo\.Discovered') | The timestamp when the NAT device was discovered\. |
| [Endpoint](Endpoint.md 'Pmmux\.Abstractions\.NatDeviceInfo\.Endpoint') | The network endpoint of the NAT device\. |
| [NatProtocol](NatProtocol.md 'Pmmux\.Abstractions\.NatDeviceInfo\.NatProtocol') | The protocol used to discover and communicate with the NAT device\. |
| [PublicAddress](PublicAddress.md 'Pmmux\.Abstractions\.NatDeviceInfo\.PublicAddress') | The NAT device's public IP address visible to the internet\. |
