#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IMetricSink](index.md 'Pmmux\.Abstractions\.IMetricSink')

## IMetricSink\.ReceiveMetricAsync\(Metric, DateTimeOffset\) Method

Receive and process a metric asynchronously\.

```csharp
System.Threading.Tasks.Task ReceiveMetricAsync(Pmmux.Abstractions.Metric metric, System.DateTimeOffset captured);
```
#### Parameters

<a name='Pmmux.Abstractions.IMetricSink.ReceiveMetricAsync(Pmmux.Abstractions.Metric,System.DateTimeOffset).metric'></a>

`metric` [Metric](../Metric/index.md 'Pmmux\.Abstractions\.Metric')

The metric to process\. May be a [CounterMetric](../CounterMetric/index.md 'Pmmux\.Abstractions\.CounterMetric'), [GaugeMetric](../GaugeMetric/index.md 'Pmmux\.Abstractions\.GaugeMetric'),
[DurationMetric](../DurationMetric/index.md 'Pmmux\.Abstractions\.DurationMetric'), or other metric type\.

<a name='Pmmux.Abstractions.IMetricSink.ReceiveMetricAsync(Pmmux.Abstractions.Metric,System.DateTimeOffset).captured'></a>

`captured` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

The timestamp when the metric was captured\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the asynchronous processing operation\.