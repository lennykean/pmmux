#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[HealthCheckResult](index.md 'Pmmux\.Abstractions\.HealthCheckResult')

## HealthCheckResult\.Unhealthy\(string, Exception\) Method

Create a failed health check result\.

```csharp
public static Pmmux.Abstractions.HealthCheckResult Unhealthy(string reason, System.Exception? exception=null);
```
#### Parameters

<a name='Pmmux.Abstractions.HealthCheckResult.Unhealthy(string,System.Exception).reason'></a>

`reason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The description of why the check failed\.

<a name='Pmmux.Abstractions.HealthCheckResult.Unhealthy(string,System.Exception).exception'></a>

`exception` [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

Optional exception that caused the failure\.

#### Returns
[HealthCheckResult](index.md 'Pmmux\.Abstractions\.HealthCheckResult')  
A [HealthCheckResult](index.md 'Pmmux\.Abstractions\.HealthCheckResult') indicating failure\.