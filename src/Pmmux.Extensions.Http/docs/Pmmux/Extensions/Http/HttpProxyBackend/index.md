#### [Pmmux\.Extensions\.Http](../../../../index.md 'index')
### [Pmmux\.Extensions\.Http](../index.md 'Pmmux\.Extensions\.Http')

## HttpProxyBackend Class

HTTP backend that proxies requests to an upstream server using YARP\.

```csharp
public sealed class HttpProxyBackend : Pmmux.Extensions.Http.HttpBackend, Pmmux.Abstractions.IHealthCheckBackend, Pmmux.Abstractions.IBackend, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; [HttpBackend](../HttpBackend/index.md 'Pmmux\.Extensions\.Http\.HttpBackend') &#129106; HttpProxyBackend

Implements [IHealthCheckBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IHealthCheckBackend/index.md 'Pmmux\.Abstractions\.IHealthCheckBackend'), [IBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackend/index.md 'Pmmux\.Abstractions\.IBackend'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Constructors | |
| :--- | :--- |
| [HttpProxyBackend\(BackendSpec, ILoggerFactory, PriorityTier\)](HttpProxyBackend(BackendSpec,ILoggerFactory,PriorityTier).md 'Pmmux\.Extensions\.Http\.HttpProxyBackend\.HttpProxyBackend\(Pmmux\.Abstractions\.BackendSpec, Microsoft\.Extensions\.Logging\.ILoggerFactory, Pmmux\.Abstractions\.PriorityTier\)') | HTTP backend that proxies requests to an upstream server using YARP\. |

| Methods | |
| :--- | :--- |
| [HealthCheckAsync\(Protocol, IReadOnlyDictionary&lt;string,string&gt;, CancellationToken\)](HealthCheckAsync(Protocol,IReadOnlyDictionary_string,string_,CancellationToken).md 'Pmmux\.Extensions\.Http\.HttpProxyBackend\.HealthCheckAsync\(Mono\.Nat\.Protocol, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, System\.Threading\.CancellationToken\)') | Perform a health check to determine operational status\. |
