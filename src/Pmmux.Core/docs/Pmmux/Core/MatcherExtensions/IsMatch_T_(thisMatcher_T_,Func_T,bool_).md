#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[MatcherExtensions](index.md 'Pmmux\.Core\.MatcherExtensions')

## MatcherExtensions\.IsMatch\<T\>\(this Matcher\<T\>, Func\<T,bool\>\) Method

Check if the matcher matches using the provided comparer\.

```csharp
public static bool IsMatch<T>(this Pmmux.Core.Matcher<T> matchers, System.Func<T,bool> comparer);
```
#### Type parameters

<a name='Pmmux.Core.MatcherExtensions.IsMatch_T_(thisPmmux.Core.Matcher_T_,System.Func_T,bool_).T'></a>

`T`

The type of the matcher value\.
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.IsMatch_T_(thisPmmux.Core.Matcher_T_,System.Func_T,bool_).matchers'></a>

`matchers` [Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[T](IsMatch_T_(thisMatcher_T_,Func_T,bool_).md#Pmmux.Core.MatcherExtensions.IsMatch_T_(thisPmmux.Core.Matcher_T_,System.Func_T,bool_).T 'Pmmux\.Core\.MatcherExtensions\.IsMatch\<T\>\(this Pmmux\.Core\.Matcher\<T\>, System\.Func\<T,bool\>\)\.T')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')

The matcher to check\.

<a name='Pmmux.Core.MatcherExtensions.IsMatch_T_(thisPmmux.Core.Matcher_T_,System.Func_T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IsMatch_T_(thisMatcher_T_,Func_T,bool_).md#Pmmux.Core.MatcherExtensions.IsMatch_T_(thisPmmux.Core.Matcher_T_,System.Func_T,bool_).T 'Pmmux\.Core\.MatcherExtensions\.IsMatch\<T\>\(this Pmmux\.Core\.Matcher\<T\>, System\.Func\<T,bool\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The comparison function\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the matcher passes; otherwise, `false`\.