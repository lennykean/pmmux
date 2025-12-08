#### [Pmmux\.Core](../../../../index.md 'index')
### [Pmmux\.Core](../../index.md 'Pmmux\.Core').[PassthroughBackend](../index.md 'Pmmux\.Core\.PassthroughBackend').[Protocol](index.md 'Pmmux\.Core\.PassthroughBackend\.Protocol')

## PassthroughBackend\.Protocol\.Name Property

Protocol name used to match against [ProtocolName](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendSpec/ProtocolName.md 'Pmmux\.Abstractions\.BackendSpec\.ProtocolName')\.

```csharp
public string Name { get; }
```

Implements [Name](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendProtocol/Name.md 'Pmmux\.Abstractions\.IBackendProtocol\.Name')

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Remarks
The name should be lowercase and use hyphens for multi\-word names \(e\.g\., "http\-proxy"\)\.