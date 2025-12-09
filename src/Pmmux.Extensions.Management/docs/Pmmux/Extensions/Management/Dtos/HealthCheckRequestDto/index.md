#### [Pmmux\.Extensions\.Management](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Dtos](../index.md 'Pmmux\.Extensions\.Management\.Dtos')

## HealthCheckRequestDto Class

A DTO for health check requests\.

```csharp
public record HealthCheckRequestDto : System.IEquatable<Pmmux.Extensions.Management.Dtos.HealthCheckRequestDto>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; HealthCheckRequestDto

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[HealthCheckRequestDto](index.md 'Pmmux\.Extensions\.Management\.Dtos\.HealthCheckRequestDto')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [HealthCheckRequestDto\(string, string, Nullable&lt;TimeSpan&gt;, Nullable&lt;TimeSpan&gt;, Nullable&lt;TimeSpan&gt;, Nullable&lt;int&gt;, Nullable&lt;int&gt;, Dictionary&lt;string,string&gt;\)](HealthCheckRequestDto(string,string,Nullable_TimeSpan_,Nullable_TimeSpan_,Nullable_TimeSpan_,Nullable_int_,Nullable_int_,Dictionary_string,string_).md 'Pmmux\.Extensions\.Management\.Dtos\.HealthCheckRequestDto\.HealthCheckRequestDto\(string, string, System\.Nullable\<System\.TimeSpan\>, System\.Nullable\<System\.TimeSpan\>, System\.Nullable\<System\.TimeSpan\>, System\.Nullable\<int\>, System\.Nullable\<int\>, System\.Collections\.Generic\.Dictionary\<string,string\>\)') | A DTO for health check requests\. |

| Methods | |
| :--- | :--- |
| [ToHealthCheckSpec\(\)](ToHealthCheckSpec().md 'Pmmux\.Extensions\.Management\.Dtos\.HealthCheckRequestDto\.ToHealthCheckSpec\(\)') | Convert to a [HealthCheckSpec](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/HealthCheckSpec/index.md 'Pmmux\.Abstractions\.HealthCheckSpec')\. |
