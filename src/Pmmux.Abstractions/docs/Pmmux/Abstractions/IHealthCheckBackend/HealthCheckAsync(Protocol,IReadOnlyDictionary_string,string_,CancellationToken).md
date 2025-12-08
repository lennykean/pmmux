#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IHealthCheckBackend](index.md 'Pmmux\.Abstractions\.IHealthCheckBackend')

## IHealthCheckBackend\.HealthCheckAsync\(Protocol, IReadOnlyDictionary\<string,string\>, CancellationToken\) Method

Perform a health check to determine operational status\.

```csharp
System.Threading.Tasks.Task<Pmmux.Abstractions.HealthCheckResult> HealthCheckAsync(Mono.Nat.Protocol networkProtocol, System.Collections.Generic.IReadOnlyDictionary<string,string> parameters, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IHealthCheckBackend.HealthCheckAsync(Mono.Nat.Protocol,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol used to perform the health check\.

<a name='Pmmux.Abstractions.IHealthCheckBackend.HealthCheckAsync(Mono.Nat.Protocol,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).parameters'></a>

`parameters` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

Health check configuration parameters from [Parameters](../HealthCheckSpec/Parameters.md 'Pmmux\.Abstractions\.HealthCheckSpec\.Parameters')\.
Common parameters include `timeout`, `interval`, and protocol\-specific options\.

<a name='Pmmux.Abstractions.IHealthCheckBackend.HealthCheckAsync(Mono.Nat.Protocol,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[HealthCheckResult](../HealthCheckResult/index.md 'Pmmux\.Abstractions\.HealthCheckResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A [HealthCheckResult](../HealthCheckResult/index.md 'Pmmux\.Abstractions\.HealthCheckResult') indicating the backend's current health status\.