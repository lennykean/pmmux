#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IBackendProtocol Interface

Factory for creating backend instances from specifications\.

```csharp
public interface IBackendProtocol
```

### Remarks
Backend protocols interpret [BackendSpec](../BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec') configurations and create
corresponding [IBackend](../IBackend/index.md 'Pmmux\.Abstractions\.IBackend') instances\. Each protocol has a unique [Name](Name.md 'Pmmux\.Abstractions\.IBackendProtocol\.Name')
that is matched against [ProtocolName](../BackendSpec/ProtocolName.md 'Pmmux\.Abstractions\.BackendSpec\.ProtocolName')\.

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Abstractions\.IBackendProtocol\.Name') | Protocol name used to match against [ProtocolName](../BackendSpec/ProtocolName.md 'Pmmux\.Abstractions\.BackendSpec\.ProtocolName')\. |

| Methods | |
| :--- | :--- |
| [CreateBackendAsync\(BackendSpec, CancellationToken\)](CreateBackendAsync(BackendSpec,CancellationToken).md 'Pmmux\.Abstractions\.IBackendProtocol\.CreateBackendAsync\(Pmmux\.Abstractions\.BackendSpec, System\.Threading\.CancellationToken\)') | Create a backend instance from the given specification\. |
