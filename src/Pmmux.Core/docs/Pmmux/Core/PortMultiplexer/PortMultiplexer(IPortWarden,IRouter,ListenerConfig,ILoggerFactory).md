#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PortMultiplexer](index.md 'Pmmux\.Core\.PortMultiplexer')

## PortMultiplexer\(IPortWarden, IRouter, ListenerConfig, ILoggerFactory\) Constructor

Default implementation of [IPortMultiplexer](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortMultiplexer/index.md 'Pmmux\.Abstractions\.IPortMultiplexer')\.

```csharp
public PortMultiplexer(Pmmux.Abstractions.IPortWarden portWarden, Pmmux.Abstractions.IRouter router, Pmmux.Core.Configuration.ListenerConfig config, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory);
```
#### Parameters

<a name='Pmmux.Core.PortMultiplexer.PortMultiplexer(Pmmux.Abstractions.IPortWarden,Pmmux.Abstractions.IRouter,Pmmux.Core.Configuration.ListenerConfig,Microsoft.Extensions.Logging.ILoggerFactory).portWarden'></a>

`portWarden` [IPortWarden](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortWarden/index.md 'Pmmux\.Abstractions\.IPortWarden')

The port warden service\.

<a name='Pmmux.Core.PortMultiplexer.PortMultiplexer(Pmmux.Abstractions.IPortWarden,Pmmux.Abstractions.IRouter,Pmmux.Core.Configuration.ListenerConfig,Microsoft.Extensions.Logging.ILoggerFactory).router'></a>

`router` [IRouter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/index.md 'Pmmux\.Abstractions\.IRouter')

The router service\.

<a name='Pmmux.Core.PortMultiplexer.PortMultiplexer(Pmmux.Abstractions.IPortWarden,Pmmux.Abstractions.IRouter,Pmmux.Core.Configuration.ListenerConfig,Microsoft.Extensions.Logging.ILoggerFactory).config'></a>

`config` [ListenerConfig](../Configuration/ListenerConfig/index.md 'Pmmux\.Core\.Configuration\.ListenerConfig')

The listener configuration\.

<a name='Pmmux.Core.PortMultiplexer.PortMultiplexer(Pmmux.Abstractions.IPortWarden,Pmmux.Abstractions.IRouter,Pmmux.Core.Configuration.ListenerConfig,Microsoft.Extensions.Logging.ILoggerFactory).loggerFactory'></a>

`loggerFactory` [Microsoft\.Extensions\.Logging\.ILoggerFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory 'Microsoft\.Extensions\.Logging\.ILoggerFactory')

The logger factory\.