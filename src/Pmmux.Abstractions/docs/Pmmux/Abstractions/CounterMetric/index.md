#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## CounterMetric Class

Counter metric that tracks cumulative values\.

```csharp
public record CounterMetric : Pmmux.Abstractions.Metric<double>, System.IEquatable<Pmmux.Abstractions.CounterMetric>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; [Metric](../Metric/index.md 'Pmmux\.Abstractions\.Metric') &#129106; [Pmmux\.Abstractions\.Metric&lt;](../Metric_T_/index.md 'Pmmux\.Abstractions\.Metric\<T\>')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](../Metric_T_/index.md 'Pmmux\.Abstractions\.Metric\<T\>') &#129106; CounterMetric

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[CounterMetric](index.md 'Pmmux\.Abstractions\.CounterMetric')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [CounterMetric\(string, string, Dictionary&lt;string,string&gt;, double\)](CounterMetric(string,string,Dictionary_string,string_,double).md 'Pmmux\.Abstractions\.CounterMetric\.CounterMetric\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, double\)') | Counter metric that tracks cumulative values\. |
