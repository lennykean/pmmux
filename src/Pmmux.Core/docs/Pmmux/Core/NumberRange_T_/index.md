#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## NumberRange\<T\> Class

A range of numbers\.

```csharp
public record NumberRange<T> : System.IEquatable<Pmmux.Core.NumberRange<T>>
    where T : System.Numerics.INumber<T>
```
#### Type parameters

<a name='Pmmux.Core.NumberRange_T_.T'></a>

`T`

The type of the numbers in the range\.

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; NumberRange\<T\>

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[Pmmux\.Core\.NumberRange&lt;](index.md 'Pmmux\.Core\.NumberRange\<T\>')[T](index.md#Pmmux.Core.NumberRange_T_.T 'Pmmux\.Core\.NumberRange\<T\>\.T')[&gt;](index.md 'Pmmux\.Core\.NumberRange\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

### Remarks
The range is inclusive of the start and end values\.

| Constructors | |
| :--- | :--- |
| [NumberRange\(T, T\)](NumberRange(T,T).md 'Pmmux\.Core\.NumberRange\<T\>\.NumberRange\(T, T\)') | A range of numbers\. |

| Properties | |
| :--- | :--- |
| [End](End.md 'Pmmux\.Core\.NumberRange\<T\>\.End') | The end of the range\. |
| [Start](Start.md 'Pmmux\.Core\.NumberRange\<T\>\.Start') | The start of the range\. |

| Methods | |
| :--- | :--- |
| [Contains\(T\)](Contains(T).md 'Pmmux\.Core\.NumberRange\<T\>\.Contains\(T\)') | Checks if the given value is within the range\. |
