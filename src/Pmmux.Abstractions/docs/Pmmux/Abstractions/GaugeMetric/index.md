#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## GaugeMetric Class

Gauge metric that tracks instantaneous values\.

```csharp
public record GaugeMetric : Pmmux.Abstractions.Metric<double>, System.IEquatable<Pmmux.Abstractions.GaugeMetric>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; [Metric](../Metric/index.md 'Pmmux\.Abstractions\.Metric') &#129106; [Pmmux\.Abstractions\.Metric&lt;](../Metric_T_/index.md 'Pmmux\.Abstractions\.Metric\<T\>')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](../Metric_T_/index.md 'Pmmux\.Abstractions\.Metric\<T\>') &#129106; GaugeMetric

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[GaugeMetric](index.md 'Pmmux\.Abstractions\.GaugeMetric')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [GaugeMetric\(string, string, Dictionary&lt;string,string&gt;, double\)](GaugeMetric(string,string,Dictionary_string,string_,double).md 'Pmmux\.Abstractions\.GaugeMetric\.GaugeMetric\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, double\)') | Gauge metric that tracks instantaneous values\. |
