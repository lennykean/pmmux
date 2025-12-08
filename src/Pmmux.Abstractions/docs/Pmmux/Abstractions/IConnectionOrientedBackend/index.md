#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IConnectionOrientedBackend Interface

Backend for connection\-oriented protocols like TCP\.

```csharp
public interface IConnectionOrientedBackend : Pmmux.Abstractions.IBackend, System.IAsyncDisposable
```

Implements [IBackend](../IBackend/index.md 'Pmmux\.Abstractions\.IBackend'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Connection\-oriented backends handle persistent bidirectional connections where data flows
continuously between client and backend\. This interface is used for TCP\-based protocols\.
For message\-based protocols like UDP, [IConnectionlessBackend](../IConnectionlessBackend/index.md 'Pmmux\.Abstractions\.IConnectionlessBackend') should be used\.

| Methods | |
| :--- | :--- |
| [CanHandleConnectionAsync\(IClientConnectionPreview, CancellationToken\)](CanHandleConnectionAsync(IClientConnectionPreview,CancellationToken).md 'Pmmux\.Abstractions\.IConnectionOrientedBackend\.CanHandleConnectionAsync\(Pmmux\.Abstractions\.IClientConnectionPreview, System\.Threading\.CancellationToken\)') | Determine whether this backend can handle the client connection\. |
| [CreateBackendConnectionAsync\(IClientConnection, CancellationToken\)](CreateBackendConnectionAsync(IClientConnection,CancellationToken).md 'Pmmux\.Abstractions\.IConnectionOrientedBackend\.CreateBackendConnectionAsync\(Pmmux\.Abstractions\.IClientConnection, System\.Threading\.CancellationToken\)') | Create and establish the backend connection for the client\. |
| [InitializeAsync\(CancellationToken\)](InitializeAsync(CancellationToken).md 'Pmmux\.Abstractions\.IConnectionOrientedBackend\.InitializeAsync\(System\.Threading\.CancellationToken\)') | Initialize the backend at startup\. |
