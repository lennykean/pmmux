#### [Pmmux\.Extensions\.Http](../../../../index.md 'index')
### [Pmmux\.Extensions\.Http](../index.md 'Pmmux\.Extensions\.Http')

## HttpBackend Class

Base class for HTTP\-based backends\.

```csharp
public abstract class HttpBackend : Pmmux.Abstractions.IConnectionOrientedBackend, Pmmux.Abstractions.IBackend, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; HttpBackend

Derived  
&#8627; [HttpProxyBackend](../HttpProxyBackend/index.md 'Pmmux\.Extensions\.Http\.HttpProxyBackend')

Implements [IConnectionOrientedBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend'), [IBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackend/index.md 'Pmmux\.Abstractions\.IBackend'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Properties | |
| :--- | :--- |
| [Backend](Backend.md 'Pmmux\.Extensions\.Http\.HttpBackend\.Backend') | The backend metadata and configuration information\. |

| Methods | |
| :--- | :--- |
| [CanHandleConnectionAsync\(IClientConnectionPreview, CancellationToken\)](CanHandleConnectionAsync(IClientConnectionPreview,CancellationToken).md 'Pmmux\.Extensions\.Http\.HttpBackend\.CanHandleConnectionAsync\(Pmmux\.Abstractions\.IClientConnectionPreview, System\.Threading\.CancellationToken\)') | Determine whether this backend can handle the client connection\. |
| [CreateBackendConnectionAsync\(IClientConnection, CancellationToken\)](CreateBackendConnectionAsync(IClientConnection,CancellationToken).md 'Pmmux\.Extensions\.Http\.HttpBackend\.CreateBackendConnectionAsync\(Pmmux\.Abstractions\.IClientConnection, System\.Threading\.CancellationToken\)') | Create and establish the backend connection for the client\. |
| [InitializeAsync\(CancellationToken\)](InitializeAsync(CancellationToken).md 'Pmmux\.Extensions\.Http\.HttpBackend\.InitializeAsync\(System\.Threading\.CancellationToken\)') | Initialize the backend at startup\. |
