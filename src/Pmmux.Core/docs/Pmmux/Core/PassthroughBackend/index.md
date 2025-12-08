#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## PassthroughBackend Class

Backend that forwards traffic to a configured upstream endpoint\.

```csharp
public class PassthroughBackend : Pmmux.Abstractions.IConnectionOrientedBackend, Pmmux.Abstractions.IBackend, System.IAsyncDisposable, Pmmux.Abstractions.IConnectionlessBackend, Pmmux.Abstractions.IHealthCheckBackend
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PassthroughBackend

Implements [IConnectionOrientedBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend'), [IBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackend/index.md 'Pmmux\.Abstractions\.IBackend'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable'), [IConnectionlessBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionlessBackend/index.md 'Pmmux\.Abstractions\.IConnectionlessBackend'), [IHealthCheckBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IHealthCheckBackend/index.md 'Pmmux\.Abstractions\.IHealthCheckBackend')

| Constructors | |
| :--- | :--- |
| [PassthroughBackend\(BackendSpec\)](PassthroughBackend(BackendSpec).md 'Pmmux\.Core\.PassthroughBackend\.PassthroughBackend\(Pmmux\.Abstractions\.BackendSpec\)') | |

| Properties | |
| :--- | :--- |
| [Backend](Backend.md 'Pmmux\.Core\.PassthroughBackend\.Backend') | The backend metadata and configuration information\. |

| Methods | |
| :--- | :--- |
| [CanHandleConnectionAsync\(IClientConnectionPreview, CancellationToken\)](CanHandleConnectionAsync(IClientConnectionPreview,CancellationToken).md 'Pmmux\.Core\.PassthroughBackend\.CanHandleConnectionAsync\(Pmmux\.Abstractions\.IClientConnectionPreview, System\.Threading\.CancellationToken\)') | Determine whether this backend can handle the client connection\. |
| [CanHandleMessageAsync\(ClientInfo, Dictionary&lt;string,string&gt;, ReadOnlyMemory&lt;byte&gt;, CancellationToken\)](CanHandleMessageAsync(ClientInfo,Dictionary_string,string_,ReadOnlyMemory_byte_,CancellationToken).md 'Pmmux\.Core\.PassthroughBackend\.CanHandleMessageAsync\(Pmmux\.Abstractions\.ClientInfo, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.ReadOnlyMemory\<byte\>, System\.Threading\.CancellationToken\)') | Determine whether this backend can handle the message\. |
| [CreateBackendConnectionAsync\(IClientConnection, CancellationToken\)](CreateBackendConnectionAsync(IClientConnection,CancellationToken).md 'Pmmux\.Core\.PassthroughBackend\.CreateBackendConnectionAsync\(Pmmux\.Abstractions\.IClientConnection, System\.Threading\.CancellationToken\)') | Create and establish the backend connection for the client\. |
| [HandleMessageAsync\(ClientInfo, Dictionary&lt;string,string&gt;, byte\[\], CancellationToken\)](HandleMessageAsync(ClientInfo,Dictionary_string,string_,byte[],CancellationToken).md 'Pmmux\.Core\.PassthroughBackend\.HandleMessageAsync\(Pmmux\.Abstractions\.ClientInfo, System\.Collections\.Generic\.Dictionary\<string,string\>, byte\[\], System\.Threading\.CancellationToken\)') | Process the message from the client\. |
| [HealthCheckAsync\(Protocol, IReadOnlyDictionary&lt;string,string&gt;, CancellationToken\)](HealthCheckAsync(Protocol,IReadOnlyDictionary_string,string_,CancellationToken).md 'Pmmux\.Core\.PassthroughBackend\.HealthCheckAsync\(Mono\.Nat\.Protocol, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, System\.Threading\.CancellationToken\)') | Perform a health check to determine operational status\. |
| [InitializeAsync\(IClientWriterFactory, CancellationToken\)](InitializeAsync.md#Pmmux.Core.PassthroughBackend.InitializeAsync(Pmmux.Abstractions.IClientWriterFactory,System.Threading.CancellationToken) 'Pmmux\.Core\.PassthroughBackend\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory, System\.Threading\.CancellationToken\)') | Initialize the backend with the client writer factory\. |
| [InitializeAsync\(CancellationToken\)](InitializeAsync.md#Pmmux.Core.PassthroughBackend.InitializeAsync(System.Threading.CancellationToken) 'Pmmux\.Core\.PassthroughBackend\.InitializeAsync\(System\.Threading\.CancellationToken\)') | Initialize the backend at startup\. |
