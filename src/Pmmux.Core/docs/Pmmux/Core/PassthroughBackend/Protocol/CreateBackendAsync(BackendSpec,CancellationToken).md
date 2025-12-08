#### [Pmmux\.Core](../../../../index.md 'index')
### [Pmmux\.Core](../../index.md 'Pmmux\.Core').[PassthroughBackend](../index.md 'Pmmux\.Core\.PassthroughBackend').[Protocol](index.md 'Pmmux\.Core\.PassthroughBackend\.Protocol')

## PassthroughBackend\.Protocol\.CreateBackendAsync\(BackendSpec, CancellationToken\) Method

Create a backend instance from the given specification\.

```csharp
public System.Threading.Tasks.Task<Pmmux.Abstractions.IBackend> CreateBackendAsync(Pmmux.Abstractions.BackendSpec spec, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.PassthroughBackend.Protocol.CreateBackendAsync(Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).spec'></a>

`spec` [BackendSpec](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The backend specification containing name and configuration parameters\.

<a name='Pmmux.Core.PassthroughBackend.Protocol.CreateBackendAsync(Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [CreateBackendAsync\(BackendSpec, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendProtocol/CreateBackendAsync(BackendSpec,CancellationToken).md 'Pmmux\.Abstractions\.IBackendProtocol\.CreateBackendAsync\(Pmmux\.Abstractions\.BackendSpec,System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[IBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackend/index.md 'Pmmux\.Abstractions\.IBackend')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that resolves to the created backend instance\. The backend should implement
either [IConnectionOrientedBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend') or [IConnectionlessBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionlessBackend/index.md 'Pmmux\.Abstractions\.IConnectionlessBackend')\.