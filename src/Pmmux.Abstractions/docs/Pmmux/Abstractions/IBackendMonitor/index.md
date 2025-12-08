#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IBackendMonitor Interface

Monitors backend health status based on configured health check specifications\.

```csharp
public interface IBackendMonitor : System.IAsyncDisposable
```

Implements [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Backend monitor performs health checks on backends that implement [IHealthCheckBackend](../IHealthCheckBackend/index.md 'Pmmux\.Abstractions\.IHealthCheckBackend')\.
Health check results can help determine whether a backend is available for routing\.

| Methods | |
| :--- | :--- |
| [GetHealthChecks\(\)](GetHealthChecks().md 'Pmmux\.Abstractions\.IBackendMonitor\.GetHealthChecks\(\)') | Get all currently configured health check specifications\. |
| [MonitorAsync\(IHealthCheckBackend, Protocol, CancellationToken, CancellationToken\)](MonitorAsync(IHealthCheckBackend,Protocol,CancellationToken,CancellationToken).md 'Pmmux\.Abstractions\.IBackendMonitor\.MonitorAsync\(Pmmux\.Abstractions\.IHealthCheckBackend, Mono\.Nat\.Protocol, System\.Threading\.CancellationToken, System\.Threading\.CancellationToken\)') | Monitor the health of a backend and provide status updates as an async stream\. |
| [TryAddHealthCheck\(HealthCheckSpec\)](TryAddHealthCheck(HealthCheckSpec).md 'Pmmux\.Abstractions\.IBackendMonitor\.TryAddHealthCheck\(Pmmux\.Abstractions\.HealthCheckSpec\)') | Add a new health check specification at runtime\. |
| [TryRemoveHealthCheck\(HealthCheckSpec\)](TryRemoveHealthCheck(HealthCheckSpec).md 'Pmmux\.Abstractions\.IBackendMonitor\.TryRemoveHealthCheck\(Pmmux\.Abstractions\.HealthCheckSpec\)') | Remove an existing health check specification at runtime\. |
