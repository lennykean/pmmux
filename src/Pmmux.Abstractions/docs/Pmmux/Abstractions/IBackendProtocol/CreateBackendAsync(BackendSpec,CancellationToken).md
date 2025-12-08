#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IBackendProtocol](index.md 'Pmmux\.Abstractions\.IBackendProtocol')

## IBackendProtocol\.CreateBackendAsync\(BackendSpec, CancellationToken\) Method

Create a backend instance from the given specification\.

```csharp
System.Threading.Tasks.Task<Pmmux.Abstractions.IBackend> CreateBackendAsync(Pmmux.Abstractions.BackendSpec spec, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IBackendProtocol.CreateBackendAsync(Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).spec'></a>

`spec` [BackendSpec](../BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The backend specification containing name and configuration parameters\.

<a name='Pmmux.Abstractions.IBackendProtocol.CreateBackendAsync(Pmmux.Abstractions.BackendSpec,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[IBackend](../IBackend/index.md 'Pmmux\.Abstractions\.IBackend')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that resolves to the created backend instance\. The backend should implement
either [IConnectionOrientedBackend](../IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend') or [IConnectionlessBackend](../IConnectionlessBackend/index.md 'Pmmux\.Abstractions\.IConnectionlessBackend')\.