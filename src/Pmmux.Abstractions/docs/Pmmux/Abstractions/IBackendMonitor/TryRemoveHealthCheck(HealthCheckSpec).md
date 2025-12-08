#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IBackendMonitor](index.md 'Pmmux\.Abstractions\.IBackendMonitor')

## IBackendMonitor\.TryRemoveHealthCheck\(HealthCheckSpec\) Method

Remove an existing health check specification at runtime\.

```csharp
bool TryRemoveHealthCheck(Pmmux.Abstractions.HealthCheckSpec healthCheckSpec);
```
#### Parameters

<a name='Pmmux.Abstractions.IBackendMonitor.TryRemoveHealthCheck(Pmmux.Abstractions.HealthCheckSpec).healthCheckSpec'></a>

`healthCheckSpec` [HealthCheckSpec](../HealthCheckSpec/index.md 'Pmmux\.Abstractions\.HealthCheckSpec')

The health check specification to remove\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the specification was removed, `false` if no matching specification exists\.