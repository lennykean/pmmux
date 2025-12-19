#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## Matcher\<T\> Class

A matcher for a backend parameter value\.

```csharp
public record Matcher<T> : System.IEquatable<Pmmux.Core.Matcher<T>>
```
#### Type parameters

<a name='Pmmux.Core.Matcher_T_.T'></a>

`T`

The type of the value to match\.

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; Matcher\<T\>

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[Pmmux\.Core\.Matcher&lt;](index.md 'Pmmux\.Core\.Matcher\<T\>')[T](index.md#Pmmux.Core.Matcher_T_.T 'Pmmux\.Core\.Matcher\<T\>\.T')[&gt;](index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [Matcher\(bool, T\)](Matcher(bool,T).md 'Pmmux\.Core\.Matcher\<T\>\.Matcher\(bool, T\)') | A matcher for a backend parameter value\. |

| Properties | |
| :--- | :--- |
| [Negate](Negate.md 'Pmmux\.Core\.Matcher\<T\>\.Negate') | Whether to negate the matcher\. |
| [Value](Value.md 'Pmmux\.Core\.Matcher\<T\>\.Value') | The value to match\. |
