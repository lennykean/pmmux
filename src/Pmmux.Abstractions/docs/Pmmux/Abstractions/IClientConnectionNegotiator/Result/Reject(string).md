#### [Pmmux\.Abstractions](../../../../index.md 'index')
### [Pmmux\.Abstractions](../../index.md 'Pmmux\.Abstractions').[IClientConnectionNegotiator](../index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator').[Result](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result')

## IClientConnectionNegotiator\.Result\.Reject\(string\) Method

Create a failed negotiation result with the specified reason\.

```csharp
public static Pmmux.Abstractions.IClientConnectionNegotiator.Result Reject(string reason);
```
#### Parameters

<a name='Pmmux.Abstractions.IClientConnectionNegotiator.Result.Reject(string).reason'></a>

`reason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason why the connection was rejected or could not be negotiated\.

#### Returns
[Result](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result')  
A [Result](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result') indicating the negotiation failed\.