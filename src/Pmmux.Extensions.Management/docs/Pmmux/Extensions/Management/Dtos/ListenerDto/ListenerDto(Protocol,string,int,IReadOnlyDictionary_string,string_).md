#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos').[ListenerDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.ListenerDto')

## ListenerDto\(Protocol, string, int, IReadOnlyDictionary\<string,string\>\) Constructor

A DTO object representing a listener\.

```csharp
public ListenerDto(Mono.Nat.Protocol NetworkProtocol, string Address, int Port, System.Collections.Generic.IReadOnlyDictionary<string,string> Properties);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Dtos.ListenerDto.ListenerDto(Mono.Nat.Protocol,string,int,System.Collections.Generic.IReadOnlyDictionary_string,string_).NetworkProtocol'></a>

`NetworkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol\.

<a name='Pmmux.Extensions.Management.Dtos.ListenerDto.ListenerDto(Mono.Nat.Protocol,string,int,System.Collections.Generic.IReadOnlyDictionary_string,string_).Address'></a>

`Address` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The bound address\.

<a name='Pmmux.Extensions.Management.Dtos.ListenerDto.ListenerDto(Mono.Nat.Protocol,string,int,System.Collections.Generic.IReadOnlyDictionary_string,string_).Port'></a>

`Port` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The bound port\.

<a name='Pmmux.Extensions.Management.Dtos.ListenerDto.ListenerDto(Mono.Nat.Protocol,string,int,System.Collections.Generic.IReadOnlyDictionary_string,string_).Properties'></a>

`Properties` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

Listener properties\.