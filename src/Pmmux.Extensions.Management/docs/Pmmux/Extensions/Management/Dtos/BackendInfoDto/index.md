#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos')

## BackendInfoDto Class

A DTO object representing backend runtime information\.

```csharp
public record BackendInfoDto : System.IEquatable<Pmmux.Extensions.Management.Dtos.BackendInfoDto>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BackendInfoDto

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[BackendInfoDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [BackendInfoDto\(BackendSpecDto, IReadOnlyDictionary&lt;string,string&gt;, PriorityTier, BackendStatus, string, Nullable&lt;DateTime&gt;, Nullable&lt;long&gt;, Nullable&lt;long&gt;\)](BackendInfoDto(BackendSpecDto,IReadOnlyDictionary_string,string_,PriorityTier,BackendStatus,string,Nullable_DateTime_,Nullable_long_,Nullable_long_).md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.BackendInfoDto\(Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, Pmmux\.Abstractions\.PriorityTier, Pmmux\.Abstractions\.BackendStatus, string, System\.Nullable\<System\.DateTime\>, System\.Nullable\<long\>, System\.Nullable\<long\>\)') | A DTO object representing backend runtime information\. |

| Properties | |
| :--- | :--- |
| [HealthCheckFailureCount](HealthCheckFailureCount.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.HealthCheckFailureCount') | The number of consecutive health check failures\. |
| [HealthCheckSuccessCount](HealthCheckSuccessCount.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.HealthCheckSuccessCount') | The number of consecutive health check successes\. |
| [LastHealthCheck](LastHealthCheck.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.LastHealthCheck') | The timestamp of the last health check\. |
| [PriorityTier](PriorityTier.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.PriorityTier') | The routing priority tier\. |
| [Properties](Properties.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.Properties') | Runtime properties provided by the backend implementation\. |
| [Spec](Spec.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.Spec') | The specification that defines the backend\. |
| [Status](Status.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.Status') | The backend status\. |
| [StatusReason](StatusReason.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.StatusReason') | The reason for the current status\. |

| Methods | |
| :--- | :--- |
| [FromBackendInfo\(BackendInfo\)](FromBackendInfo(BackendInfo).md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.FromBackendInfo\(Pmmux\.Abstractions\.BackendInfo\)') | Create a DTO from a [BackendInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')\. |
| [FromBackendStatusInfo\(BackendStatusInfo\)](FromBackendStatusInfo(BackendStatusInfo).md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto\.FromBackendStatusInfo\(Pmmux\.Abstractions\.BackendStatusInfo\)') | Create a DTO from a [BackendStatusInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendStatusInfo/index.md 'Pmmux\.Abstractions\.BackendStatusInfo')\. |
