#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IClientConnection](index.md 'Pmmux\.Abstractions\.IClientConnection')

## IClientConnection\.Preview\(\) Method

Create a preview of the connection for peeking at incoming data without consuming it\.

```csharp
Pmmux.Abstractions.IClientConnectionPreview Preview();
```

#### Returns
[IClientConnectionPreview](../IClientConnectionPreview/index.md 'Pmmux\.Abstractions\.IClientConnectionPreview')  
[IClientConnectionPreview](../IClientConnectionPreview/index.md 'Pmmux\.Abstractions\.IClientConnectionPreview') providing peek operations for routing decisions\.

### Remarks
Previews are used by the router and backends to inspect connection properties and peek at the data stream
before committing to handling the connection\. Multiple previews can be created sequentially from the same
connection, allowing different backends to evaluate the connection during routing\.