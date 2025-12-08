#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[HealthCheckSpec](index.md 'Pmmux\.Abstractions\.HealthCheckSpec')

## HealthCheckSpec\.RecoveryThreshold Property

Number of consecutive successes required to mark an unhealthy backend as healthy again\.

```csharp
public int RecoveryThreshold { get; init; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Remarks
Defaults to 5\.