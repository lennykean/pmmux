#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## MetricReporter Class

Default implementation of [IMetricReporter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter') that dispatches metrics to sinks\.

```csharp
public sealed class MetricReporter : Pmmux.Abstractions.IMetricReporter, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; MetricReporter

Implements [IMetricReporter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Constructors | |
| :--- | :--- |
| [MetricReporter\(IEnumerable&lt;IMetricSink&gt;, ILoggerFactory\)](MetricReporter(IEnumerable_IMetricSink_,ILoggerFactory).md 'Pmmux\.Core\.MetricReporter\.MetricReporter\(System\.Collections\.Generic\.IEnumerable\<Pmmux\.Abstractions\.IMetricSink\>, Microsoft\.Extensions\.Logging\.ILoggerFactory\)') | Default implementation of [IMetricReporter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter') that dispatches metrics to sinks\. |
