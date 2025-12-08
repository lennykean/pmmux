#### [Pmmux\.Core](../../../../../index.md 'index')
### [Pmmux\.Core\.Configuration](../../index.md 'Pmmux\.Core\.Configuration').[PortWardenConfig](../index.md 'Pmmux\.Core\.Configuration\.PortWardenConfig').[PortMapConfig](index.md 'Pmmux\.Core\.Configuration\.PortWardenConfig\.PortMapConfig')

## PortMapConfig\(Protocol, Nullable\<int\>, Nullable\<int\>\) Constructor

Represents a NAT port mapping configuration\.

```csharp
public PortMapConfig(Mono.Nat.Protocol NetworkProtocol, System.Nullable<int> LocalPort=null, System.Nullable<int> PublicPort=null);
```
#### Parameters

<a name='Pmmux.Core.Configuration.PortWardenConfig.PortMapConfig.PortMapConfig(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_).NetworkProtocol'></a>

`NetworkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

Network protocol \(TCP or UDP\) for the port mapping\.

<a name='Pmmux.Core.Configuration.PortWardenConfig.PortMapConfig.PortMapConfig(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_).LocalPort'></a>

`LocalPort` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Local port to map\.

<a name='Pmmux.Core.Configuration.PortWardenConfig.PortMapConfig.PortMapConfig(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_).PublicPort'></a>

`PublicPort` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Public port to request on the NAT device\.