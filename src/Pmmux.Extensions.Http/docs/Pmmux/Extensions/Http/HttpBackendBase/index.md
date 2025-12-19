#### [Pmmux\.Extensions\.Http](../../../../index.md 'index')
### [Pmmux\.Extensions\.Http](../index.md 'Pmmux\.Extensions\.Http')

## HttpBackendBase Class

Base class for HTTP\-based backends\.

```csharp
public abstract class HttpBackendBase : Pmmux.Core.BackendBase, Pmmux.Abstractions.IConnectionOrientedBackend, Pmmux.Abstractions.IBackend, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; [BackendBase](..\..\Pmmux.Core\docs\Pmmux/Core/BackendBase/index.md 'Pmmux\.Core\.BackendBase') &#129106; HttpBackendBase

Derived  
&#8627; [HttpProxyBackend](../HttpProxyBackend/index.md 'Pmmux\.Extensions\.Http\.HttpProxyBackend')

Implements [IConnectionOrientedBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend'), [IBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackend/index.md 'Pmmux\.Abstractions\.IBackend'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Methods | |
| :--- | :--- |
| [CanHandleConnectionAsync\(IClientConnectionPreview, CancellationToken\)](CanHandleConnectionAsync(IClientConnectionPreview,CancellationToken).md 'Pmmux\.Extensions\.Http\.HttpBackendBase\.CanHandleConnectionAsync\(Pmmux\.Abstractions\.IClientConnectionPreview, System\.Threading\.CancellationToken\)') | Determine whether this backend can handle the client connection\. |
| [CreateBackendConnectionAsync\(IClientConnection, CancellationToken\)](CreateBackendConnectionAsync(IClientConnection,CancellationToken).md 'Pmmux\.Extensions\.Http\.HttpBackendBase\.CreateBackendConnectionAsync\(Pmmux\.Abstractions\.IClientConnection, System\.Threading\.CancellationToken\)') | Create and establish the backend connection for the client\. |
| [InitializeAsync\(CancellationToken\)](InitializeAsync(CancellationToken).md 'Pmmux\.Extensions\.Http\.HttpBackendBase\.InitializeAsync\(System\.Threading\.CancellationToken\)') | Initialize the backend at startup\. |
