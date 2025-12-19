#### [Pmmux\.Extensions\.Management\.Models](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Models](../index.md 'Pmmux\.Extensions\.Management\.Models')

## HealthCheckSpecDto Class

Health check specification DTO for API requests\.

```csharp
public class HealthCheckSpecDto
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; HealthCheckSpecDto

| Properties | |
| :--- | :--- |
| [BackendName](BackendName.md 'Pmmux\.Extensions\.Management\.Models\.HealthCheckSpecDto\.BackendName') | Backend name \(null for all backends\)\. |
| [FailureThreshold](FailureThreshold.md 'Pmmux\.Extensions\.Management\.Models\.HealthCheckSpecDto\.FailureThreshold') | Number of failures before marking unhealthy\. |
| [InitialDelay](InitialDelay.md 'Pmmux\.Extensions\.Management\.Models\.HealthCheckSpecDto\.InitialDelay') | Initial delay before first health check in milliseconds\. |
| [Interval](Interval.md 'Pmmux\.Extensions\.Management\.Models\.HealthCheckSpecDto\.Interval') | Interval between health checks in milliseconds\. |
| [Parameters](Parameters.md 'Pmmux\.Extensions\.Management\.Models\.HealthCheckSpecDto\.Parameters') | Health check parameters\. |
| [ProtocolName](ProtocolName.md 'Pmmux\.Extensions\.Management\.Models\.HealthCheckSpecDto\.ProtocolName') | Protocol name \(null for all protocols\)\. |
| [RecoveryThreshold](RecoveryThreshold.md 'Pmmux\.Extensions\.Management\.Models\.HealthCheckSpecDto\.RecoveryThreshold') | Number of successes before marking healthy\. |
| [Timeout](Timeout.md 'Pmmux\.Extensions\.Management\.Models\.HealthCheckSpecDto\.Timeout') | Timeout for health checks in milliseconds\. |
