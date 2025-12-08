#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IConnectionlessBackend](index.md 'Pmmux\.Abstractions\.IConnectionlessBackend')

## IConnectionlessBackend\.InitializeAsync\(IClientWriterFactory, CancellationToken\) Method

Initialize the backend with the client writer factory\.

```csharp
System.Threading.Tasks.Task InitializeAsync(Pmmux.Abstractions.IClientWriterFactory clientWriterFactory, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IConnectionlessBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).clientWriterFactory'></a>

`clientWriterFactory` [IClientWriterFactory](../IClientWriterFactory/index.md 'Pmmux\.Abstractions\.IClientWriterFactory')

The factory to create writers for sending responses to clients\.

<a name='Pmmux.Abstractions.IConnectionlessBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the initialization operation\.

### Remarks
The [clientWriterFactory](InitializeAsync(IClientWriterFactory,CancellationToken).md#Pmmux.Abstractions.IConnectionlessBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).clientWriterFactory 'Pmmux\.Abstractions\.IConnectionlessBackend\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory, System\.Threading\.CancellationToken\)\.clientWriterFactory') should be stored for use in [HandleMessageAsync\(ClientInfo, Dictionary&lt;string,string&gt;, byte\[\], CancellationToken\)](HandleMessageAsync(ClientInfo,Dictionary_string,string_,byte[],CancellationToken).md 'Pmmux\.Abstractions\.IConnectionlessBackend\.HandleMessageAsync\(Pmmux\.Abstractions\.ClientInfo, System\.Collections\.Generic\.Dictionary\<string,string\>, byte\[\], System\.Threading\.CancellationToken\)')
to send responses back to clients\.