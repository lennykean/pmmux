#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IConnectionOrientedBackend](index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend')

## IConnectionOrientedBackend\.CreateBackendConnectionAsync\(IClientConnection, CancellationToken\) Method

Create and establish the backend connection for the client\.

```csharp
System.Threading.Tasks.Task<Pmmux.Abstractions.IConnection> CreateBackendConnectionAsync(Pmmux.Abstractions.IClientConnection client, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IConnectionOrientedBackend.CreateBackendConnectionAsync(Pmmux.Abstractions.IClientConnection,System.Threading.CancellationToken).client'></a>

`client` [IClientConnection](../IClientConnection/index.md 'Pmmux\.Abstractions\.IClientConnection')

The client connection with full read/write access\.

<a name='Pmmux.Abstractions.IConnectionOrientedBackend.CreateBackendConnectionAsync(Pmmux.Abstractions.IClientConnection,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[IConnection](../IConnection/index.md 'Pmmux\.Abstractions\.IConnection')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [IConnection](../IConnection/index.md 'Pmmux\.Abstractions\.IConnection') representing the established backend connection\.
The multiplexer will relay data bidirectionally between client and this connection\.