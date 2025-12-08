#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[BackendMonitor](index.md 'Pmmux\.Core\.BackendMonitor')

## BackendMonitor\(RouterConfig, ILoggerFactory, IMetricReporter, IEventSender\) Constructor

Default implementation of [IBackendMonitor](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor')\.

```csharp
public BackendMonitor(Pmmux.Core.Configuration.RouterConfig routerConfig, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, Pmmux.Abstractions.IMetricReporter metricReporter, Pmmux.Abstractions.IEventSender eventSender);
```
#### Parameters

<a name='Pmmux.Core.BackendMonitor.BackendMonitor(Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory,Pmmux.Abstractions.IMetricReporter,Pmmux.Abstractions.IEventSender).routerConfig'></a>

`routerConfig` [RouterConfig](../Configuration/RouterConfig/index.md 'Pmmux\.Core\.Configuration\.RouterConfig')

The router configuration\.

<a name='Pmmux.Core.BackendMonitor.BackendMonitor(Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory,Pmmux.Abstractions.IMetricReporter,Pmmux.Abstractions.IEventSender).loggerFactory'></a>

`loggerFactory` [Microsoft\.Extensions\.Logging\.ILoggerFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory 'Microsoft\.Extensions\.Logging\.ILoggerFactory')

The logger factory\.

<a name='Pmmux.Core.BackendMonitor.BackendMonitor(Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory,Pmmux.Abstractions.IMetricReporter,Pmmux.Abstractions.IEventSender).metricReporter'></a>

`metricReporter` [IMetricReporter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter')

The metric reporter service\.

<a name='Pmmux.Core.BackendMonitor.BackendMonitor(Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory,Pmmux.Abstractions.IMetricReporter,Pmmux.Abstractions.IEventSender).eventSender'></a>

`eventSender` [IEventSender](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IEventSender/index.md 'Pmmux\.Abstractions\.IEventSender')

The event sender service\.