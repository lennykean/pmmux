#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IConnectionlessBackend](index.md 'Pmmux\.Abstractions\.IConnectionlessBackend')

## IConnectionlessBackend\.CanHandleMessageAsync\(ClientInfo, Dictionary\<string,string\>, ReadOnlyMemory\<byte\>, CancellationToken\) Method

Determine whether this backend can handle the message\.

```csharp
System.Threading.Tasks.Task<bool> CanHandleMessageAsync(Pmmux.Abstractions.ClientInfo client, System.Collections.Generic.Dictionary<string,string> messageProperties, System.ReadOnlyMemory<byte> message, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IConnectionlessBackend.CanHandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,System.ReadOnlyMemory_byte_,System.Threading.CancellationToken).client'></a>

`client` [ClientInfo](../ClientInfo/index.md 'Pmmux\.Abstractions\.ClientInfo')

The client that sent the message\.

<a name='Pmmux.Abstractions.IConnectionlessBackend.CanHandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,System.ReadOnlyMemory_byte_,System.Threading.CancellationToken).messageProperties'></a>

`messageProperties` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

The metadata about the message \(protocol\-specific\)\.

<a name='Pmmux.Abstractions.IConnectionlessBackend.CanHandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,System.ReadOnlyMemory_byte_,System.Threading.CancellationToken).message'></a>

`message` [System\.ReadOnlyMemory&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlymemory-1 'System\.ReadOnlyMemory\`1')[System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlymemory-1 'System\.ReadOnlyMemory\`1')

The message content as a read\-only buffer for inspection\.

<a name='Pmmux.Abstractions.IConnectionlessBackend.CanHandleMessageAsync(Pmmux.Abstractions.ClientInfo,System.Collections.Generic.Dictionary_string,string_,System.ReadOnlyMemory_byte_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
`true` if this backend can handle the message; otherwise, `false`\.