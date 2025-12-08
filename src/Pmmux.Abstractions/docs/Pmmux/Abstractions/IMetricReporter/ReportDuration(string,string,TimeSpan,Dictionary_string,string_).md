#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IMetricReporter](index.md 'Pmmux\.Abstractions\.IMetricReporter')

## IMetricReporter\.ReportDuration\(string, string, TimeSpan, Dictionary\<string,string\>\) Method

Report duration metric with a specified time span\.

```csharp
void ReportDuration(string name, string category, System.TimeSpan value, System.Collections.Generic.Dictionary<string,string?> metadata);
```
#### Parameters

<a name='Pmmux.Abstractions.IMetricReporter.ReportDuration(string,string,System.TimeSpan,System.Collections.Generic.Dictionary_string,string_).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric name\.

<a name='Pmmux.Abstractions.IMetricReporter.ReportDuration(string,string,System.TimeSpan,System.Collections.Generic.Dictionary_string,string_).category'></a>

`category` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric category for grouping\.

<a name='Pmmux.Abstractions.IMetricReporter.ReportDuration(string,string,System.TimeSpan,System.Collections.Generic.Dictionary_string,string_).value'></a>

`value` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The duration to report\.

<a name='Pmmux.Abstractions.IMetricReporter.ReportDuration(string,string,System.TimeSpan,System.Collections.Generic.Dictionary_string,string_).metadata'></a>

`metadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Additional dimensions for the metric\.