#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[BackendSpecExtensions](index.md 'Pmmux\.Core\.BackendSpecExtensions')

## BackendSpecExtensions\.GetMatchers\(this BackendSpec\) Method

Gets the matchers from a backend specification\.

```csharp
public static System.Collections.Generic.IReadOnlyDictionary<string,Pmmux.Core.Matcher<string>> GetMatchers(this Pmmux.Abstractions.BackendSpec spec);
```
#### Parameters

<a name='Pmmux.Core.BackendSpecExtensions.GetMatchers(thisPmmux.Abstractions.BackendSpec).spec'></a>

`spec` [BackendSpec](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The backend specification\.

#### Returns
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[Pmmux\.Core\.Matcher&lt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](../Matcher_T_/index.md 'Pmmux\.Core\.Matcher\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')  
A dictionary of matchers\.