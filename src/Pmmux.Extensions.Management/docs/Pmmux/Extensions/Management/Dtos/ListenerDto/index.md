#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos')

## ListenerDto Class

A DTO object representing a listener\.

```csharp
public record ListenerDto : System.IEquatable<Pmmux.Extensions.Management.Dtos.ListenerDto>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; ListenerDto

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ListenerDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.ListenerDto')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [ListenerDto\(Protocol, string, int, IReadOnlyDictionary&lt;string,string&gt;\)](ListenerDto(Protocol,string,int,IReadOnlyDictionary_string,string_).md 'Pmmux\.Extensions\.Management\.Dtos\.ListenerDto\.ListenerDto\(Mono\.Nat\.Protocol, string, int, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>\)') | A DTO object representing a listener\. |

| Properties | |
| :--- | :--- |
| [Address](Address.md 'Pmmux\.Extensions\.Management\.Dtos\.ListenerDto\.Address') | The bound address\. |
| [NetworkProtocol](NetworkProtocol.md 'Pmmux\.Extensions\.Management\.Dtos\.ListenerDto\.NetworkProtocol') | The network protocol\. |
| [Port](Port.md 'Pmmux\.Extensions\.Management\.Dtos\.ListenerDto\.Port') | The bound port\. |
| [Properties](Properties.md 'Pmmux\.Extensions\.Management\.Dtos\.ListenerDto\.Properties') | Listener properties\. |

| Methods | |
| :--- | :--- |
| [FromListenerInfo\(ListenerInfo\)](FromListenerInfo(ListenerInfo).md 'Pmmux\.Extensions\.Management\.Dtos\.ListenerDto\.FromListenerInfo\(Pmmux\.Abstractions\.ListenerInfo\)') | Create a DTO from a [ListenerInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo')\. |
