#### [Pmmux\.Extensions\.Acme\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Acme\.Abstractions](../index.md 'Pmmux\.Extensions\.Acme\.Abstractions').[AuthorizationInfo](index.md 'Pmmux\.Extensions\.Acme\.Abstractions\.AuthorizationInfo')

## AuthorizationInfo\(string, IChallengeContext\) Constructor

An ACME authorization for a domain\.

```csharp
public AuthorizationInfo(string Domain, Certes.Acme.IChallengeContext Challenge);
```
#### Parameters

<a name='Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo.AuthorizationInfo(string,Certes.Acme.IChallengeContext).Domain'></a>

`Domain` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The domain that the authorization covers\.

<a name='Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo.AuthorizationInfo(string,Certes.Acme.IChallengeContext).Challenge'></a>

`Challenge` [Certes\.Acme\.IChallengeContext](https://learn.microsoft.com/en-us/dotnet/api/certes.acme.ichallengecontext 'Certes\.Acme\.IChallengeContext')

The challenge context chosen for validation\.