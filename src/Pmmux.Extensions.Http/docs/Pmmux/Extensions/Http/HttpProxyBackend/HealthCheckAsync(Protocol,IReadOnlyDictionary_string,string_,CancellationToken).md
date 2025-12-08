#### [Pmmux\.Extensions\.Http](../../../../index.md 'index')
### [Pmmux\.Extensions\.Http](../index.md 'Pmmux\.Extensions\.Http').[HttpProxyBackend](index.md 'Pmmux\.Extensions\.Http\.HttpProxyBackend')

## HttpProxyBackend\.HealthCheckAsync\(Protocol, IReadOnlyDictionary\<string,string\>, CancellationToken\) Method

Perform a health check to determine operational status\.

```csharp
public System.Threading.Tasks.Task<Pmmux.Abstractions.HealthCheckResult> HealthCheckAsync(Mono.Nat.Protocol _, System.Collections.Generic.IReadOnlyDictionary<string,string> parameters, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Extensions.Http.HttpProxyBackend.HealthCheckAsync(Mono.Nat.Protocol,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken)._'></a>

`_` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

<a name='Pmmux.Extensions.Http.HttpProxyBackend.HealthCheckAsync(Mono.Nat.Protocol,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).parameters'></a>

`parameters` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

Health check configuration parameters from [Parameters](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/HealthCheckSpec/Parameters.md 'Pmmux\.Abstractions\.HealthCheckSpec\.Parameters')\.
Common parameters include `timeout`, `interval`, and protocol\-specific options\.

<a name='Pmmux.Extensions.Http.HttpProxyBackend.HealthCheckAsync(Mono.Nat.Protocol,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [HealthCheckAsync\(Protocol, IReadOnlyDictionary&lt;string,string&gt;, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IHealthCheckBackend/HealthCheckAsync(Protocol,IReadOnlyDictionary_string,string_,CancellationToken).md 'Pmmux\.Abstractions\.IHealthCheckBackend\.HealthCheckAsync\(Mono\.Nat\.Protocol,System\.Collections\.Generic\.IReadOnlyDictionary\{System\.String,System\.String\},System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[HealthCheckResult](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/HealthCheckResult/index.md 'Pmmux\.Abstractions\.HealthCheckResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A [HealthCheckResult](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/HealthCheckResult/index.md 'Pmmux\.Abstractions\.HealthCheckResult') indicating the backend's current health status\.