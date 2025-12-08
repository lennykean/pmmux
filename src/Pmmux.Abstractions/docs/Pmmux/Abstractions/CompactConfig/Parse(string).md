#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[CompactConfig](index.md 'Pmmux\.Abstractions\.CompactConfig')

## CompactConfig\.Parse\(string\) Method

Parse compact configuration string into a sequence of segments\.

```csharp
public static System.Collections.Generic.IEnumerable<Pmmux.Abstractions.CompactConfig.Segment> Parse(string compactConfig);
```
#### Parameters

<a name='Pmmux.Abstractions.CompactConfig.Parse(string).compactConfig'></a>

`compactConfig` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The compact configuration string to parse\.

#### Returns
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Segment](Segment/index.md 'Pmmux\.Abstractions\.CompactConfig\.Segment')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')  
Enumerable of [Segment](Segment/index.md 'Pmmux\.Abstractions\.CompactConfig\.Segment') objects representing the parsed configuration\.