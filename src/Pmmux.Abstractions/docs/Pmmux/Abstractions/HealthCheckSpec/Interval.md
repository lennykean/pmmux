#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[HealthCheckSpec](index.md 'Pmmux\.Abstractions\.HealthCheckSpec')

## HealthCheckSpec\.Interval Property

Interval between health checks\.

```csharp
public System.TimeSpan Interval { get; init; }
```

#### Property Value
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

### Remarks
Defaults to 10 seconds\. Health checks run continuously at this interval while the backend is monitored\.