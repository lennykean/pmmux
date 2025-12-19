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
| [BindingConfig\(Protocol, Nullable&lt;int&gt;, Nullable&lt;int&gt;, IPAddress, bool\)](BindingConfig(Protocol,Nullable_int_,Nullable_int_,IPAddress,bool).md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.BindingConfig\(Mono\.Nat\.Protocol, System\.Nullable\<int\>, System\.Nullable\<int\>, System\.Net\.IPAddress, bool\)') | Represents a port binding configuration for a listener\. |

| Properties | |
| :--- | :--- |
| [BindAddress](BindAddress.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.BindAddress') | The IP address to bind the listener to\. |
| [Index](Index.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.Index') | The index that correlates this binding to a port map configuration\. |
| [Listen](Listen.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.Listen') | Whether the multiplexer should create a listener for this binding\. |
| [NetworkProtocol](NetworkProtocol.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.NetworkProtocol') | Network protocol \(TCP or UDP\) for the binding\. |
| [Port](Port.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig\.Port') | Local port number to bind the listener to\. |
