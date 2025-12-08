#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IClientConnection Interface

Established client connection ready for routing and data transfer\.

```csharp
public interface IClientConnection : Pmmux.Abstractions.IConnection, System.IAsyncDisposable
```

Implements [IConnection](../IConnection/index.md 'Pmmux\.Abstractions\.IConnection'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Client connections are created through [IClientConnectionNegotiator](../IClientConnectionNegotiator/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator') implementations
that perform protocol negotiation \(e\.g\., TLS handshake\)\.

| Properties | |
| :--- | :--- |
| [Client](Client.md 'Pmmux\.Abstractions\.IClientConnection\.Client') | Client associated with the connection\. |

| Methods | |
| :--- | :--- |
| [Preview\(\)](Preview().md 'Pmmux\.Abstractions\.IClientConnection\.Preview\(\)') | Create a preview of the connection for peeking at incoming data without consuming it\. |
