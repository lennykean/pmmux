#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## DurationMetric Class

Duration metric that tracks time\-based measurements\.

```csharp
public record DurationMetric : Pmmux.Abstractions.Metric<System.TimeSpan>, System.IEquatable<Pmmux.Abstractions.DurationMetric>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; [Metric](../Metric/index.md 'Pmmux\.Abstractions\.Metric') &#129106; [Pmmux\.Abstractions\.Metric&lt;](../Metric_T_/index.md 'Pmmux\.Abstractions\.Metric\<T\>')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[&gt;](../Metric_T_/index.md 'Pmmux\.Abstractions\.Metric\<T\>') &#129106; DurationMetric

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[DurationMetric](index.md 'Pmmux\.Abstractions\.DurationMetric')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [DurationMetric\(string, string, Dictionary&lt;string,string&gt;, TimeSpan\)](DurationMetric(string,string,Dictionary_string,string_,TimeSpan).md 'Pmmux\.Abstractions\.DurationMetric\.DurationMetric\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.TimeSpan\)') | Duration metric that tracks time\-based measurements\. |
