#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[MatcherExtensions](index.md 'Pmmux\.Core\.MatcherExtensions')

## MatcherExtensions\.HasMatch Method

| Overloads | |
| :--- | :--- |
| [HasMatch&lt;T&gt;\(this IEnumerable&lt;Matcher&lt;T&gt;&gt;, Func&lt;T,bool&gt;\)](HasMatch.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T__,System.Func_T,bool_) 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<T\>\>, System\.Func\<T,bool\>\)') | Check if any matcher in the collection matches using the provided comparer\. |
| [HasMatch&lt;T&gt;\(this IReadOnlyDictionary&lt;string,Matcher&lt;T&gt;&gt;, string, Func&lt;T,bool&gt;\)](HasMatch.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_T__,string,System.Func_T,bool_) 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,Pmmux\.Core\.Matcher\<T\>\>, string, System\.Func\<T,bool\>\)') | Check if the indexed matcher matches using the provided comparer\. |
| [HasMatch&lt;T&gt;\(this IReadOnlyDictionary&lt;string,IEnumerable&lt;Matcher&lt;T&gt;&gt;&gt;, string, Func&lt;T,bool&gt;\)](HasMatch.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,System.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T___,string,System.Func_T,bool_) 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<T\>\>\>, string, System\.Func\<T,bool\>\)') | Check if any matcher at the indexed key matches using the provided comparer\. |

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T__,System.Func_T,bool_)'></a>

## MatcherExtensions\.HasMatch\<T\>\(this IEnumerable\<Matcher\<T\>\>, Func\<T,bool\>\) Method

Check if any matcher in the collection matches using the provided comparer\.

```csharp
public static bool HasMatch<T>(this System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<T>> matchers, System.Func<T,bool> comparer);
```
#### Type parameters

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T__,System.Func_T,bool_).T'></a>

`T`

The type of the matcher value\.
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T__,System.Func_T,bool_).matchers'></a>

`matchers` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[T](index.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T__,System.Func_T,bool_).T 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<T\>\>, System\.Func\<T,bool\>\)\.T')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The matchers to check\.

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T__,System.Func_T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](index.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T__,System.Func_T,bool_).T 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<T\>\>, System\.Func\<T,bool\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The comparison function\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the matcher collection passes; otherwise, `false`\.

### Remarks
Matchers match if any value matches \(logical OR\)\.
Negated matchers match only if none of the values match \(logical AND\)\.
Empty matcher collection always returns `true` \(no constraints to fail\)\.

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_T__,string,System.Func_T,bool_)'></a>

## MatcherExtensions\.HasMatch\<T\>\(this IReadOnlyDictionary\<string,Matcher\<T\>\>, string, Func\<T,bool\>\) Method

Check if the indexed matcher matches using the provided comparer\.

```csharp
public static bool HasMatch<T>(this System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<T>> matchers, string indexer, System.Func<T,bool> comparer);
```
#### Type parameters

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_T__,string,System.Func_T,bool_).T'></a>

`T`

The type of the matcher value\.
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_T__,string,System.Func_T,bool_).matchers'></a>

`matchers` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[T](index.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_T__,string,System.Func_T,bool_).T 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,Pmmux\.Core\.Matcher\<T\>\>, string, System\.Func\<T,bool\>\)\.T')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The dictionary of matchers\.

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_T__,string,System.Func_T,bool_).indexer'></a>

`indexer` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The key to look up\.

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_T__,string,System.Func_T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](index.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_T__,string,System.Func_T,bool_).T 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,Pmmux\.Core\.Matcher\<T\>\>, string, System\.Func\<T,bool\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The comparison function\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the key exists and the matcher passes; otherwise, `false`\.

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,System.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T___,string,System.Func_T,bool_)'></a>

## MatcherExtensions\.HasMatch\<T\>\(this IReadOnlyDictionary\<string,IEnumerable\<Matcher\<T\>\>\>, string, Func\<T,bool\>\) Method

Check if any matcher at the indexed key matches using the provided comparer\.

```csharp
public static bool HasMatch<T>(this System.Collections.Generic.IReadOnlyDictionary<string,System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<T>>> matchers, string indexer, System.Func<T,bool> comparer);
```
#### Type parameters

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,System.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T___,string,System.Func_T,bool_).T'></a>

`T`

The type of the matcher value\.
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,System.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T___,string,System.Func_T,bool_).matchers'></a>

`matchers` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[T](index.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,System.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T___,string,System.Func_T,bool_).T 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<T\>\>\>, string, System\.Func\<T,bool\>\)\.T')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The dictionary of matcher collections\.

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,System.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T___,string,System.Func_T,bool_).indexer'></a>

`indexer` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The key to look up\.

<a name='Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,System.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T___,string,System.Func_T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](index.md#Pmmux.Core.MatcherExtensions.HasMatch_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,System.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_T___,string,System.Func_T,bool_).T 'Pmmux\.Core\.MatcherExtensions\.HasMatch\<T\>\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<T\>\>\>, string, System\.Func\<T,bool\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The comparison function\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the key exists and any matcher passes; otherwise, `false`\.