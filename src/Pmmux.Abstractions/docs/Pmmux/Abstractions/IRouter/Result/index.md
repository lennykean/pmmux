#### [Pmmux\.Abstractions](../../../../index.md 'index')
### [Pmmux\.Abstractions](../../index.md 'Pmmux\.Abstractions').[IRouter](../index.md 'Pmmux\.Abstractions\.IRouter')

## IRouter\.Result Class

Result of a routing operation\.

```csharp
public record IRouter.Result : System.IEquatable<Pmmux.Abstractions.IRouter.Result>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; Result

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[Result](index.md 'Pmmux\.Abstractions\.IRouter\.Result')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [Result\(bool, BackendInfo, string\)](Result(bool,BackendInfo,string).md 'Pmmux\.Abstractions\.IRouter\.Result\.Result\(bool, Pmmux\.Abstractions\.BackendInfo, string\)') | Result of a routing operation\. |

| Properties | |
| :--- | :--- |
| [Backend](Backend.md 'Pmmux\.Abstractions\.IRouter\.Result\.Backend') | The selected backend if successful\. |
| [Reason](Reason.md 'Pmmux\.Abstractions\.IRouter\.Result\.Reason') | The failure reason if unsuccessful\. |
| [Success](Success.md 'Pmmux\.Abstractions\.IRouter\.Result\.Success') | `true` if routing succeeded, otherwise `false`\. |

| Methods | |
| :--- | :--- |
| [Failed\(string\)](Failed(string).md 'Pmmux\.Abstractions\.IRouter\.Result\.Failed\(string\)') | Create a failed routing result\. |
| [Succeeded\(BackendInfo\)](Succeeded(BackendInfo).md 'Pmmux\.Abstractions\.IRouter\.Result\.Succeeded\(Pmmux\.Abstractions\.BackendInfo\)') | Create a successful routing result\. |
