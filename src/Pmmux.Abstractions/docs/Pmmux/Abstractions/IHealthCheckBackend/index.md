#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IHealthCheckBackend Interface

Optional backend interface for health checking support\.

```csharp
public interface IHealthCheckBackend : Pmmux.Abstractions.IBackend, System.IAsyncDisposable
```

Implements [IBackend](../IBackend/index.md 'Pmmux\.Abstractions\.IBackend'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Backends implementing this interface can be monitored by [IBackendMonitor](../IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor') to
continuously assess health status\. Health check results influence routing decisions \- unhealthy
backends are typically excluded from receiving traffic\. Health checks are configured via
[HealthCheckSpec](../HealthCheckSpec/index.md 'Pmmux\.Abstractions\.HealthCheckSpec') using the `--health-check` CLI option or configuration file\.

| Methods | |
| :--- | :--- |
| [HealthCheckAsync\(Protocol, IReadOnlyDictionary&lt;string,string&gt;, CancellationToken\)](HealthCheckAsync(Protocol,IReadOnlyDictionary_string,string_,CancellationToken).md 'Pmmux\.Abstractions\.IHealthCheckBackend\.HealthCheckAsync\(Mono\.Nat\.Protocol, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, System\.Threading\.CancellationToken\)') | Perform a health check to determine operational status\. |
