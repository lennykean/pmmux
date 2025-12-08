#### [Pmmux\.Extensions\.Http](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Http](../../index.md 'Pmmux\.Extensions\.Http').[HttpProxyBackend](../index.md 'Pmmux\.Extensions\.Http\.HttpProxyBackend')

## HttpProxyBackend\.Protocol Class

Protocol factory for [HttpProxyBackend](../index.md 'Pmmux\.Extensions\.Http\.HttpProxyBackend')\.

```csharp
public sealed class HttpProxyBackend.Protocol : Pmmux.Abstractions.IBackendProtocol
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; Protocol

Implements [IBackendProtocol](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendProtocol/index.md 'Pmmux\.Abstractions\.IBackendProtocol')

| Constructors | |
| :--- | :--- |
| [Protocol\(ILoggerFactory\)](Protocol(ILoggerFactory).md 'Pmmux\.Extensions\.Http\.HttpProxyBackend\.Protocol\.Protocol\(Microsoft\.Extensions\.Logging\.ILoggerFactory\)') | Protocol factory for [HttpProxyBackend](../index.md 'Pmmux\.Extensions\.Http\.HttpProxyBackend')\. |

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Extensions\.Http\.HttpProxyBackend\.Protocol\.Name') | Protocol name used to match against [ProtocolName](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendSpec/ProtocolName.md 'Pmmux\.Abstractions\.BackendSpec\.ProtocolName')\. |

| Methods | |
| :--- | :--- |
| [CreateBackendAsync\(BackendSpec, CancellationToken\)](CreateBackendAsync(BackendSpec,CancellationToken).md 'Pmmux\.Extensions\.Http\.HttpProxyBackend\.Protocol\.CreateBackendAsync\(Pmmux\.Abstractions\.BackendSpec, System\.Threading\.CancellationToken\)') | Create a backend instance from the given specification\. |
