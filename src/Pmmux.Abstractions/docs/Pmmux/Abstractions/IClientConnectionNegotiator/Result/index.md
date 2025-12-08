#### [Pmmux\.Abstractions](../../../../index.md 'index')
### [Pmmux\.Abstractions](../../index.md 'Pmmux\.Abstractions').[IClientConnectionNegotiator](../index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator')

## IClientConnectionNegotiator\.Result Class

Result of a connection negotiation attempt\.

```csharp
public record IClientConnectionNegotiator.Result : System.IEquatable<Pmmux.Abstractions.IClientConnectionNegotiator.Result>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; Result

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[Result](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [Result\(bool, IClientConnection, string\)](Result(bool,IClientConnection,string).md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result\.Result\(bool, Pmmux\.Abstractions\.IClientConnection, string\)') | Result of a connection negotiation attempt\. |

| Properties | |
| :--- | :--- |
| [ClientConnection](ClientConnection.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result\.ClientConnection') | The negotiated client connection if successful; otherwise, `null`\. |
| [Reason](Reason.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result\.Reason') | The rejection reason if unsuccessful; otherwise, `null`\. |
| [Success](Success.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result\.Success') | `true` if negotiation succeeded; otherwise, `false`\. |

| Methods | |
| :--- | :--- |
| [Accept\(IClientConnection\)](Accept(IClientConnection).md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result\.Accept\(Pmmux\.Abstractions\.IClientConnection\)') | Create a successful negotiation result with the specified client connection\. |
| [Reject\(string\)](Reject(string).md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result\.Reject\(string\)') | Create a failed negotiation result with the specified reason\. |
