#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[BackendInfo](index.md 'Pmmux\.Abstractions\.BackendInfo')

## BackendInfo\(BackendSpec, IDictionary\<string,string\>, PriorityTier\) Constructor

```csharp
public BackendInfo(Pmmux.Abstractions.BackendSpec spec, System.Collections.Generic.IDictionary<string,string> properties, Pmmux.Abstractions.PriorityTier priorityTier=Pmmux.Abstractions.PriorityTier.Normal);
```
#### Parameters

<a name='Pmmux.Abstractions.BackendInfo.BackendInfo(Pmmux.Abstractions.BackendSpec,System.Collections.Generic.IDictionary_string,string_,Pmmux.Abstractions.PriorityTier).spec'></a>

`spec` [BackendSpec](../BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec')

The specification that defines the backend\.

<a name='Pmmux.Abstractions.BackendInfo.BackendInfo(Pmmux.Abstractions.BackendSpec,System.Collections.Generic.IDictionary_string,string_,Pmmux.Abstractions.PriorityTier).properties'></a>

`properties` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

Runtime properties provided by the backend implementation\.

<a name='Pmmux.Abstractions.BackendInfo.BackendInfo(Pmmux.Abstractions.BackendSpec,System.Collections.Generic.IDictionary_string,string_,Pmmux.Abstractions.PriorityTier).priorityTier'></a>

`priorityTier` [PriorityTier](../PriorityTier/index.md 'Pmmux\.Abstractions\.PriorityTier')

The routing priority tier\.