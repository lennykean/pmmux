#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[Router](index.md 'Pmmux\.Core\.Router')

## Router\.InitializeAsync\(IClientWriterFactory, CancellationToken\) Method

Initialize the router with listener information at startup\.

```csharp
public System.Threading.Tasks.Task InitializeAsync(Pmmux.Abstractions.IClientWriterFactory clientWriterFactory, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Pmmux.Core.Router.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).clientWriterFactory'></a>

`clientWriterFactory` [IClientWriterFactory](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IClientWriterFactory/index.md 'Pmmux\.Abstractions\.IClientWriterFactory')

The factory to create client writers\.

<a name='Pmmux.Core.Router.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [InitializeAsync\(IClientWriterFactory, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/InitializeAsync(IClientWriterFactory,CancellationToken).md 'Pmmux\.Abstractions\.IRouter\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory,System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the initialization operation\.