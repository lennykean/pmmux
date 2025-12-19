#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[MatcherExtensions](index.md 'Pmmux\.Core\.MatcherExtensions')

## MatcherExtensions\.AsIndexed\(this IReadOnlyDictionary\<string,Matcher\<string\>\>, string\) Method

Gets the indexer\-based matchers from a dictionary of matchers\.

```csharp
public static System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<string>> AsIndexed(this System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<string>> matchers, string indexer);
```
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsIndexed(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__,string).matchers'></a>

`matchers` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The dictionary of matchers\.

<a name='Pmmux.Core.MatcherExtensions.AsIndexed(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__,string).indexer'></a>

`indexer` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The indexer to use\.

#### Returns
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')  
A dictionary of indexable matchers\.