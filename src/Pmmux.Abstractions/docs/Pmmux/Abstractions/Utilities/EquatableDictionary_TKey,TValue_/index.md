#### [Pmmux\.Abstractions](../../../../index.md 'index')
### [Pmmux\.Abstractions\.Utilities](../index.md 'Pmmux\.Abstractions\.Utilities')

## EquatableDictionary\<TKey,TValue\> Class

A read\-only dictionary that provides value\-based equality comparison\.

```csharp
public sealed class EquatableDictionary<TKey,TValue> : System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>
    where TKey : System.IComparable<TKey>
```
#### Type parameters

<a name='Pmmux.Abstractions.Utilities.EquatableDictionary_TKey,TValue_.TKey'></a>

`TKey`

<a name='Pmmux.Abstractions.Utilities.EquatableDictionary_TKey,TValue_.TValue'></a>

`TValue`

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; [System\.Collections\.ObjectModel\.ReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2 'System\.Collections\.ObjectModel\.ReadOnlyDictionary\`2')[TKey](index.md#Pmmux.Abstractions.Utilities.EquatableDictionary_TKey,TValue_.TKey 'Pmmux\.Abstractions\.Utilities\.EquatableDictionary\<TKey,TValue\>\.TKey')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2 'System\.Collections\.ObjectModel\.ReadOnlyDictionary\`2')[TValue](index.md#Pmmux.Abstractions.Utilities.EquatableDictionary_TKey,TValue_.TValue 'Pmmux\.Abstractions\.Utilities\.EquatableDictionary\<TKey,TValue\>\.TValue')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2 'System\.Collections\.ObjectModel\.ReadOnlyDictionary\`2') &#129106; EquatableDictionary\<TKey,TValue\>

| Constructors | |
| :--- | :--- |
| [EquatableDictionary\(IDictionary&lt;TKey,TValue&gt;\)](EquatableDictionary(IDictionary_TKey,TValue_).md 'Pmmux\.Abstractions\.Utilities\.EquatableDictionary\<TKey,TValue\>\.EquatableDictionary\(System\.Collections\.Generic\.IDictionary\<TKey,TValue\>\)') | A read\-only dictionary that provides value\-based equality comparison\. |

| Methods | |
| :--- | :--- |
| [Equals\(object\)](Equals(object).md 'Pmmux\.Abstractions\.Utilities\.EquatableDictionary\<TKey,TValue\>\.Equals\(object\)') | Determines whether the specified object is equal to the current object\. |
| [GetHashCode\(\)](GetHashCode().md 'Pmmux\.Abstractions\.Utilities\.EquatableDictionary\<TKey,TValue\>\.GetHashCode\(\)') | Serves as the default hash function\. |
