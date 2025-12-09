#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos').[PortRequestDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.PortRequestDto')

## PortRequestDto\(Protocol, Nullable\<int\>, Nullable\<int\>\) Constructor

A DTO object for port map add/remove requests\.

```csharp
public PortRequestDto(Mono.Nat.Protocol NetworkProtocol, System.Nullable<int> LocalPort, System.Nullable<int> PublicPort);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Dtos.PortRequestDto.PortRequestDto(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_).NetworkProtocol'></a>

`NetworkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol\.

<a name='Pmmux.Extensions.Management.Dtos.PortRequestDto.PortRequestDto(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_).LocalPort'></a>

`LocalPort` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The local port number\.

<a name='Pmmux.Extensions.Management.Dtos.PortRequestDto.PortRequestDto(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_).PublicPort'></a>

`PublicPort` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The public port number\.