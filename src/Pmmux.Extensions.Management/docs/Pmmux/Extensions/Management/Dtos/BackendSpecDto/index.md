#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos')

## BackendSpecDto Class

A DTO object representing a backend specification\.

```csharp
public record BackendSpecDto : System.IEquatable<Pmmux.Extensions.Management.Dtos.BackendSpecDto>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BackendSpecDto

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[BackendSpecDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [BackendSpecDto\(string, string, IReadOnlyDictionary&lt;string,string&gt;\)](BackendSpecDto(string,string,IReadOnlyDictionary_string,string_).md 'Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto\.BackendSpecDto\(string, string, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>\)') | A DTO object representing a backend specification\. |

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto\.Name') | The unique name identifying the backend\. |
| [Parameters](Parameters.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto\.Parameters') | Protocol\-specific configuration parameters\. |
| [ProtocolName](ProtocolName.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto\.ProtocolName') | The name of the protocol that creates the backend\. |

| Methods | |
| :--- | :--- |
| [FromBackendSpec\(BackendSpec\)](FromBackendSpec(BackendSpec).md 'Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto\.FromBackendSpec\(Pmmux\.Abstractions\.BackendSpec\)') | Create a DTO from a [BackendSpec](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')\. |
