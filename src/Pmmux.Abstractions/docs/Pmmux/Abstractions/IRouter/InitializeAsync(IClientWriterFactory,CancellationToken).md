#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IRouter](index.md 'Pmmux\.Abstractions\.IRouter')

## IRouter\.InitializeAsync\(IClientWriterFactory, CancellationToken\) Method

Initialize the router with listener information at startup\.

```csharp
System.Threading.Tasks.Task InitializeAsync(Pmmux.Abstractions.IClientWriterFactory clientWriterFactory, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IRouter.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).clientWriterFactory'></a>

`clientWriterFactory` [IClientWriterFactory](../IClientWriterFactory/index.md 'Pmmux\.Abstractions\.IClientWriterFactory')

The factory to create client writers\.

<a name='Pmmux.Abstractions.IRouter.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the initialization operation\.