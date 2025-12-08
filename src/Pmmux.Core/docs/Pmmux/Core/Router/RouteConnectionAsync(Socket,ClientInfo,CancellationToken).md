#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[Router](index.md 'Pmmux\.Core\.Router')

## Router\.RouteConnectionAsync\(Socket, ClientInfo, CancellationToken\) Method

Route a client connection to a backend\.

```csharp
public System.Threading.Tasks.Task<Pmmux.Abstractions.IRouter.Result> RouteConnectionAsync(System.Net.Sockets.Socket connection, Pmmux.Abstractions.ClientInfo client, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.Router.RouteConnectionAsync(System.Net.Sockets.Socket,Pmmux.Abstractions.ClientInfo,System.Threading.CancellationToken).connection'></a>

`connection` [System\.Net\.Sockets\.Socket](https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.socket 'System\.Net\.Sockets\.Socket')

The client socket connection\.

<a name='Pmmux.Core.Router.RouteConnectionAsync(System.Net.Sockets.Socket,Pmmux.Abstractions.ClientInfo,System.Threading.CancellationToken).client'></a>

`client` [ClientInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/ClientInfo/index.md 'Pmmux\.Abstractions\.ClientInfo')

The client information\.

<a name='Pmmux.Core.Router.RouteConnectionAsync(System.Net.Sockets.Socket,Pmmux.Abstractions.ClientInfo,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [RouteConnectionAsync\(Socket, ClientInfo, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/RouteConnectionAsync(Socket,ClientInfo,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.RouteConnectionAsync\(System\.Net\.Sockets\.Socket,Pmmux\.Abstractions\.ClientInfo,System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Result](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/Result/index.md 'Pmmux\.Abstractions\.IRouter\.Result')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
Routing result with selected backend or failure reason\.