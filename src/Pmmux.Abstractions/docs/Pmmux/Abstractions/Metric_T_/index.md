#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## Metric\<T\> Class

Base class for metrics with a specific value type\.

```csharp
public abstract record Metric<T> : Pmmux.Abstractions.Metric, System.IEquatable<Pmmux.Abstractions.Metric<T>>
```
#### Type parameters

<a name='Pmmux.Abstractions.Metric_T_.T'></a>

`T`

Type of the metric value\.

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; [Metric](../Metric/index.md 'Pmmux\.Abstractions\.Metric') &#129106; Metric\<T\>

Derived  
&#8627; [CounterMetric](../CounterMetric/index.md 'Pmmux\.Abstractions\.CounterMetric')  
&#8627; [DurationMetric](../DurationMetric/index.md 'Pmmux\.Abstractions\.DurationMetric')  
&#8627; [GaugeMetric](../GaugeMetric/index.md 'Pmmux\.Abstractions\.GaugeMetric')

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[Pmmux\.Abstractions\.Metric&lt;](index.md 'Pmmux\.Abstractions\.Metric\<T\>')[T](index.md#Pmmux.Abstractions.Metric_T_.T 'Pmmux\.Abstractions\.Metric\<T\>\.T')[&gt;](index.md 'Pmmux\.Abstractions\.Metric\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Properties | |
| :--- | :--- |
| [Value](Value.md 'Pmmux\.Abstractions\.Metric\<T\>\.Value') | The measured value\. |

| Methods | |
| :--- | :--- |
| [StringValue\(\)](StringValue().md 'Pmmux\.Abstractions\.Metric\<T\>\.StringValue\(\)') | Get string representation of the metric's value\. |
