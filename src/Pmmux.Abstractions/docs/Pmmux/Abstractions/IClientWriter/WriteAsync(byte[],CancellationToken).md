#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IClientWriter](index.md 'Pmmux\.Abstractions\.IClientWriter')

## IClientWriter\.WriteAsync\(byte\[\], CancellationToken\) Method

Write data to the client through the listener\.

```csharp
System.Threading.Tasks.Task WriteAsync(byte[] data, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Pmmux.Abstractions.IClientWriter.WriteAsync(byte[],System.Threading.CancellationToken).data'></a>

`data` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The data to write\.

<a name='Pmmux.Abstractions.IClientWriter.WriteAsync(byte[],System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')