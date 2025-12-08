#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[BackendStatusInfo](index.md 'Pmmux\.Abstractions\.BackendStatusInfo')

## BackendStatusInfo\(BackendInfo, BackendStatus, string, Nullable\<DateTime\>, Nullable\<long\>, Nullable\<long\>\) Constructor

Health status and statistics for a backend\.

```csharp
public BackendStatusInfo(Pmmux.Abstractions.BackendInfo Backend, Pmmux.Abstractions.BackendStatus Status, string? StatusReason=null, System.Nullable<System.DateTime> LastHealthCheck=null, System.Nullable<long> HealthCheckFailureCount=null, System.Nullable<long> HealthCheckSuccessCount=null);
```
#### Parameters

<a name='Pmmux.Abstractions.BackendStatusInfo.BackendStatusInfo(Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).Backend'></a>

`Backend` [BackendInfo](../BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')

The backend that the status information pertains to\.

<a name='Pmmux.Abstractions.BackendStatusInfo.BackendStatusInfo(Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).Status'></a>

`Status` [BackendStatus](../BackendStatus/index.md 'Pmmux\.Abstractions\.BackendStatus')

The current health status\.

<a name='Pmmux.Abstractions.BackendStatusInfo.BackendStatusInfo(Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).StatusReason'></a>

`StatusReason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Optional reason for the current status\.

<a name='Pmmux.Abstractions.BackendStatusInfo.BackendStatusInfo(Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).LastHealthCheck'></a>

`LastHealthCheck` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The timestamp of the most recent health check if one has been performed\.

<a name='Pmmux.Abstractions.BackendStatusInfo.BackendStatusInfo(Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).HealthCheckFailureCount'></a>

`HealthCheckFailureCount` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The number of most recent successive failed health checks\.

<a name='Pmmux.Abstractions.BackendStatusInfo.BackendStatusInfo(Pmmux.Abstractions.BackendInfo,Pmmux.Abstractions.BackendStatus,string,System.Nullable_System.DateTime_,System.Nullable_long_,System.Nullable_long_).HealthCheckSuccessCount'></a>

`HealthCheckSuccessCount` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The number of most recent successive successful health checks\.