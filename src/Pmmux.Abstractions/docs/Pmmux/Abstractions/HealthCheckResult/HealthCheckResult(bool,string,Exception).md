#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[HealthCheckResult](index.md 'Pmmux\.Abstractions\.HealthCheckResult')

## HealthCheckResult\(bool, string, Exception\) Constructor

Result of a health check operation\.

```csharp
public HealthCheckResult(bool IsSuccess, string? Reason=null, System.Exception? Exception=null);
```
#### Parameters

<a name='Pmmux.Abstractions.HealthCheckResult.HealthCheckResult(bool,string,System.Exception).IsSuccess'></a>

`IsSuccess` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` if the health check succeeded; otherwise, `false`\.

<a name='Pmmux.Abstractions.HealthCheckResult.HealthCheckResult(bool,string,System.Exception).Reason'></a>

`Reason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Optional human\-readable reason describing the result\.

<a name='Pmmux.Abstractions.HealthCheckResult.HealthCheckResult(bool,string,System.Exception).Exception'></a>

`Exception` [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

Optional exception if the health check failed due to an error\.