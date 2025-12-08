#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IEventSender](index.md 'Pmmux\.Abstractions\.IEventSender')

## IEventSender\.RaiseHealthCheckRemoved\(object, HealthCheckSpec\) Method

Raise an event indicating a health check was removed\.

```csharp
void RaiseHealthCheckRemoved(object sender, Pmmux.Abstractions.HealthCheckSpec healthCheck);
```
#### Parameters

<a name='Pmmux.Abstractions.IEventSender.RaiseHealthCheckRemoved(object,Pmmux.Abstractions.HealthCheckSpec).sender'></a>

`sender` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

The component raising the event\.

<a name='Pmmux.Abstractions.IEventSender.RaiseHealthCheckRemoved(object,Pmmux.Abstractions.HealthCheckSpec).healthCheck'></a>

`healthCheck` [HealthCheckSpec](../HealthCheckSpec/index.md 'Pmmux\.Abstractions\.HealthCheckSpec')

The health check spec that was removed\.