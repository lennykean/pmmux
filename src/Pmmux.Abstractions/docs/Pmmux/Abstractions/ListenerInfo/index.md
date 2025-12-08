#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## ListenerInfo Class

Information about a network listener and its accessibility\.

```csharp
public record ListenerInfo : System.IEquatable<Pmmux.Abstractions.ListenerInfo>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; ListenerInfo

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ListenerInfo](index.md 'Pmmux\.Abstractions\.ListenerInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

### Remarks
Describes a bound socket listener and optionally its external accessibility through a NAT port mapping\.

| Constructors | |
| :--- | :--- |
| [ListenerInfo\(Protocol, IPEndPoint, IDictionary&lt;string,string&gt;\)](ListenerInfo(Protocol,IPEndPoint,IDictionary_string,string_).md 'Pmmux\.Abstractions\.ListenerInfo\.ListenerInfo\(Mono\.Nat\.Protocol, System\.Net\.IPEndPoint, System\.Collections\.Generic\.IDictionary\<string,string\>\)') | |

| Properties | |
| :--- | :--- |
| [LocalEndPoint](LocalEndPoint.md 'Pmmux\.Abstractions\.ListenerInfo\.LocalEndPoint') | The local endpoint where the listener is bound\. |
| [NetworkProtocol](NetworkProtocol.md 'Pmmux\.Abstractions\.ListenerInfo\.NetworkProtocol') | The network protocol that the listener accepts\. |
| [Properties](Properties.md 'Pmmux\.Abstractions\.ListenerInfo\.Properties') | The listener\-specific properties and metadata\. |
