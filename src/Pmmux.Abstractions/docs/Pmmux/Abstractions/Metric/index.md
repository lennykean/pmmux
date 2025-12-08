#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## Metric Class

Base class for all metric types\.

```csharp
public abstract record Metric : System.IEquatable<Pmmux.Abstractions.Metric>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; Metric

Derived  
&#8627; [Metric&lt;T&gt;](../Metric_T_/index.md 'Pmmux\.Abstractions\.Metric\<T\>')

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[Metric](index.md 'Pmmux\.Abstractions\.Metric')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Properties | |
| :--- | :--- |
| [Category](Category.md 'Pmmux\.Abstractions\.Metric\.Category') | The category for grouping related metrics\. |
| [Metadata](Metadata.md 'Pmmux\.Abstractions\.Metric\.Metadata') | Additional dimensions and context for the metric\. |
| [Name](Name.md 'Pmmux\.Abstractions\.Metric\.Name') | The metric name identifying what is being measured\. |

| Methods | |
| :--- | :--- |
| [StringValue\(\)](StringValue().md 'Pmmux\.Abstractions\.Metric\.StringValue\(\)') | Get string representation of the metric's value\. |
