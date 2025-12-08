#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IEventSender](index.md 'Pmmux\.Abstractions\.IEventSender')

## IEventSender\.RaiseBackendAdded\(object, BackendSpec\) Method

Raise an event indicating a backend was added\.

```csharp
void RaiseBackendAdded(object sender, Pmmux.Abstractions.BackendSpec backend);
```
#### Parameters

<a name='Pmmux.Abstractions.IEventSender.RaiseBackendAdded(object,Pmmux.Abstractions.BackendSpec).sender'></a>

`sender` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

The component raising the event\.

<a name='Pmmux.Abstractions.IEventSender.RaiseBackendAdded(object,Pmmux.Abstractions.BackendSpec).backend'></a>

`backend` [BackendSpec](../BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The backend spec that was added\.