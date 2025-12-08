#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## HealthCheckSpec Class

Specifies how to perform health checks on backends\.

```csharp
public record HealthCheckSpec : System.IEquatable<Pmmux.Abstractions.HealthCheckSpec>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; HealthCheckSpec

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[HealthCheckSpec](index.md 'Pmmux\.Abstractions\.HealthCheckSpec')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

### Remarks
Health check specifications configure the behavior of [IBackendMonitor](../IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor')\.
Specifications can target:
- All backends if neither [ProtocolName](ProtocolName.md 'Pmmux\.Abstractions\.HealthCheckSpec\.ProtocolName') nor [BackendName](BackendName.md 'Pmmux\.Abstractions\.HealthCheckSpec\.BackendName') is specified
- Backends of a specific protocol if only [ProtocolName](ProtocolName.md 'Pmmux\.Abstractions\.HealthCheckSpec\.ProtocolName') is specified
- A specific backend if both [ProtocolName](ProtocolName.md 'Pmmux\.Abstractions\.HealthCheckSpec\.ProtocolName') and [BackendName](BackendName.md 'Pmmux\.Abstractions\.HealthCheckSpec\.BackendName') are specified

Uses fully value\-based equality\.

| Constructors | |
| :--- | :--- |
| [HealthCheckSpec\(IDictionary&lt;string,string&gt;\)](HealthCheckSpec(IDictionary_string,string_).md 'Pmmux\.Abstractions\.HealthCheckSpec\.HealthCheckSpec\(System\.Collections\.Generic\.IDictionary\<string,string\>\)') | |

| Properties | |
| :--- | :--- |
| [BackendName](BackendName.md 'Pmmux\.Abstractions\.HealthCheckSpec\.BackendName') | Name of a specific backend to target or `null` to match all backends of the specified protocol\. |
| [FailureThreshold](FailureThreshold.md 'Pmmux\.Abstractions\.HealthCheckSpec\.FailureThreshold') | Number of consecutive failures required to mark a backend as unhealthy\. |
| [InitialDelay](InitialDelay.md 'Pmmux\.Abstractions\.HealthCheckSpec\.InitialDelay') | Initial delay before starting health checks\. |
| [Interval](Interval.md 'Pmmux\.Abstractions\.HealthCheckSpec\.Interval') | Interval between health checks\. |
| [Parameters](Parameters.md 'Pmmux\.Abstractions\.HealthCheckSpec\.Parameters') | Backend\-specific health check parameters\. |
| [ProtocolName](ProtocolName.md 'Pmmux\.Abstractions\.HealthCheckSpec\.ProtocolName') | Protocol of backends to target, or `null` to match all protocols\. |
| [RecoveryThreshold](RecoveryThreshold.md 'Pmmux\.Abstractions\.HealthCheckSpec\.RecoveryThreshold') | Number of consecutive successes required to mark an unhealthy backend as healthy again\. |
| [Timeout](Timeout.md 'Pmmux\.Abstractions\.HealthCheckSpec\.Timeout') | Timeout for each health check operation\. |
