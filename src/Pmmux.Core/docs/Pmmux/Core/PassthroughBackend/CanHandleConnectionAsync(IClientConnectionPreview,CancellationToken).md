#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PassthroughBackend](index.md 'Pmmux\.Core\.PassthroughBackend')

## PassthroughBackend\.CanHandleConnectionAsync\(IClientConnectionPreview, CancellationToken\) Method

Determine whether this backend can handle the client connection\.

```csharp
public virtual System.Threading.Tasks.Task<bool> CanHandleConnectionAsync(Pmmux.Abstractions.IClientConnectionPreview preview, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.PassthroughBackend.CanHandleConnectionAsync(Pmmux.Abstractions.IClientConnectionPreview,System.Threading.CancellationToken).preview'></a>

`preview` [IClientConnectionPreview](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IClientConnectionPreview/index.md 'Pmmux\.Abstractions\.IClientConnectionPreview')

<a name='Pmmux.Core.PassthroughBackend.CanHandleConnectionAsync(Pmmux.Abstractions.IClientConnectionPreview,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [CanHandleConnectionAsync\(IClientConnectionPreview, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionOrientedBackend/CanHandleConnectionAsync(IClientConnectionPreview,CancellationToken).md 'Pmmux\.Abstractions\.IConnectionOrientedBackend\.CanHandleConnectionAsync\(Pmmux\.Abstractions\.IClientConnectionPreview,System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
`true` if this backend can handle the connection; otherwise, `false`\.

### Remarks
Use [Properties](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IClientConnectionPreview/Properties.md 'Pmmux\.Abstractions\.IClientConnectionPreview\.Properties') to check connection metadata or
[Ingress](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IClientConnectionPreview/Ingress.md 'Pmmux\.Abstractions\.IClientConnectionPreview\.Ingress') to peek at initial bytes for protocol detection\.