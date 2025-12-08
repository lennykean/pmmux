#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[Router](index.md 'Pmmux\.Core\.Router')

## Router\.RouteMessageAsync\(byte\[\], ClientInfo, CancellationToken\) Method

Route a client message to a backend\.

```csharp
public System.Threading.Tasks.Task<Pmmux.Abstractions.IRouter.Result> RouteMessageAsync(byte[] messageBuffer, Pmmux.Abstractions.ClientInfo client, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.Router.RouteMessageAsync(byte[],Pmmux.Abstractions.ClientInfo,System.Threading.CancellationToken).messageBuffer'></a>

`messageBuffer` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The message data\.

<a name='Pmmux.Core.Router.RouteMessageAsync(byte[],Pmmux.Abstractions.ClientInfo,System.Threading.CancellationToken).client'></a>

`client` [ClientInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/ClientInfo/index.md 'Pmmux\.Abstractions\.ClientInfo')

The client information\.

<a name='Pmmux.Core.Router.RouteMessageAsync(byte[],Pmmux.Abstractions.ClientInfo,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [RouteMessageAsync\(byte\[\], ClientInfo, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/RouteMessageAsync(byte[],ClientInfo,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.RouteMessageAsync\(System\.Byte\[\],Pmmux\.Abstractions\.ClientInfo,System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Result](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/Result/index.md 'Pmmux\.Abstractions\.IRouter\.Result')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
Routing result with selected backend or failure reason\.