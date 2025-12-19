#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[MatcherExtensions](index.md 'Pmmux\.Core\.MatcherExtensions')

## MatcherExtensions\.AsRegex Method

| Overloads | |
| :--- | :--- |
| [AsRegex\(this Matcher&lt;string&gt;\)](AsRegex.md#Pmmux.Core.MatcherExtensions.AsRegex(thisPmmux.Core.Matcher_string_) 'Pmmux\.Core\.MatcherExtensions\.AsRegex\(this Pmmux\.Core\.Matcher\<string\>\)') | Parses a string matcher into a regex matcher\. |
| [AsRegex\(this IEnumerable&lt;Matcher&lt;string&gt;&gt;\)](AsRegex.md#Pmmux.Core.MatcherExtensions.AsRegex(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__) 'Pmmux\.Core\.MatcherExtensions\.AsRegex\(this System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<string\>\>\)') | Parse a collection of string matchers into regex matchers\. |
| [AsRegex\(this IReadOnlyDictionary&lt;string,Matcher&lt;string&gt;&gt;\)](AsRegex.md#Pmmux.Core.MatcherExtensions.AsRegex(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__) 'Pmmux\.Core\.MatcherExtensions\.AsRegex\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,Pmmux\.Core\.Matcher\<string\>\>\)') | Parse a dictionary of string matchers into regex matchers\. |

<a name='Pmmux.Core.MatcherExtensions.AsRegex(thisPmmux.Core.Matcher_string_)'></a>

## MatcherExtensions\.AsRegex\(this Matcher\<string\>\) Method

Parses a string matcher into a regex matcher\.

```csharp
public static Pmmux.Core.Matcher<System.Text.RegularExpressions.Regex> AsRegex(this Pmmux.Core.Matcher<string> matcher);
```
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsRegex(thisPmmux.Core.Matcher_string_).matcher'></a>

`matcher` [Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')

The matcher to parse\.

#### Returns
[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.Text\.RegularExpressions\.Regex](https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex 'System\.Text\.RegularExpressions\.Regex')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')  
A regex matcher\.

<a name='Pmmux.Core.MatcherExtensions.AsRegex(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__)'></a>

## MatcherExtensions\.AsRegex\(this IEnumerable\<Matcher\<string\>\>\) Method

Parse a collection of string matchers into regex matchers\.

```csharp
public static System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<System.Text.RegularExpressions.Regex>> AsRegex(this System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<string>> matchers);
```
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsRegex(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__).matchers'></a>

`matchers` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

#### Returns
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.Text\.RegularExpressions\.Regex](https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex 'System\.Text\.RegularExpressions\.Regex')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Pmmux.Core.MatcherExtensions.AsRegex(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__)'></a>

## MatcherExtensions\.AsRegex\(this IReadOnlyDictionary\<string,Matcher\<string\>\>\) Method

Parse a dictionary of string matchers into regex matchers\.

```csharp
public static System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<System.Text.RegularExpressions.Regex>> AsRegex(this System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<string>> matchers);
```
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsRegex(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__).matchers'></a>

`matchers` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

#### Returns
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.Text\.RegularExpressions\.Regex](https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex 'System\.Text\.RegularExpressions\.Regex')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')