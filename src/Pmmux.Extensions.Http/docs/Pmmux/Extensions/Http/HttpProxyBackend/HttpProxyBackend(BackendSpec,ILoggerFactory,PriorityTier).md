#### [Pmmux\.Extensions\.Http](../../../../index.md 'index')
### [Pmmux\.Extensions\.Http](../index.md 'Pmmux\.Extensions\.Http').[HttpProxyBackend](index.md 'Pmmux\.Extensions\.Http\.HttpProxyBackend')

## HttpProxyBackend\(BackendSpec, ILoggerFactory, PriorityTier\) Constructor

HTTP backend that proxies requests to an upstream server using YARP\.

```csharp
public HttpProxyBackend(Pmmux.Abstractions.BackendSpec backendSpec, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, Pmmux.Abstractions.PriorityTier priorityTier=Pmmux.Abstractions.PriorityTier.Normal);
```
#### Parameters

<a name='Pmmux.Extensions.Http.HttpProxyBackend.HttpProxyBackend(Pmmux.Abstractions.BackendSpec,Microsoft.Extensions.Logging.ILoggerFactory,Pmmux.Abstractions.PriorityTier).backendSpec'></a>

`backendSpec` [BackendSpec](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The backend specification\.

<a name='Pmmux.Extensions.Http.HttpProxyBackend.HttpProxyBackend(Pmmux.Abstractions.BackendSpec,Microsoft.Extensions.Logging.ILoggerFactory,Pmmux.Abstractions.PriorityTier).loggerFactory'></a>

`loggerFactory` [Microsoft\.Extensions\.Logging\.ILoggerFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory 'Microsoft\.Extensions\.Logging\.ILoggerFactory')

The logger factory\.

<a name='Pmmux.Extensions.Http.HttpProxyBackend.HttpProxyBackend(Pmmux.Abstractions.BackendSpec,Microsoft.Extensions.Logging.ILoggerFactory,Pmmux.Abstractions.PriorityTier).priorityTier'></a>

`priorityTier` [PriorityTier](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/PriorityTier/index.md 'Pmmux\.Abstractions\.PriorityTier')

The priority tier of the backend\.