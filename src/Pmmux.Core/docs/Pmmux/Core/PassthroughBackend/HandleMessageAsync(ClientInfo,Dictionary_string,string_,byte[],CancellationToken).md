#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PassthroughBackend](index.md 'Pmmux\.Core\.PassthroughBackend')

## PassthroughBackend\.HandleMessageAsync\(ClientInfo, Dictionary\<string,string\>, byte\[\], CancellationToken\) Method

Process the message from the client\.

```csharp
public virtual System.Threading.Tasks.Task HandleMessageAsync(Pmmux.Abstractions.ClientInfo client, System.Collections.Generic.Dictionary<string,string> messageMetadata, byte[] message, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.PassthroughBackend.HandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,byte[],System.Threading.CancellationToken).client'></a>

`client` [ClientInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/ClientInfo/index.md 'Pmmux\.Abstractions\.ClientInfo')

The client that sent the message\.

<a name='Pmmux.Core.PassthroughBackend.HandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,byte[],System.Threading.CancellationToken).messageMetadata'></a>

`messageMetadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

<a name='Pmmux.Core.PassthroughBackend.HandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,byte[],System.Threading.CancellationToken).message'></a>

`message` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The message content\.

<a name='Pmmux.Core.PassthroughBackend.HandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,byte[],System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [HandleMessageAsync\(ClientInfo, Dictionary&lt;string,string&gt;, byte\[\], CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionlessBackend/HandleMessageAsync(ClientInfo,Dictionary_string,string_,byte[],CancellationToken).md 'Pmmux\.Abstractions\.IConnectionlessBackend\.HandleMessageAsync\(Pmmux\.Abstractions\.ClientInfo,System\.Collections\.Generic\.Dictionary\{System\.String,System\.String\},System\.Byte\[\],System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the message handling operation\.

### Remarks
Use the [IClientWriterFactory](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IClientWriterFactory/index.md 'Pmmux\.Abstractions\.IClientWriterFactory') provided during initialization
to send responses back to the client\.