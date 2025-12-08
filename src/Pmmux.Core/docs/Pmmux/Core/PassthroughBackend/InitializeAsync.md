#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PassthroughBackend](index.md 'Pmmux\.Core\.PassthroughBackend')

## PassthroughBackend\.InitializeAsync Method

| Overloads | |
| :--- | :--- |
| [InitializeAsync\(IClientWriterFactory, CancellationToken\)](InitializeAsync.md#Pmmux.Core.PassthroughBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken) 'Pmmux\.Core\.PassthroughBackend\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory, System\.Threading\.CancellationToken\)') | Initialize the backend with the client writer factory\. |
| [InitializeAsync\(CancellationToken\)](InitializeAsync.md#Pmmux.Core.PassthroughBackend.InitializeAsync(System.Threading.CancellationToken) 'Pmmux\.Core\.PassthroughBackend\.InitializeAsync\(System\.Threading\.CancellationToken\)') | Initialize the backend at startup\. |

<a name='Pmmux.Core.PassthroughBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken)'></a>

## PassthroughBackend\.InitializeAsync\(IClientWriterFactory, CancellationToken\) Method

Initialize the backend with the client writer factory\.

```csharp
public virtual System.Threading.Tasks.Task InitializeAsync(Pmmux.Abstractions.IClientWriterFactory clientWriterFactory, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.PassthroughBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).clientWriterFactory'></a>

`clientWriterFactory` [IClientWriterFactory](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IClientWriterFactory/index.md 'Pmmux\.Abstractions\.IClientWriterFactory')

The factory to create writers for sending responses to clients\.

<a name='Pmmux.Core.PassthroughBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [InitializeAsync\(IClientWriterFactory, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionlessBackend/InitializeAsync(IClientWriterFactory,CancellationToken).md 'Pmmux\.Abstractions\.IConnectionlessBackend\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory,System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the initialization operation\.

### Remarks
The [clientWriterFactory](index.md#Pmmux.Core.PassthroughBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken).clientWriterFactory 'Pmmux\.Core\.PassthroughBackend\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory, System\.Threading\.CancellationToken\)\.clientWriterFactory') should be stored for use in [HandleMessageAsync\(ClientInfo, Dictionary&lt;string,string&gt;, byte\[\], CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionlessBackend/HandleMessageAsync(ClientInfo,Dictionary_string,string_,byte[],CancellationToken).md 'Pmmux\.Abstractions\.IConnectionlessBackend\.HandleMessageAsync\(Pmmux\.Abstractions\.ClientInfo,System\.Collections\.Generic\.Dictionary\{System\.String,System\.String\},System\.Byte\[\],System\.Threading\.CancellationToken\)')
to send responses back to clients\.

<a name='Pmmux.Core.PassthroughBackend.InitializeAsync(System.Threading.CancellationToken)'></a>

## PassthroughBackend\.InitializeAsync\(CancellationToken\) Method

Initialize the backend at startup\.

```csharp
public virtual System.Threading.Tasks.Task InitializeAsync(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.PassthroughBackend.InitializeAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [InitializeAsync\(CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionOrientedBackend/InitializeAsync(CancellationToken).md 'Pmmux\.Abstractions\.IConnectionOrientedBackend\.InitializeAsync\(System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the initialization operation\.

### Remarks
Perform any one\-time setup such as establishing persistent connections to upstream servers,
warming caches, or validating configuration\.