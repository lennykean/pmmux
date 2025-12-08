#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IConnectionlessBackend](index.md 'Pmmux\.Abstractions\.IConnectionlessBackend')

## IConnectionlessBackend\.HandleMessageAsync\(ClientInfo, Dictionary\<string,string\>, byte\[\], CancellationToken\) Method

Process the message from the client\.

```csharp
System.Threading.Tasks.Task HandleMessageAsync(Pmmux.Abstractions.ClientInfo client, System.Collections.Generic.Dictionary<string,string> messageProperties, byte[] message, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IConnectionlessBackend.HandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,byte[],System.Threading.CancellationToken).client'></a>

`client` [ClientInfo](../ClientInfo/index.md 'Pmmux\.Abstractions\.ClientInfo')

The client that sent the message\.

<a name='Pmmux.Abstractions.IConnectionlessBackend.HandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,byte[],System.Threading.CancellationToken).messageProperties'></a>

`messageProperties` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

The metadata about the message\.

<a name='Pmmux.Abstractions.IConnectionlessBackend.HandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,byte[],System.Threading.CancellationToken).message'></a>

`message` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The message content\.

<a name='Pmmux.Abstractions.IConnectionlessBackend.HandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,byte[],System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the message handling operation\.

### Remarks
Use the [IClientWriterFactory](../IClientWriterFactory/index.md 'Pmmux\.Abstractions\.IClientWriterFactory') provided during initialization
to send responses back to the client\.