#### [Pmmux\.Core](../../../../index.md 'index')
### [Pmmux\.Core](../../index.md 'Pmmux\.Core').[PassthroughBackend](../index.md 'Pmmux\.Core\.PassthroughBackend')

## PassthroughBackend\.Protocol Class

Protocol factory for [PassthroughBackend](../index.md 'Pmmux\.Core\.PassthroughBackend')\.

```csharp
public sealed class PassthroughBackend.Protocol : Pmmux.Abstractions.IBackendProtocol
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; Protocol

Implements [IBackendProtocol](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendProtocol/index.md 'Pmmux\.Abstractions\.IBackendProtocol')

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Core\.PassthroughBackend\.Protocol\.Name') | Protocol name used to match against [ProtocolName](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendSpec/ProtocolName.md 'Pmmux\.Abstractions\.BackendSpec\.ProtocolName')\. |

| Methods | |
| :--- | :--- |
| [CreateBackendAsync\(BackendSpec, CancellationToken\)](CreateBackendAsync(BackendSpec,CancellationToken).md 'Pmmux\.Core\.PassthroughBackend\.Protocol\.CreateBackendAsync\(Pmmux\.Abstractions\.BackendSpec, System\.Threading\.CancellationToken\)') | Create a backend instance from the given specification\. |
