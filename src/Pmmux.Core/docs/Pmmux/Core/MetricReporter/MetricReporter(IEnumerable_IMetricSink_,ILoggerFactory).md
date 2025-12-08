#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[MetricReporter](index.md 'Pmmux\.Core\.MetricReporter')

## MetricReporter\(IEnumerable\<IMetricSink\>, ILoggerFactory\) Constructor

Default implementation of [IMetricReporter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter') that dispatches metrics to sinks\.

```csharp
public MetricReporter(System.Collections.Generic.IEnumerable<Pmmux.Abstractions.IMetricSink> sinks, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory);
```
#### Parameters

<a name='Pmmux.Core.MetricReporter.MetricReporter(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IMetricSink_,Microsoft.Extensions.Logging.ILoggerFactory).sinks'></a>

`sinks` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[IMetricSink](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IMetricSink/index.md 'Pmmux\.Abstractions\.IMetricSink')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of metric sinks to dispatch metrics to\.

<a name='Pmmux.Core.MetricReporter.MetricReporter(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IMetricSink_,Microsoft.Extensions.Logging.ILoggerFactory).loggerFactory'></a>

`loggerFactory` [Microsoft\.Extensions\.Logging\.ILoggerFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory 'Microsoft\.Extensions\.Logging\.ILoggerFactory')

The logger factory\.