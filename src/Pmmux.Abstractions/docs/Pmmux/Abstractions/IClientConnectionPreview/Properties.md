#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IClientConnectionPreview](index.md 'Pmmux\.Abstractions\.IClientConnectionPreview')

## IClientConnectionPreview\.Properties Property

Metadata associated with the connection\.

```csharp
System.Collections.Generic.IReadOnlyDictionary<string,string> Properties { get; }
```

#### Property Value
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

### Remarks
Properties are populated by [IClientConnectionNegotiator](../IClientConnectionNegotiator/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator') implementations
and may include `tls`, `tls.sni`, `host`, `path`, and other metadata\.