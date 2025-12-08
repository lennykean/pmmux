#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[BackendMonitor](index.md 'Pmmux\.Core\.BackendMonitor')

## BackendMonitor\.TryRemoveHealthCheck\(HealthCheckSpec\) Method

Remove an existing health check specification at runtime\.

```csharp
public bool TryRemoveHealthCheck(Pmmux.Abstractions.HealthCheckSpec healthCheckSpec);
```
#### Parameters

<a name='Pmmux.Core.BackendMonitor.TryRemoveHealthCheck(Pmmux.Abstractions.HealthCheckSpec).healthCheckSpec'></a>

`healthCheckSpec` [HealthCheckSpec](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/HealthCheckSpec/index.md 'Pmmux\.Abstractions\.HealthCheckSpec')

The health check specification to remove\.

Implements [TryRemoveHealthCheck\(HealthCheckSpec\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendMonitor/TryRemoveHealthCheck(HealthCheckSpec).md 'Pmmux\.Abstractions\.IBackendMonitor\.TryRemoveHealthCheck\(Pmmux\.Abstractions\.HealthCheckSpec\)')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the specification was removed, `false` if no matching specification exists\.