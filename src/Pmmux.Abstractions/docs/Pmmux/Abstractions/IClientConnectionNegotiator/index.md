#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IClientConnectionNegotiator Interface

Negotiates client connections before routing using a chain\-of\-responsibility pattern\.

```csharp
public interface IClientConnectionNegotiator
```

### Remarks
Connection negotiators intercept raw socket connections for transport\-level processing such as
encryption, handshakes, authentication, or packet filtering\. Multiple negotiators can be chained
together, each deciding whether to handle the connection or pass it to the next negotiator\.

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Name') | The name of the connection negotiator\. |

| Methods | |
| :--- | :--- |
| [NegotiateAsync\(ClientConnectionContext, Func&lt;Task&lt;Result&gt;&gt;, CancellationToken\)](NegotiateAsync(ClientConnectionContext,Func_Task_Result__,CancellationToken).md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.NegotiateAsync\(Pmmux\.Abstractions\.ClientConnectionContext, System\.Func\<System\.Threading\.Tasks\.Task\<Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result\>\>, System\.Threading\.CancellationToken\)') | Negotiate a client connection, performing any required transport\-level processing\. |
