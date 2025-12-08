#### [Pmmux\.Core](../../../../index.md 'index')
### [Microsoft\.Extensions\.DependencyInjection](../index.md 'Microsoft\.Extensions\.DependencyInjection')

## ServiceCollectionExtensions Class

Extension methods for registering Pmmux services in an [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')\.

```csharp
public static class ServiceCollectionExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; ServiceCollectionExtensions

| Methods | |
| :--- | :--- |
| [AddPmmux\(this IServiceCollection, HostBuilderContext, Func&lt;ListenerConfig&gt;, Func&lt;PortWardenConfig&gt;, Func&lt;RouterConfig&gt;, IEnumerable&lt;IExtension&gt;\)](AddPmmux(thisIServiceCollection,HostBuilderContext,Func_ListenerConfig_,Func_PortWardenConfig_,Func_RouterConfig_,IEnumerable_IExtension_).md 'Microsoft\.Extensions\.DependencyInjection\.ServiceCollectionExtensions\.AddPmmux\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, Microsoft\.Extensions\.Hosting\.HostBuilderContext, System\.Func\<Pmmux\.Core\.Configuration\.ListenerConfig\>, System\.Func\<Pmmux\.Core\.Configuration\.PortWardenConfig\>, System\.Func\<Pmmux\.Core\.Configuration\.RouterConfig\>, System\.Collections\.Generic\.IEnumerable\<Pmmux\.Abstractions\.IExtension\>\)') | Register core Pmmux services and services from the provided extensions\. |
