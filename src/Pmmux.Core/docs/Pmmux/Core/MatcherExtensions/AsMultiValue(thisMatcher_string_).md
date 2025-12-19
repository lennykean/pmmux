#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[MatcherExtensions](index.md 'Pmmux\.Core\.MatcherExtensions')

## MatcherExtensions\.AsMultiValue\(this Matcher\<string\>\) Method

Parses a string matcher into an array of string matchers\.

```csharp
public static System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<string>> AsMultiValue(this Pmmux.Core.Matcher<string> matcher);
```
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsMultiValue(thisPmmux.Core.Matcher_string_).matcher'></a>

`matcher` [Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')

The matcher to parse\.

#### Returns
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')  
A list of string matchers\.