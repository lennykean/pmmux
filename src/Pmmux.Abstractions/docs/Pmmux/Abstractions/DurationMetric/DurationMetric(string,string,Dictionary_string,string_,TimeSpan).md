#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[DurationMetric](index.md 'Pmmux\.Abstractions\.DurationMetric')

## DurationMetric\(string, string, Dictionary\<string,string\>, TimeSpan\) Constructor

Duration metric that tracks time\-based measurements\.

```csharp
public DurationMetric(string Name, string Category, System.Collections.Generic.Dictionary<string,string?> Metadata, System.TimeSpan Value);
```
#### Parameters

<a name='Pmmux.Abstractions.DurationMetric.DurationMetric(string,string,System.Collections.Generic.Dictionary_string,string_,System.TimeSpan).Name'></a>

`Name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric name\.

<a name='Pmmux.Abstractions.DurationMetric.DurationMetric(string,string,System.Collections.Generic.Dictionary_string,string_,System.TimeSpan).Category'></a>

`Category` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric category\.

<a name='Pmmux.Abstractions.DurationMetric.DurationMetric(string,string,System.Collections.Generic.Dictionary_string,string_,System.TimeSpan).Metadata'></a>

`Metadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Additional dimensions and context\.

<a name='Pmmux.Abstractions.DurationMetric.DurationMetric(string,string,System.Collections.Generic.Dictionary_string,string_,System.TimeSpan).Value'></a>

`Value` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The duration measured\.