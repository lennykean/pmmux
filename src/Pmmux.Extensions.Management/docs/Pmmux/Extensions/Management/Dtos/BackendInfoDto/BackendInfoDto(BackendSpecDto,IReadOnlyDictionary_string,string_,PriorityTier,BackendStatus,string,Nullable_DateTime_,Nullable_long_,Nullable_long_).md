#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos').[BackendInfoDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendInfoDto')

## BackendInfoDto\(BackendSpecDto, IReadOnlyDictionary\<string,string\>, PriorityTier, BackendStatus, string, Nullable\<DateTime\>, Nullable\<long\>, Nullable\<long\>\) Constructor

A DTO object representing backend runtime information\.

```csharp
public BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto Spec, System.Collections.Generic.IReadOnlyDictionary<string,string> Properties, Pmmux.Abstractions.PriorityTier PriorityTier, Pmmux.Abstractions.BackendStatus Status, string? StatusReason, System.Nullable<System.DateTime> LastHealthCheck, System.Nullable<long> HealthCheckFailureCount, System.Nullable<long> HealthCheckSuccessCount);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Dtos.BackendInfoDto.BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto,System.Collections.Generic.IReadOnlyDictionary_string,string_,Pmmux.Abstractions.PriorityTier,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).Spec'></a>

`Spec` [BackendSpecDto](../BackendSpecDto/index.md 'Pmmux\.Extensions\.Management\.Dtos\.BackendSpecDto')

The specification that defines the backend\.

<a name='Pmmux.Extensions.Management.Dtos.BackendInfoDto.BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto,System.Collections.Generic.IReadOnlyDictionary_string,string_,Pmmux.Abstractions.PriorityTier,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).Properties'></a>

`Properties` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

Runtime properties provided by the backend implementation\.

<a name='Pmmux.Extensions.Management.Dtos.BackendInfoDto.BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto,System.Collections.Generic.IReadOnlyDictionary_string,string_,Pmmux.Abstractions.PriorityTier,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).PriorityTier'></a>

`PriorityTier` [PriorityTier](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/PriorityTier/index.md 'Pmmux\.Abstractions\.PriorityTier')

The routing priority tier\.

<a name='Pmmux.Extensions.Management.Dtos.BackendInfoDto.BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto,System.Collections.Generic.IReadOnlyDictionary_string,string_,Pmmux.Abstractions.PriorityTier,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).Status'></a>

`Status` [BackendStatus](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendStatus/index.md 'Pmmux\.Abstractions\.BackendStatus')

The backend status\.

<a name='Pmmux.Extensions.Management.Dtos.BackendInfoDto.BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto,System.Collections.Generic.IReadOnlyDictionary_string,string_,Pmmux.Abstractions.PriorityTier,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).StatusReason'></a>

`StatusReason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason for the current status\.

<a name='Pmmux.Extensions.Management.Dtos.BackendInfoDto.BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto,System.Collections.Generic.IReadOnlyDictionary_string,string_,Pmmux.Abstractions.PriorityTier,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).LastHealthCheck'></a>

`LastHealthCheck` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The timestamp of the last health check\.

<a name='Pmmux.Extensions.Management.Dtos.BackendInfoDto.BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto,System.Collections.Generic.IReadOnlyDictionary_string,string_,Pmmux.Abstractions.PriorityTier,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).HealthCheckFailureCount'></a>

`HealthCheckFailureCount` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The number of consecutive health check failures\.

<a name='Pmmux.Extensions.Management.Dtos.BackendInfoDto.BackendInfoDto(Pmmux.Extensions.Management.Dtos.BackendSpecDto,System.Collections.Generic.IReadOnlyDictionary_string,string_,Pmmux.Abstractions.PriorityTier,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).HealthCheckSuccessCount'></a>

`HealthCheckSuccessCount` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The number of consecutive health check successes\.