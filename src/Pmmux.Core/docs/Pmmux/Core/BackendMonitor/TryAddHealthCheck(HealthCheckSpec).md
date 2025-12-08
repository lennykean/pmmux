#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[BackendMonitor](index.md 'Pmmux\.Core\.BackendMonitor')

## BackendMonitor\.TryAddHealthCheck\(HealthCheckSpec\) Method

Add a new health check specification at runtime\.

```csharp
public bool TryAddHealthCheck(Pmmux.Abstractions.HealthCheckSpec healthCheckSpec);
```
#### Parameters

<a name='Pmmux.Core.BackendMonitor.TryAddHealthCheck(Pmmux.Abstractions.HealthCheckSpec).healthCheckSpec'></a>

`healthCheckSpec` [HealthCheckSpec](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/HealthCheckSpec/index.md 'Pmmux\.Abstractions\.HealthCheckSpec')

The health check specification to add\.

Implements [TryAddHealthCheck\(HealthCheckSpec\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendMonitor/TryAddHealthCheck(HealthCheckSpec).md 'Pmmux\.Abstractions\.IBackendMonitor\.TryAddHealthCheck\(Pmmux\.Abstractions\.HealthCheckSpec\)')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the specification was added, or `false` if the specification already exists\.