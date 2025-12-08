#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IMetricReporter](index.md 'Pmmux\.Abstractions\.IMetricReporter')

## IMetricReporter\.ReportCounter\(string, string, double, Dictionary\<string,string\>\) Method

Report counter metric with a specified value\.

```csharp
void ReportCounter(string name, string category, double value, System.Collections.Generic.Dictionary<string,string?> metadata);
```
#### Parameters

<a name='Pmmux.Abstractions.IMetricReporter.ReportCounter(string,string,double,System.Collections.Generic.Dictionary_string,string_).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric name\.

<a name='Pmmux.Abstractions.IMetricReporter.ReportCounter(string,string,double,System.Collections.Generic.Dictionary_string,string_).category'></a>

`category` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric category\.

<a name='Pmmux.Abstractions.IMetricReporter.ReportCounter(string,string,double,System.Collections.Generic.Dictionary_string,string_).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The counter value\.

<a name='Pmmux.Abstractions.IMetricReporter.ReportCounter(string,string,double,System.Collections.Generic.Dictionary_string,string_).metadata'></a>

`metadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Additional dimensions for the metric\.