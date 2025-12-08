#### [Pmmux\.Core](../../../../../index.md 'index')
### [Pmmux\.Core\.Configuration](../../index.md 'Pmmux\.Core\.Configuration').[ListenerConfig](../index.md 'Pmmux\.Core\.Configuration\.ListenerConfig')

## ListenerConfig\.BindingConfig Class

Represents a port binding configuration for a listener\.

```csharp
public record ListenerConfig.BindingConfig : System.IEquatable<Pmmux.Core.Configuration.ListenerConfig.BindingConfig>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BindingConfig

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[BindingConfig](index.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [BindingConfig\(Protocol, int\)](BindingConfig(Protocol,int).md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.BindingConfig\(Mono\.Nat\.Protocol, int\)') | Represents a port binding configuration for a listener\. |

| Properties | |
| :--- | :--- |
| [NetworkProtocol](NetworkProtocol.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.NetworkProtocol') | Network protocol \(TCP or UDP\) for the binding\. |
| [Port](Port.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.Port') | Local port number to bind the listener to\. |
