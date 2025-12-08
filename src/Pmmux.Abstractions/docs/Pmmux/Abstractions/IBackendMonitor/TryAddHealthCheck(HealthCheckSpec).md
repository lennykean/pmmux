#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IBackendMonitor](index.md 'Pmmux\.Abstractions\.IBackendMonitor')

## IBackendMonitor\.TryAddHealthCheck\(HealthCheckSpec\) Method

Add a new health check specification at runtime\.

```csharp
bool TryAddHealthCheck(Pmmux.Abstractions.HealthCheckSpec healthCheckSpec);
```
#### Parameters

<a name='Pmmux.Abstractions.IBackendMonitor.TryAddHealthCheck(Pmmux.Abstractions.HealthCheckSpec).healthCheckSpec'></a>

`healthCheckSpec` [HealthCheckSpec](../HealthCheckSpec/index.md 'Pmmux\.Abstractions\.HealthCheckSpec')

The health check specification to add\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the specification was added, or `false` if the specification already exists\.