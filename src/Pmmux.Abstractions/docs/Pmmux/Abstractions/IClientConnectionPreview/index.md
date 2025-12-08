#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IClientConnectionPreview Interface

Read\-only preview of a client connection for making routing decisions without consuming data\.

```csharp
public interface IClientConnectionPreview : System.IAsyncDisposable
```

Implements [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Connection previews allow backends to inspect connection properties and peek at incoming data
for routing decisions\. The preview uses a pipeline reader that supports examining buffered data
without consuming it\. Once routing completes, the actual [IClientConnection](../IClientConnection/index.md 'Pmmux\.Abstractions\.IClientConnection') is used
for data transfer with all original data intact\.

| Properties | |
| :--- | :--- |
| [Client](Client.md 'Pmmux\.Abstractions\.IClientConnectionPreview\.Client') | Client associated with this connection\. |
| [Ingress](Ingress.md 'Pmmux\.Abstractions\.IClientConnectionPreview\.Ingress') | Pipeline reader for peeking at incoming data\. |
| [Properties](Properties.md 'Pmmux\.Abstractions\.IClientConnectionPreview\.Properties') | Metadata associated with the connection\. |
