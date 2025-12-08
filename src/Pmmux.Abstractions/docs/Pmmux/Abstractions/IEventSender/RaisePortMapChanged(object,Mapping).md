#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IEventSender](index.md 'Pmmux\.Abstractions\.IEventSender')

## IEventSender\.RaisePortMapChanged\(object, Mapping\) Method

Raise an event indicating a port map was changed \(renewed\)\.

```csharp
void RaisePortMapChanged(object sender, Mono.Nat.Mapping mapping);
```
#### Parameters

<a name='Pmmux.Abstractions.IEventSender.RaisePortMapChanged(object,Mono.Nat.Mapping).sender'></a>

`sender` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

The component raising the event\.

<a name='Pmmux.Abstractions.IEventSender.RaisePortMapChanged(object,Mono.Nat.Mapping).mapping'></a>

`mapping` [Mono\.Nat\.Mapping](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.mapping 'Mono\.Nat\.Mapping')

The updated port mapping\.