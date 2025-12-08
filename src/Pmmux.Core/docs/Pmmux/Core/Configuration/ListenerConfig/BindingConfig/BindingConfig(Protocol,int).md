#### [Pmmux\.Core](../../../../../index.md 'index')
### [Pmmux\.Core\.Configuration](../../index.md 'Pmmux\.Core\.Configuration').[ListenerConfig](../index.md 'Pmmux\.Core\.Configuration\.ListenerConfig').[BindingConfig](index.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig')

## BindingConfig\(Protocol, int\) Constructor

Represents a port binding configuration for a listener\.

```csharp
public BindingConfig(Mono.Nat.Protocol NetworkProtocol, int Port);
```
#### Parameters

<a name='Pmmux.Core.Configuration.ListenerConfig.BindingConfig.BindingConfig(Mono.Nat.Protocol,int).NetworkProtocol'></a>

`NetworkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

Network protocol \(TCP or UDP\) for the binding\.

<a name='Pmmux.Core.Configuration.ListenerConfig.BindingConfig.BindingConfig(Mono.Nat.Protocol,int).Port'></a>

`Port` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Local port number to bind the listener to\.