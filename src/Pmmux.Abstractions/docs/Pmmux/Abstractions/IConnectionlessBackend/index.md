#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IConnectionlessBackend Interface

Backend for connectionless protocols like UDP\.

```csharp
public interface IConnectionlessBackend : Pmmux.Abstractions.IBackend, System.IAsyncDisposable
```

Implements [IBackend](../IBackend/index.md 'Pmmux\.Abstractions\.IBackend'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Connectionless backends handle discrete messages without persistent connections\.
Each message is processed independently, and responses are sent via [IClientWriterFactory](../IClientWriterFactory/index.md 'Pmmux\.Abstractions\.IClientWriterFactory')\.
For persistent connection protocols like TCP, [IConnectionOrientedBackend](../IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend') should be used\.

| Methods | |
| :--- | :--- |
| [CanHandleMessageAsync\(ClientInfo, Dictionary&lt;string,string&gt;, ReadOnlyMemory&lt;byte&gt;, CancellationToken\)](CanHandleMessageAsync(ClientInfo,Dictionary_string,string_,ReadOnlyMemory_byte_,CancellationToken).md 'Pmmux\.Abstractions\.IConnectionlessBackend\.CanHandleMessageAsync\(Pmmux\.Abstractions\.ClientInfo, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.ReadOnlyMemory\<byte\>, System\.Threading\.CancellationToken\)') | Determine whether this backend can handle the message\. |
| [HandleMessageAsync\(ClientInfo, Dictionary&lt;string,string&gt;, byte\[\], CancellationToken\)](HandleMessageAsync(ClientInfo,Dictionary_string,string_,byte[],CancellationToken).md 'Pmmux\.Abstractions\.IConnectionlessBackend\.HandleMessageAsync\(Pmmux\.Abstractions\.ClientInfo, System\.Collections\.Generic\.Dictionary\<string,string\>, byte\[\], System\.Threading\.CancellationToken\)') | Process the message from the client\. |
| [InitializeAsync\(IClientWriterFactory, CancellationToken\)](InitializeAsync(IClientWriterFactory,CancellationToken).md 'Pmmux\.Abstractions\.IConnectionlessBackend\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory, System\.Threading\.CancellationToken\)') | Initialize the backend with the client writer factory\. |
