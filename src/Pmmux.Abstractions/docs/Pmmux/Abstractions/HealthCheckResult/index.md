#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## HealthCheckResult Class

Result of a health check operation\.

```csharp
public record HealthCheckResult : System.IEquatable<Pmmux.Abstractions.HealthCheckResult>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; HealthCheckResult

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[HealthCheckResult](index.md 'Pmmux\.Abstractions\.HealthCheckResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [HealthCheckResult\(bool, string, Exception\)](HealthCheckResult(bool,string,Exception).md 'Pmmux\.Abstractions\.HealthCheckResult\.HealthCheckResult\(bool, string, System\.Exception\)') | Result of a health check operation\. |

| Properties | |
| :--- | :--- |
| [Exception](Exception.md 'Pmmux\.Abstractions\.HealthCheckResult\.Exception') | Optional exception if the health check failed due to an error\. |
| [IsSuccess](IsSuccess.md 'Pmmux\.Abstractions\.HealthCheckResult\.IsSuccess') | `true` if the health check succeeded; otherwise, `false`\. |
| [Reason](Reason.md 'Pmmux\.Abstractions\.HealthCheckResult\.Reason') | Optional human\-readable reason describing the result\. |

| Methods | |
| :--- | :--- |
| [Healthy\(string\)](Healthy(string).md 'Pmmux\.Abstractions\.HealthCheckResult\.Healthy\(string\)') | Create a successful health check result\. |
| [Unhealthy\(string, Exception\)](Unhealthy(string,Exception).md 'Pmmux\.Abstractions\.HealthCheckResult\.Unhealthy\(string, System\.Exception\)') | Create a failed health check result\. |
