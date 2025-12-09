#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos').[BackendSpecDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto')

## BackendSpecDto\(string, string, IReadOnlyDictionary\<string,string\>\) Constructor

A DTO object representing a backend specification\.

```csharp
public BackendSpecDto(string Name, string ProtocolName, System.Collections.Generic.IReadOnlyDictionary<string,string> Parameters);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Dtos.BackendSpecDto.BackendSpecDto(string,string,System.Collections.Generic.IReadOnlyDictionary_string,string_).Name'></a>

`Name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique name identifying the backend\.

<a name='Pmmux.Extensions.Management.Dtos.BackendSpecDto.BackendSpecDto(string,string,System.Collections.Generic.IReadOnlyDictionary_string,string_).ProtocolName'></a>

`ProtocolName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the protocol that creates the backend\.

<a name='Pmmux.Extensions.Management.Dtos.BackendSpecDto.BackendSpecDto(string,string,System.Collections.Generic.IReadOnlyDictionary_string,string_).Parameters'></a>

`Parameters` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

Protocol\-specific configuration parameters\.