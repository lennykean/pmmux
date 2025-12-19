#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[MatcherExtensions](index.md 'Pmmux\.Core\.MatcherExtensions')

## MatcherExtensions\.AsCidr Method

| Overloads | |
| :--- | :--- |
| [AsCidr\(this Matcher&lt;string&gt;\)](AsCidr.md#Pmmux.Core.MatcherExtensions.AsCidr(thisPmmux.Core.Matcher_string_) 'Pmmux\.Core\.MatcherExtensions\.AsCidr\(this Pmmux\.Core\.Matcher\<string\>\)') | Parses a string matcher into a CIDR matcher\. |
| [AsCidr\(this IEnumerable&lt;Matcher&lt;string&gt;&gt;\)](AsCidr.md#Pmmux.Core.MatcherExtensions.AsCidr(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__) 'Pmmux\.Core\.MatcherExtensions\.AsCidr\(this System\.Collections\.Generic\.IEnumerable\<Pmmux\.Core\.Matcher\<string\>\>\)') | Parse a collection of string matchers into CIDR matchers\. |
| [AsCidr\(this IReadOnlyDictionary&lt;string,Matcher&lt;string&gt;&gt;\)](AsCidr.md#Pmmux.Core.MatcherExtensions.AsCidr(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__) 'Pmmux\.Core\.MatcherExtensions\.AsCidr\(this System\.Collections\.Generic\.IReadOnlyDictionary\<string,Pmmux\.Core\.Matcher\<string\>\>\)') | Parse a dictionary of string matchers into CIDR matchers\. |

<a name='Pmmux.Core.MatcherExtensions.AsCidr(thisPmmux.Core.Matcher_string_)'></a>

## MatcherExtensions\.AsCidr\(this Matcher\<string\>\) Method

Parses a string matcher into a CIDR matcher\.

```csharp
public static Pmmux.Core.Matcher<System.Net.IPNetwork> AsCidr(this Pmmux.Core.Matcher<string> matcher);
```
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsCidr(thisPmmux.Core.Matcher_string_).matcher'></a>

`matcher` [Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')

The matcher to parse\.

#### Returns
[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.Net\.IPNetwork](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipnetwork 'System\.Net\.IPNetwork')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')  
A CIDR matcher\.

<a name='Pmmux.Core.MatcherExtensions.AsCidr(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__)'></a>

## MatcherExtensions\.AsCidr\(this IEnumerable\<Matcher\<string\>\>\) Method

Parse a collection of string matchers into CIDR matchers\.

```csharp
public static System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<System.Net.IPNetwork>> AsCidr(this System.Collections.Generic.IEnumerable<Pmmux.Core.Matcher<string>> matchers);
```
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsCidr(thisSystem.Collections.Generic.IEnumerable_Pmmux.Core.Matcher_string__).matchers'></a>

`matchers` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

#### Returns
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.Net\.IPNetwork](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipnetwork 'System\.Net\.IPNetwork')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Pmmux.Core.MatcherExtensions.AsCidr(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__)'></a>

## MatcherExtensions\.AsCidr\(this IReadOnlyDictionary\<string,Matcher\<string\>\>\) Method

Parse a dictionary of string matchers into CIDR matchers\.

```csharp
public static System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<System.Net.IPNetwork>> AsCidr(this System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<string>> matchers);
```
#### Parameters

<a name='Pmmux.Core.MatcherExtensions.AsCidr(thisSystem.Collections.Generic.IReadOnlyDictionary_string,Pmmux.Core.Matcher_string__).matchers'></a>

`matchers` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

#### Returns
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.Net\.IPNetwork](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipnetwork 'System\.Net\.IPNetwork')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')