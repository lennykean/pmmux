#### [Pmmux\.Abstractions](../../../../index.md 'index')
### [Pmmux\.Abstractions](../../index.md 'Pmmux\.Abstractions').[IClientConnectionNegotiator](../index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator').[Result](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result')

## Result\(bool, IClientConnection, string\) Constructor

Result of a connection negotiation attempt\.

```csharp
public Result(bool Success, Pmmux.Abstractions.IClientConnection? ClientConnection, string? Reason);
```
#### Parameters

<a name='Pmmux.Abstractions.IClientConnectionNegotiator.Result.Result(bool,Pmmux.Abstractions.IClientConnection,string).Success'></a>

`Success` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` if negotiation succeeded; otherwise, `false`\.

<a name='Pmmux.Abstractions.IClientConnectionNegotiator.Result.Result(bool,Pmmux.Abstractions.IClientConnection,string).ClientConnection'></a>

`ClientConnection` [IClientConnection](../../IClientConnection/index.md 'Pmmux\.Abstractions\.IClientConnection')

The negotiated client connection if successful; otherwise, `null`\.

<a name='Pmmux.Abstractions.IClientConnectionNegotiator.Result.Result(bool,Pmmux.Abstractions.IClientConnection,string).Reason'></a>

`Reason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The rejection reason if unsuccessful; otherwise, `null`\.