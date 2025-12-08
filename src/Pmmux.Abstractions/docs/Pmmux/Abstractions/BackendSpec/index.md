#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## BackendSpec Class

Specification for creating a backend instance\.

```csharp
public sealed record BackendSpec : System.IEquatable<Pmmux.Abstractions.BackendSpec>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BackendSpec

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[BackendSpec](index.md 'Pmmux\.Abstractions\.BackendSpec')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [BackendSpec\(string, string, IDictionary&lt;string,string&gt;\)](BackendSpec(string,string,IDictionary_string,string_).md 'Pmmux\.Abstractions\.BackendSpec\.BackendSpec\(string, string, System\.Collections\.Generic\.IDictionary\<string,string\>\)') | |

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Abstractions\.BackendSpec\.Name') | The unique name identifying the backend\. |
| [Parameters](Parameters.md 'Pmmux\.Abstractions\.BackendSpec\.Parameters') | Protocol\-specific configuration parameters\. |
| [ProtocolName](ProtocolName.md 'Pmmux\.Abstractions\.BackendSpec\.ProtocolName') | The name of the [IBackendProtocol](../IBackendProtocol/index.md 'Pmmux\.Abstractions\.IBackendProtocol') that creates the backend instance\. |
