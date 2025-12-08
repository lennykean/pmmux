#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## BackendMonitor Class

Default implementation of [IBackendMonitor](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor')\.

```csharp
public sealed class BackendMonitor : Pmmux.Abstractions.IBackendMonitor, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BackendMonitor

Implements [IBackendMonitor](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Constructors | |
| :--- | :--- |
| [BackendMonitor\(RouterConfig, ILoggerFactory, IMetricReporter, IEventSender\)](BackendMonitor(RouterConfig,ILoggerFactory,IMetricReporter,IEventSender).md 'Pmmux\.Core\.BackendMonitor\.BackendMonitor\(Pmmux\.Core\.Configuration\.RouterConfig, Microsoft\.Extensions\.Logging\.ILoggerFactory, Pmmux\.Abstractions\.IMetricReporter, Pmmux\.Abstractions\.IEventSender\)') | Default implementation of [IBackendMonitor](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor')\. |

| Methods | |
| :--- | :--- |
| [GetHealthChecks\(\)](GetHealthChecks().md 'Pmmux\.Core\.BackendMonitor\.GetHealthChecks\(\)') | Get all currently configured health check specifications\. |
| [MonitorAsync\(IHealthCheckBackend, Protocol, CancellationToken, CancellationToken\)](MonitorAsync(IHealthCheckBackend,Protocol,CancellationToken,CancellationToken).md 'Pmmux\.Core\.BackendMonitor\.MonitorAsync\(Pmmux\.Abstractions\.IHealthCheckBackend, Mono\.Nat\.Protocol, System\.Threading\.CancellationToken, System\.Threading\.CancellationToken\)') | Monitor the health of a backend and provide status updates as an async stream\. |
| [TryAddHealthCheck\(HealthCheckSpec\)](TryAddHealthCheck(HealthCheckSpec).md 'Pmmux\.Core\.BackendMonitor\.TryAddHealthCheck\(Pmmux\.Abstractions\.HealthCheckSpec\)') | Add a new health check specification at runtime\. |
| [TryRemoveHealthCheck\(HealthCheckSpec\)](TryRemoveHealthCheck(HealthCheckSpec).md 'Pmmux\.Core\.BackendMonitor\.TryRemoveHealthCheck\(Pmmux\.Abstractions\.HealthCheckSpec\)') | Remove an existing health check specification at runtime\. |
