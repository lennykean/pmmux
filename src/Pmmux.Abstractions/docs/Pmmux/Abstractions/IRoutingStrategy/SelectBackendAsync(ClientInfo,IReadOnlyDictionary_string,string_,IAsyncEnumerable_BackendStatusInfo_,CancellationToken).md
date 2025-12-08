#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IRoutingStrategy](index.md 'Pmmux\.Abstractions\.IRoutingStrategy')

## IRoutingStrategy\.SelectBackendAsync\(ClientInfo, IReadOnlyDictionary\<string,string\>, IAsyncEnumerable\<BackendStatusInfo\>, CancellationToken\) Method

Select a backend from matched candidates to handle the client request\.

```csharp
System.Threading.Tasks.Task<Pmmux.Abstractions.BackendInfo?> SelectBackendAsync(Pmmux.Abstractions.ClientInfo client, System.Collections.Generic.IReadOnlyDictionary<string,string> clientConnectionProperties, System.Collections.Generic.IAsyncEnumerable<Pmmux.Abstractions.BackendStatusInfo> matchedBackends, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IRoutingStrategy.SelectBackendAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Collections.Generic.IAsyncEnumerable_Pmmux.Abstractions.BackendStatusInfo_,System.Threading.CancellationToken).client'></a>

`client` [ClientInfo](../ClientInfo/index.md 'Pmmux\.Abstractions\.ClientInfo')

The client making the request\.

<a name='Pmmux.Abstractions.IRoutingStrategy.SelectBackendAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Collections.Generic.IAsyncEnumerable_Pmmux.Abstractions.BackendStatusInfo_,System.Threading.CancellationToken).clientConnectionProperties'></a>

`clientConnectionProperties` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The connection metadata that may influence selection\.

<a name='Pmmux.Abstractions.IRoutingStrategy.SelectBackendAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Collections.Generic.IAsyncEnumerable_Pmmux.Abstractions.BackendStatusInfo_,System.Threading.CancellationToken).matchedBackends'></a>

`matchedBackends` [System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[BackendStatusInfo](../BackendStatusInfo/index.md 'Pmmux\.Abstractions\.BackendStatusInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')

Async stream of backends that matched the request, provided in the order in which they were defined\.

<a name='Pmmux.Abstractions.IRoutingStrategy.SelectBackendAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Collections.Generic.IAsyncEnumerable_Pmmux.Abstractions.BackendStatusInfo_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[BackendInfo](../BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The selected backend's [BackendInfo](../BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo') if a suitable backend was found,
otherwise, `null` to indicate no backend is available\.