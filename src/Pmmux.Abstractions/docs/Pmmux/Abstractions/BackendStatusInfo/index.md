#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## BackendStatusInfo Class

Health status and statistics for a backend\.

```csharp
public record BackendStatusInfo : System.IEquatable<Pmmux.Abstractions.BackendStatusInfo>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BackendStatusInfo

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[BackendStatusInfo](index.md 'Pmmux\.Abstractions\.BackendStatusInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [BackendStatusInfo\(BackendInfo, BackendStatus, string, Nullable&lt;DateTime&gt;, Nullable&lt;long&gt;, Nullable&lt;long&gt;\)](BackendStatusInfo(BackendInfo,BackendStatus,string,Nullable_DateTime_,Nullable_long_,Nullable_long_).md 'Pmmux\.Abstractions\.BackendStatusInfo\.BackendStatusInfo\(Pmmux\.Abstractions\.BackendInfo, Pmmux\.Abstractions\.BackendStatus, string, System\.Nullable\<System\.DateTime\>, System\.Nullable\<long\>, System\.Nullable\<long\>\)') | Health status and statistics for a backend\. |

| Properties | |
| :--- | :--- |
| [Backend](Backend.md 'Pmmux\.Abstractions\.BackendStatusInfo\.Backend') | The backend that the status information pertains to\. |
| [HealthCheckFailureCount](HealthCheckFailureCount.md 'Pmmux\.Abstractions\.BackendStatusInfo\.HealthCheckFailureCount') | The number of most recent successive failed health checks\. |
| [HealthCheckSuccessCount](HealthCheckSuccessCount.md 'Pmmux\.Abstractions\.BackendStatusInfo\.HealthCheckSuccessCount') | The number of most recent successive successful health checks\. |
| [LastHealthCheck](LastHealthCheck.md 'Pmmux\.Abstractions\.BackendStatusInfo\.LastHealthCheck') | The timestamp of the most recent health check if one has been performed\. |
| [Status](Status.md 'Pmmux\.Abstractions\.BackendStatusInfo\.Status') | The current health status\. |
| [StatusReason](StatusReason.md 'Pmmux\.Abstractions\.BackendStatusInfo\.StatusReason') | Optional reason for the current status\. |
