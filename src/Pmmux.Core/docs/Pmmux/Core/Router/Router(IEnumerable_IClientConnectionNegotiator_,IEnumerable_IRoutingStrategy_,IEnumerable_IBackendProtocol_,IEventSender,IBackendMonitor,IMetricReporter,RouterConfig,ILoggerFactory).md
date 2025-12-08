#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[Router](index.md 'Pmmux\.Core\.Router')

## Router\(IEnumerable\<IClientConnectionNegotiator\>, IEnumerable\<IRoutingStrategy\>, IEnumerable\<IBackendProtocol\>, IEventSender, IBackendMonitor, IMetricReporter, RouterConfig, ILoggerFactory\) Constructor

Default implementation of [IRouter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/index.md 'Pmmux\.Abstractions\.IRouter')\.

```csharp
public Router(System.Collections.Generic.IEnumerable<Pmmux.Abstractions.IClientConnectionNegotiator> connectionNegotiators, System.Collections.Generic.IEnumerable<Pmmux.Abstractions.IRoutingStrategy> routingStrategies, System.Collections.Generic.IEnumerable<Pmmux.Abstractions.IBackendProtocol> protocols, Pmmux.Abstractions.IEventSender eventSender, Pmmux.Abstractions.IBackendMonitor backendMonitor, Pmmux.Abstractions.IMetricReporter metricReporter, Pmmux.Core.Configuration.RouterConfig config, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory);
```
#### Parameters

<a name='Pmmux.Core.Router.Router(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IClientConnectionNegotiator_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IRoutingStrategy_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IBackendProtocol_,Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IBackendMonitor,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory).connectionNegotiators'></a>

`connectionNegotiators` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[IClientConnectionNegotiator](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IClientConnectionNegotiator/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of loaded client connection negotiators\.

<a name='Pmmux.Core.Router.Router(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IClientConnectionNegotiator_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IRoutingStrategy_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IBackendProtocol_,Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IBackendMonitor,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory).routingStrategies'></a>

`routingStrategies` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[IRoutingStrategy](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRoutingStrategy/index.md 'Pmmux\.Abstractions\.IRoutingStrategy')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of loaded routing strategies\.

<a name='Pmmux.Core.Router.Router(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IClientConnectionNegotiator_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IRoutingStrategy_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IBackendProtocol_,Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IBackendMonitor,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory).protocols'></a>

`protocols` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[IBackendProtocol](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendProtocol/index.md 'Pmmux\.Abstractions\.IBackendProtocol')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of loaded backend protocols\.

<a name='Pmmux.Core.Router.Router(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IClientConnectionNegotiator_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IRoutingStrategy_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IBackendProtocol_,Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IBackendMonitor,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory).eventSender'></a>

`eventSender` [IEventSender](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IEventSender/index.md 'Pmmux\.Abstractions\.IEventSender')

The event sender service\.

<a name='Pmmux.Core.Router.Router(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IClientConnectionNegotiator_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IRoutingStrategy_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IBackendProtocol_,Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IBackendMonitor,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory).backendMonitor'></a>

`backendMonitor` [IBackendMonitor](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor')

The backend monitor service\.

<a name='Pmmux.Core.Router.Router(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IClientConnectionNegotiator_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IRoutingStrategy_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IBackendProtocol_,Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IBackendMonitor,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory).metricReporter'></a>

`metricReporter` [IMetricReporter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter')

The metric reporter service\.

<a name='Pmmux.Core.Router.Router(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IClientConnectionNegotiator_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IRoutingStrategy_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IBackendProtocol_,Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IBackendMonitor,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory).config'></a>

`config` [RouterConfig](../Configuration/RouterConfig/index.md 'Pmmux\.Core\.Configuration\.RouterConfig')

The router configuration\.

<a name='Pmmux.Core.Router.Router(System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IClientConnectionNegotiator_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IRoutingStrategy_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IBackendProtocol_,Pmmux.Abstractions.IEventSender,Pmmux.Abstractions.IBackendMonitor,Pmmux.Abstractions.IMetricReporter,Pmmux.Core.Configuration.RouterConfig,Microsoft.Extensions.Logging.ILoggerFactory).loggerFactory'></a>

`loggerFactory` [Microsoft\.Extensions\.Logging\.ILoggerFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory 'Microsoft\.Extensions\.Logging\.ILoggerFactory')

The logger factory\.