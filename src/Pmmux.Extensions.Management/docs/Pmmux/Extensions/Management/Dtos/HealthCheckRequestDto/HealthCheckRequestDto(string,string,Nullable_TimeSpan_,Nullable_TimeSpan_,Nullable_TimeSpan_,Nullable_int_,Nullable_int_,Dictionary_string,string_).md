#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos').[HealthCheckRequestDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.HealthCheckRequestDto')

## HealthCheckRequestDto\(string, string, Nullable\<TimeSpan\>, Nullable\<TimeSpan\>, Nullable\<TimeSpan\>, Nullable\<int\>, Nullable\<int\>, Dictionary\<string,string\>\) Constructor

A DTO for health check requests\.

```csharp
public HealthCheckRequestDto(string? ProtocolName, string? BackendName, System.Nullable<System.TimeSpan> InitialDelay, System.Nullable<System.TimeSpan> Interval, System.Nullable<System.TimeSpan> Timeout, System.Nullable<int> FailureThreshold, System.Nullable<int> RecoveryThreshold, System.Collections.Generic.Dictionary<string,string>? Parameters);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto.HealthCheckRequestDto(string,string,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_int_,System.Nullable_int_,System.Collections.Generic.Dictionary_string,string_).ProtocolName'></a>

`ProtocolName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto.HealthCheckRequestDto(string,string,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_int_,System.Nullable_int_,System.Collections.Generic.Dictionary_string,string_).BackendName'></a>

`BackendName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto.HealthCheckRequestDto(string,string,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_int_,System.Nullable_int_,System.Collections.Generic.Dictionary_string,string_).InitialDelay'></a>

`InitialDelay` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto.HealthCheckRequestDto(string,string,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_int_,System.Nullable_int_,System.Collections.Generic.Dictionary_string,string_).Interval'></a>

`Interval` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto.HealthCheckRequestDto(string,string,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_int_,System.Nullable_int_,System.Collections.Generic.Dictionary_string,string_).Timeout'></a>

`Timeout` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto.HealthCheckRequestDto(string,string,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_int_,System.Nullable_int_,System.Collections.Generic.Dictionary_string,string_).FailureThreshold'></a>

`FailureThreshold` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto.HealthCheckRequestDto(string,string,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_int_,System.Nullable_int_,System.Collections.Generic.Dictionary_string,string_).RecoveryThreshold'></a>

`RecoveryThreshold` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto.HealthCheckRequestDto(string,string,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_System.TimeSpan_,System.Nullable_int_,System.Nullable_int_,System.Collections.Generic.Dictionary_string,string_).Parameters'></a>

`Parameters` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')