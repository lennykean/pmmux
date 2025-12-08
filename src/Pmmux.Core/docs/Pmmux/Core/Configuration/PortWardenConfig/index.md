#### [Pmmux\.Core](../../../../index.md 'index')
### [Pmmux\.Core\.Configuration](../index.md 'Pmmux\.Core\.Configuration')

## PortWardenConfig Class

Configuration for NAT port mapping \(UPnP/PMP\)\.

```csharp
public record PortWardenConfig : System.IEquatable<Pmmux.Core.Configuration.PortWardenConfig>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PortWardenConfig

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[PortWardenConfig](index.md 'Pmmux\.Core\.Configuration\.PortWardenConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Properties | |
| :--- | :--- |
| [GatewayAddress](GatewayAddress.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.GatewayAddress') | Gateway address to search for NAT devices\. |
| [Lifetime](Lifetime.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.Lifetime') | Requested lifetime for port mappings\. |
| [NatProtocol](NatProtocol.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.NatProtocol') | NAT protocol to use \(UPnP or PMP\)\. |
| [NetworkInterface](NetworkInterface.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.NetworkInterface') | Network interface to use for NAT discovery\. |
| [PortMaps](PortMaps.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.PortMaps') | List of port mappings to create\. |
| [RenewalLead](RenewalLead.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.RenewalLead') | Time before expiration to renew port mappings\. |
| [Timeout](Timeout.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.Timeout') | Timeout for NAT device discovery\. |
