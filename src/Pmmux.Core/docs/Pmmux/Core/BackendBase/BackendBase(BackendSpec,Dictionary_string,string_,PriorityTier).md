#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[BackendBase](index.md 'Pmmux\.Core\.BackendBase')

## BackendBase\(BackendSpec, Dictionary\<string,string\>, PriorityTier\) Constructor

```csharp
public BackendBase(Pmmux.Abstractions.BackendSpec spec, System.Collections.Generic.Dictionary<string,string> properties, Pmmux.Abstractions.PriorityTier defaultPriority);
```
#### Parameters

<a name='Pmmux.Core.BackendBase.BackendBase(Pmmux.Abstractions.BackendSpec,System.Collections.Generic.Dictionary_string,string_,Pmmux.Abstractions.PriorityTier).spec'></a>

`spec` [BackendSpec](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The backend specification\.

<a name='Pmmux.Core.BackendBase.BackendBase(Pmmux.Abstractions.BackendSpec,System.Collections.Generic.Dictionary_string,string_,Pmmux.Abstractions.PriorityTier).properties'></a>

`properties` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Protocol\-specific properties of the backend\.

<a name='Pmmux.Core.BackendBase.BackendBase(Pmmux.Abstractions.BackendSpec,System.Collections.Generic.Dictionary_string,string_,Pmmux.Abstractions.PriorityTier).defaultPriority'></a>

`defaultPriority` [PriorityTier](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/PriorityTier/index.md 'Pmmux\.Abstractions\.PriorityTier')

The default priority tier\.