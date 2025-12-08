#### [Pmmux\.Core](../../../../index.md 'index')
### [Microsoft\.Extensions\.DependencyInjection](../index.md 'Microsoft\.Extensions\.DependencyInjection').[ServiceCollectionExtensions](index.md 'Microsoft\.Extensions\.DependencyInjection\.ServiceCollectionExtensions')

## ServiceCollectionExtensions\.AddPmmux\(this IServiceCollection, HostBuilderContext, Func\<ListenerConfig\>, Func\<PortWardenConfig\>, Func\<RouterConfig\>, IEnumerable\<IExtension\>\) Method

Register core Pmmux services and services from the provided extensions\.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddPmmux(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Hosting.HostBuilderContext hostContext, System.Func<Pmmux.Core.Configuration.ListenerConfig> listenerConfigFactory, System.Func<Pmmux.Core.Configuration.PortWardenConfig> portWardenConfigFactory, System.Func<Pmmux.Core.Configuration.RouterConfig> routerConfigFactory, System.Collections.Generic.IEnumerable<Pmmux.Abstractions.IExtension> extensions);
```
#### Parameters

<a name='Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions.AddPmmux(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Hosting.HostBuilderContext,System.Func_Pmmux.Core.Configuration.ListenerConfig_,System.Func_Pmmux.Core.Configuration.PortWardenConfig_,System.Func_Pmmux.Core.Configuration.RouterConfig_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IExtension_).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The service collection to add services to\.

<a name='Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions.AddPmmux(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Hosting.HostBuilderContext,System.Func_Pmmux.Core.Configuration.ListenerConfig_,System.Func_Pmmux.Core.Configuration.PortWardenConfig_,System.Func_Pmmux.Core.Configuration.RouterConfig_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IExtension_).hostContext'></a>

`hostContext` [Microsoft\.Extensions\.Hosting\.HostBuilderContext](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuildercontext 'Microsoft\.Extensions\.Hosting\.HostBuilderContext')

The host builder context\.

<a name='Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions.AddPmmux(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Hosting.HostBuilderContext,System.Func_Pmmux.Core.Configuration.ListenerConfig_,System.Func_Pmmux.Core.Configuration.PortWardenConfig_,System.Func_Pmmux.Core.Configuration.RouterConfig_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IExtension_).listenerConfigFactory'></a>

`listenerConfigFactory` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[ListenerConfig](../../../../Pmmux/Core/Configuration/ListenerConfig/index.md 'Pmmux\.Core\.Configuration\.ListenerConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

The listener configuration factory\.

<a name='Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions.AddPmmux(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Hosting.HostBuilderContext,System.Func_Pmmux.Core.Configuration.ListenerConfig_,System.Func_Pmmux.Core.Configuration.PortWardenConfig_,System.Func_Pmmux.Core.Configuration.RouterConfig_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IExtension_).portWardenConfigFactory'></a>

`portWardenConfigFactory` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[PortWardenConfig](../../../../Pmmux/Core/Configuration/PortWardenConfig/index.md 'Pmmux\.Core\.Configuration\.PortWardenConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

The port warden configuration factory\.

<a name='Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions.AddPmmux(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Hosting.HostBuilderContext,System.Func_Pmmux.Core.Configuration.ListenerConfig_,System.Func_Pmmux.Core.Configuration.PortWardenConfig_,System.Func_Pmmux.Core.Configuration.RouterConfig_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IExtension_).routerConfigFactory'></a>

`routerConfigFactory` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[RouterConfig](../../../../Pmmux/Core/Configuration/RouterConfig/index.md 'Pmmux\.Core\.Configuration\.RouterConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

The router configuration factory\.

<a name='Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions.AddPmmux(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Hosting.HostBuilderContext,System.Func_Pmmux.Core.Configuration.ListenerConfig_,System.Func_Pmmux.Core.Configuration.PortWardenConfig_,System.Func_Pmmux.Core.Configuration.RouterConfig_,System.Collections.Generic.IEnumerable_Pmmux.Abstractions.IExtension_).extensions'></a>

`extensions` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[IExtension](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IExtension/index.md 'Pmmux\.Abstractions\.IExtension')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The extensions to load\.

#### Returns
[Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')