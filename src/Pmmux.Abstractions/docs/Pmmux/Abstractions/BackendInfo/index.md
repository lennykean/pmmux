#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## BackendInfo Class

Represents runtime information about an active backend instance\.

```csharp
public record BackendInfo : System.IEquatable<Pmmux.Abstractions.BackendInfo>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BackendInfo

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[BackendInfo](index.md 'Pmmux\.Abstractions\.BackendInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

### Remarks
BackendInfo combines the backend's configuration \([Spec](Spec.md 'Pmmux\.Abstractions\.BackendInfo\.Spec')\) with runtime properties
and routing metadata\.

| Constructors | |
| :--- | :--- |
| [BackendInfo\(BackendSpec, IDictionary&lt;string,string&gt;, PriorityTier\)](BackendInfo(BackendSpec,IDictionary_string,string_,PriorityTier).md 'Pmmux\.Abstractions\.BackendInfo\.BackendInfo\(Pmmux\.Abstractions\.BackendSpec, System\.Collections\.Generic\.IDictionary\<string,string\>, Pmmux\.Abstractions\.PriorityTier\)') | |

| Properties | |
| :--- | :--- |
| [PriorityTier](PriorityTier.md 'Pmmux\.Abstractions\.BackendInfo\.PriorityTier') | The routing priority tier of the backend\. |
| [Properties](Properties.md 'Pmmux\.Abstractions\.BackendInfo\.Properties') | Runtime properties provided by the backend implementation\. |
| [Spec](Spec.md 'Pmmux\.Abstractions\.BackendInfo\.Spec') | The specification that defines the backend\. |
