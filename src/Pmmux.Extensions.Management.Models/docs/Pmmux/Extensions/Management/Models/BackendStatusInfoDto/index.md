#### [Pmmux\.Extensions\.Management\.Models](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Models](../index.md 'Pmmux\.Extensions\.Management\.Models')

## BackendStatusInfoDto Class

Backend runtime information DTO\.

```csharp
public record BackendStatusInfoDto : System.IEquatable<Pmmux.Extensions.Management.Models.BackendStatusInfoDto>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BackendStatusInfoDto

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[BackendStatusInfoDto](index.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Properties | |
| :--- | :--- |
| [HealthCheckFailureCount](HealthCheckFailureCount.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto\.HealthCheckFailureCount') | The number of consecutive health check failures\. |
| [HealthCheckSuccessCount](HealthCheckSuccessCount.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto\.HealthCheckSuccessCount') | The number of consecutive health check successes\. |
| [LastHealthCheck](LastHealthCheck.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto\.LastHealthCheck') | The timestamp of the last health check\. |
| [PriorityTier](PriorityTier.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto\.PriorityTier') | The routing priority tier\. |
| [Properties](Properties.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto\.Properties') | Runtime properties provided by the backend implementation\. |
| [Spec](Spec.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto\.Spec') | The specification that defines the backend\. |
| [Status](Status.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto\.Status') | The backend status\. |
| [StatusReason](StatusReason.md 'Pmmux\.Extensions\.Management\.Models\.BackendStatusInfoDto\.StatusReason') | The reason for the current status\. |
