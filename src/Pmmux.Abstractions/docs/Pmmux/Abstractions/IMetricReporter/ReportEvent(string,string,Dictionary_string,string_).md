#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IMetricReporter](index.md 'Pmmux\.Abstractions\.IMetricReporter')

## IMetricReporter\.ReportEvent\(string, string, Dictionary\<string,string\>\) Method

Report a single count of a metric\.

```csharp
void ReportEvent(string name, string category, System.Collections.Generic.Dictionary<string,string?> metadata);
```
#### Parameters

<a name='Pmmux.Abstractions.IMetricReporter.ReportEvent(string,string,System.Collections.Generic.Dictionary_string,string_).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The event name\.

<a name='Pmmux.Abstractions.IMetricReporter.ReportEvent(string,string,System.Collections.Generic.Dictionary_string,string_).category'></a>

`category` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The event category for grouping\.

<a name='Pmmux.Abstractions.IMetricReporter.ReportEvent(string,string,System.Collections.Generic.Dictionary_string,string_).metadata'></a>

`metadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Additional dimensions for the event\.