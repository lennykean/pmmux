#### [Pmmux\.Core](../../../../../index.md 'index')
### [Pmmux\.Core\.Configuration](../../index.md 'Pmmux\.Core\.Configuration').[ListenerConfig](../index.md 'Pmmux\.Core\.Configuration\.ListenerConfig').[BindingConfig](index.md 'Pmmux\.Core\.Configuration\.ListenerConfig\.BindingConfig')

## BindingConfig\(Protocol, Nullable\<int\>, Nullable\<int\>, IPAddress, bool\) Constructor

Represents a port binding configuration for a listener\.

```csharp
public BindingConfig(Mono.Nat.Protocol NetworkProtocol, System.Nullable<int> Port, System.Nullable<int> Index=null, System.Net.IPAddress? BindAddress=null, bool Listen=true);
```
#### Parameters

<a name='Pmmux.Core.Configuration.ListenerConfig.BindingConfig.BindingConfig(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Net.IPAddress,bool).NetworkProtocol'></a>

`NetworkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

Network protocol \(TCP or UDP\) for the binding\.

<a name='Pmmux.Core.Configuration.ListenerConfig.BindingConfig.BindingConfig(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Net.IPAddress,bool).Port'></a>

`Port` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Local port number to bind the listener to\.

<a name='Pmmux.Core.Configuration.ListenerConfig.BindingConfig.BindingConfig(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Net.IPAddress,bool).Index'></a>

`Index` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The index that correlates this binding to a port map configuration\.

<a name='Pmmux.Core.Configuration.ListenerConfig.BindingConfig.BindingConfig(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Net.IPAddress,bool).BindAddress'></a>

`BindAddress` [System\.Net\.IPAddress](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipaddress 'System\.Net\.IPAddress')

The IP address to bind the listener to\.

<a name='Pmmux.Core.Configuration.ListenerConfig.BindingConfig.BindingConfig(Mono.Nat.Protocol,System.Nullable_int_,System.Nullable_int_,System.Net.IPAddress,bool).Listen'></a>

`Listen` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the multiplexer should create a listener for this binding\.