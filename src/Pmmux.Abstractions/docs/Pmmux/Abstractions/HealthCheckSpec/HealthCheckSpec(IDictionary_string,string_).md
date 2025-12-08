#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[HealthCheckSpec](index.md 'Pmmux\.Abstractions\.HealthCheckSpec')

## HealthCheckSpec\(IDictionary\<string,string\>\) Constructor

```csharp
public HealthCheckSpec(System.Collections.Generic.IDictionary<string,string> parameters);
```
#### Parameters

<a name='Pmmux.Abstractions.HealthCheckSpec.HealthCheckSpec(System.Collections.Generic.IDictionary_string,string_).parameters'></a>

`parameters` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

Backend\-specific health check parameters\.

### Remarks
Parameters will be converted to a value based dictionary to support value\-based equality\.