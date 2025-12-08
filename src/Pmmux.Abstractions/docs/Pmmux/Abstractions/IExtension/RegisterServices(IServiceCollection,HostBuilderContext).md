#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IExtension](index.md 'Pmmux\.Abstractions\.IExtension')

## IExtension\.RegisterServices\(IServiceCollection, HostBuilderContext\) Method

Register services provided by this extension\.

```csharp
void RegisterServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Hosting.HostBuilderContext hostContext);
```
#### Parameters

<a name='Pmmux.Abstractions.IExtension.RegisterServices(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Hosting.HostBuilderContext).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The service collection to register services into\.

<a name='Pmmux.Abstractions.IExtension.RegisterServices(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Hosting.HostBuilderContext).hostContext'></a>

`hostContext` [Microsoft\.Extensions\.Hosting\.HostBuilderContext](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuildercontext 'Microsoft\.Extensions\.Hosting\.HostBuilderContext')

The host builder context containing configuration and environment information\.

### Remarks
Common services include [IBackendProtocol](../IBackendProtocol/index.md 'Pmmux\.Abstractions\.IBackendProtocol'), [IRoutingStrategy](../IRoutingStrategy/index.md 'Pmmux\.Abstractions\.IRoutingStrategy'),
[IClientConnectionNegotiator](../IClientConnectionNegotiator/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator'), and [IMetricSink](../IMetricSink/index.md 'Pmmux\.Abstractions\.IMetricSink')\.