#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IMetricSink Interface

Receives metrics from the [IMetricReporter](../IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter')\.

```csharp
public interface IMetricSink
```

### Remarks
Metric sinks process metrics emitted by the multiplexer and extensions\.
Common implementations include logging sinks, time\-series database exporters,
and monitoring system integrations\. Multiple sinks can be registered\.

| Methods | |
| :--- | :--- |
| [ReceiveMetricAsync\(Metric, DateTimeOffset\)](ReceiveMetricAsync(Metric,DateTimeOffset).md 'Pmmux\.Abstractions\.IMetricSink\.ReceiveMetricAsync\(Pmmux\.Abstractions\.Metric, System\.DateTimeOffset\)') | Receive and process a metric asynchronously\. |
