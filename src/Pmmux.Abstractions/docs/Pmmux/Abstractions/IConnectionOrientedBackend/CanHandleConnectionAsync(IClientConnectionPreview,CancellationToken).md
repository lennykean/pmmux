#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IConnectionOrientedBackend](index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend')

## IConnectionOrientedBackend\.CanHandleConnectionAsync\(IClientConnectionPreview, CancellationToken\) Method

Determine whether this backend can handle the client connection\.

```csharp
System.Threading.Tasks.Task<bool> CanHandleConnectionAsync(Pmmux.Abstractions.IClientConnectionPreview client, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IConnectionOrientedBackend.CanHandleConnectionAsync(Pmmux.Abstractions.IClientConnectionPreview,System.Threading.CancellationToken).client'></a>

`client` [IClientConnectionPreview](../IClientConnectionPreview/index.md 'Pmmux\.Abstractions\.IClientConnectionPreview')

The client connection preview for inspecting connection metadata and peeking at data\.

<a name='Pmmux.Abstractions.IConnectionOrientedBackend.CanHandleConnectionAsync(Pmmux.Abstractions.IClientConnectionPreview,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
`true` if this backend can handle the connection; otherwise, `false`\.

### Remarks
Use [Properties](../IClientConnectionPreview/Properties.md 'Pmmux\.Abstractions\.IClientConnectionPreview\.Properties') to check connection metadata or
[Ingress](../IClientConnectionPreview/Ingress.md 'Pmmux\.Abstractions\.IClientConnectionPreview\.Ingress') to peek at initial bytes for protocol detection\.