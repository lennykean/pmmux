#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PortWarden](index.md 'Pmmux\.Core\.PortWarden')

## PortWarden\(IEventSender, IMetricReporter, PortWardenConfig, ILoggerFactory\) Constructor

Default implementation of [IPortWarden](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortWarden/index.md 'Pmmux\.Abstractions\.IPortWarden')\.

```csharp
public PortWarden(Pmmux.Abstractions.IEventSender eventSender, Pmmux.Abstractions.IMetricReporter metricReporter, Pmmux.Core.Configuration.PortWardenConfig config, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory);
```
#### Parameters

<a name='Pmmux.Core.PortWarden.PortWarden(Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.PortWardenConfig,Microsoft.Extensions.Logging.ILoggerFactory).eventSender'></a>

`eventSender` [IEventSender](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IEventSender/index.md 'Pmmux\.Abstractions\.IEventSender')

The event sender service\.

<a name='Pmmux.Core.PortWarden.PortWarden(Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.PortWardenConfig,Microsoft.Extensions.Logging.ILoggerFactory).metricReporter'></a>

`metricReporter` [IMetricReporter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter')

The metric reporter service\.

<a name='Pmmux.Core.PortWarden.PortWarden(Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.PortWardenConfig,Microsoft.Extensions.Logging.ILoggerFactory).config'></a>

`config` [PortWardenConfig](../Configuration/PortWardenConfig/index.md 'Pmmux\.Core\.Configuration\.PortWardenConfig')

The port warden configuration\.

<a name='Pmmux.Core.PortWarden.PortWarden(Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.PortWardenConfig,Microsoft.Extensions.Logging.ILoggerFactory).loggerFactory'></a>

`loggerFactory` [Microsoft\.Extensions\.Logging\.ILoggerFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory 'Microsoft\.Extensions\.Logging\.ILoggerFactory')

The logger factory\.