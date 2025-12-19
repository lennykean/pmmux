#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[MatcherExtensions](index.md 'Pmmux\.Core\.MatcherExtensions')

## MatcherExtensions\.AsNumeric Method

| Overloads | |
| :--- | :--- |
| [AsNumeric&lt;T&gt;\(this Matcher&lt;string&gt;\)](AsNumeric.md#Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisPmmux.Core.Matcher_string_) 'Pmmux\.Core\.MatcherExtensions\.AsNumeric\<T\>\(this Pmmux\.Core\.Matcher\<string\>\)') | Parses a string matcher into a numeric range matcher\. |
| [AsNumeric&lt;T&gt;\(this IEnumerable&lt;Matcher&lt;string&gt;&gt;\)](AsNumeric.md#Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__) 'Pmmux\.Core\.MatcherExtensions\.AsNumeric\<T\>\(this System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<string\>\>\)') | Parse a collection of string matchers into numeric range matchers\. |
| [AsNumeric&lt;T&gt;\(this IReadOnlyDictionary&lt;string,Matcher&lt;string&gt;&gt;\)](AsNumeric.md#Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__) 'Pmmux\.Core\.MatcherExtensions\.AsNumeric\<T\>\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,Pmmux\.Core\.Matcher\<string\>\>\)') | Parse a dictionary of string matchers into numeric range matchers\. |

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisPmmux.Core.Matcher_string_)'></a>

## MatcherExtensions\.AsNumeric\<T\>\(this Matcher\<string\>\) Method

Parses a string matcher into a numeric range matcher\.

```csharp
public static Pmmux.Core.Matcher<Pmmux.Core.NumberRange<T>> AsNumeric<T>(this Pmmux.Core.Matcher<string> matcher)
    where T : System.Numerics.INumber<T>;
```
#### Type parameters

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisPmmux.Core.Matcher_string_).T'></a>

`T`

The type of the numbers in the range\.
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisPmmux.Core.Matcher_string_).matcher'></a>

`matcher` [Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')

The matcher to parse\.

#### Returns
[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[Pmmux\.Core\.NumberRange&lt;](../NumberRange_T_/index.md 'Pmmux\.Core\.NumberRange\<T\>')[T](index.md#Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisPmmux.Core.Matcher_string_).T 'Pmmux\.Core\.MatcherExtensions\.AsNumeric\<T\>\(this Pmmux\.Core\.Matcher\<string\>\)\.T')[&gt;](../NumberRange_T_/index.md 'Pmmux\.Core\.NumberRange\<T\>')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')  
A number range matcher\.

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__)'></a>

## MatcherExtensions\.AsNumeric\<T\>\(this IEnumerable\<Matcher\<string\>\>\) Method

Parse a collection of string matchers into numeric range matchers\.

```csharp
public static System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<Pmmux.Core.NumberRange<T>>> AsNumeric<T>(this System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<string>> matchers)
    where T : System.Numerics.INumber<T>;
```
#### Type parameters

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__).T'></a>

`T`
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__).matchers'></a>

`matchers` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

#### Returns
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[Pmmux\.Core\.NumberRange&lt;](../NumberRange_T_/index.md 'Pmmux\.Core\.NumberRange\<T\>')[T](index.md#Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__).T 'Pmmux\.Core\.MatcherExtensions\.AsNumeric\<T\>\(this System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<string\>\>\)\.T')[&gt;](../NumberRange_T_/index.md 'Pmmux\.Core\.NumberRange\<T\>')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__)'></a>

## MatcherExtensions\.AsNumeric\<T\>\(this IReadOnlyDictionary\<string,Matcher\<string\>\>\) Method

Parse a dictionary of string matchers into numeric range matchers\.

```csharp
public static System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<Pmmux.Core.NumberRange<T>>> AsNumeric<T>(this System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<string>> matchers)
    where T : System.Numerics.INumber<T>;
```
#### Type parameters

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__).T'></a>

`T`
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__).matchers'></a>

`matchers` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

#### Returns
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[Pmmux\.Core\.NumberRange&lt;](../NumberRange_T_/index.md 'Pmmux\.Core\.NumberRange\<T\>')[T](index.md#Pmmux.Core.MatcherExtensions.AsNumeric_T_(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__).T 'Pmmux\.Core\.MatcherExtensions\.AsNumeric\<T\>\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,Pmmux\.Core\.Matcher\<string\>\>\)\.T')[&gt;](../NumberRange_T_/index.md 'Pmmux\.Core\.NumberRange\<T\>')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')