#### [Pmmux\.Abstractions](../../../../index.md 'index')
### [Pmmux\.Abstractions](../../index.md 'Pmmux\.Abstractions').[IClientConnectionNegotiator](../index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator').[Result](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result')

## IClientConnectionNegotiator\.Result\.Accept\(IClientConnection\) Method

Create a successful negotiation result with the specified client connection\.

```csharp
public static Pmmux.Abstractions.IClientConnectionNegotiator.Result Accept(Pmmux.Abstractions.IClientConnection clientConnection);
```
#### Parameters

<a name='Pmmux.Abstractions.IClientConnectionNegotiator.Result.Accept(Pmmux.Abstractions.IClientConnection).clientConnection'></a>

`clientConnection` [IClientConnection](../../IClientConnection/index.md 'Pmmux\.Abstractions\.IClientConnection')

The established and negotiated client connection\.

#### Returns
[Result](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result')  
A [Result](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result') indicating successful negotiation\.