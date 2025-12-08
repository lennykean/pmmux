#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[GaugeMetric](index.md 'Pmmux\.Abstractions\.GaugeMetric')

## GaugeMetric\(string, string, Dictionary\<string,string\>, double\) Constructor

Gauge metric that tracks instantaneous values\.

```csharp
public GaugeMetric(string Name, string Category, System.Collections.Generic.Dictionary<string,string?> Metadata, double Value);
```
#### Parameters

<a name='Pmmux.Abstractions.GaugeMetric.GaugeMetric(string,string,System.Collections.Generic.Dictionary_string,string_,double).Name'></a>

`Name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric name\.

<a name='Pmmux.Abstractions.GaugeMetric.GaugeMetric(string,string,System.Collections.Generic.Dictionary_string,string_,double).Category'></a>

`Category` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric category\.

<a name='Pmmux.Abstractions.GaugeMetric.GaugeMetric(string,string,System.Collections.Generic.Dictionary_string,string_,double).Metadata'></a>

`Metadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Additional dimensions and context\.

<a name='Pmmux.Abstractions.GaugeMetric.GaugeMetric(string,string,System.Collections.Generic.Dictionary_string,string_,double).Value'></a>

`Value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The current gauge value\.