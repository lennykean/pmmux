#### [Pmmux\.Core](../../../../index.md 'index')
### [Pmmux\.Core\.Configuration](../index.md 'Pmmux\.Core\.Configuration')

## ListenerConfig Class

Configuration for TCP/UDP listeners\.

```csharp
public record ListenerConfig : System.IEquatable<Pmmux.Core.Configuration.ListenerConfig>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; ListenerConfig

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ListenerConfig](index.md 'Pmmux\.Core\.Configuration\.ListenerConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Properties | |
| :--- | :--- |
| [PortBindings](PortBindings.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.PortBindings') | List of port bindings for the listener\. |
| [QueueLength](QueueLength.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.QueueLength') | Maximum queue length for incoming connections\. |
