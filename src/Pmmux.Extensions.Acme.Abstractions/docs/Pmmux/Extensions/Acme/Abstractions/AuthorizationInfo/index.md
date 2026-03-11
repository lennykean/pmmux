#### [Pmmux\.Extensions\.Acme\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Acme\.Abstractions](../index.md 'Pmmux\.Extensions\.Acme\.Abstractions')

## AuthorizationInfo Class

An ACME authorization for a domain\.

```csharp
public record AuthorizationInfo : System.IEquatable<Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; AuthorizationInfo

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[AuthorizationInfo](index.md 'Pmmux\.Extensions\.Acme\.Abstractions\.AuthorizationInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [AuthorizationInfo\(string, IChallengeContext\)](AuthorizationInfo(string,IChallengeContext).md 'Pmmux\.Extensions\.Acme\.Abstractions\.AuthorizationInfo\.AuthorizationInfo\(string, Certes\.Acme\.IChallengeContext\)') | An ACME authorization for a domain\. |

| Properties | |
| :--- | :--- |
| [Challenge](Challenge.md 'Pmmux\.Extensions\.Acme\.Abstractions\.AuthorizationInfo\.Challenge') | The challenge context chosen for validation\. |
| [Domain](Domain.md 'Pmmux\.Extensions\.Acme\.Abstractions\.AuthorizationInfo\.Domain') | The domain that the authorization covers\. |
