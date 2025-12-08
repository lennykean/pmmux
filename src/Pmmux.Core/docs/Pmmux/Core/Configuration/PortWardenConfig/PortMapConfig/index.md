#### [Pmmux\.Core](../../../../../index.md 'index')
### [Pmmux\.Core\.Configuration](../../index.md 'Pmmux\.Core\.Configuration').[PortWardenConfig](../index.md 'Pmmux\.Core\.Configuration\.PortWardenConfig')

## PortWardenConfig\.PortMapConfig Class

Represents a NAT port mapping configuration\.

```csharp
public record PortWardenConfig.PortMapConfig : System.IEquatable<Pmmux.Core.Configuration.PortWardenConfig.PortMapConfig>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PortMapConfig

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[PortMapConfig](index.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.PortMapConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [PortMapConfig\(Protocol, Nullable&lt;int&gt;, Nullable&lt;int&gt;\)](PortMapConfig(Protocol,Nullable_int_,Nullable_int_).md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.PortMapConfig\.PortMapConfig\(Mono\.Nat\.Protocol, System\.Nullable\<int\>, System\.Nullable\<int\>\)') | Represents a NAT port mapping configuration\. |

| Properties | |
| :--- | :--- |
| [LocalPort](LocalPort.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.PortMapConfig\.LocalPort') | Local port to map\. |
| [NetworkProtocol](NetworkProtocol.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.PortMapConfig\.NetworkProtocol') | Network protocol \(TCP or UDP\) for the port mapping\. |
| [PublicPort](PublicPort.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.PortMapConfig\.PublicPort') | Public port to request on the NAT device\. |
