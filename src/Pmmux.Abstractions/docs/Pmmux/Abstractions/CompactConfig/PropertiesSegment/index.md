#### [Pmmux\.Abstractions](../../../../index.md 'index')
### [Pmmux\.Abstractions](../../index.md 'Pmmux\.Abstractions').[CompactConfig](../index.md 'Pmmux\.Abstractions\.CompactConfig')

## CompactConfig\.PropertiesSegment Class

Properties segment \(key\-value pairs\) in the configuration\.

```csharp
public record CompactConfig.PropertiesSegment : Pmmux.Abstractions.CompactConfig.Segment, System.IEquatable<Pmmux.Abstractions.CompactConfig.PropertiesSegment>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; [Segment](../Segment/index.md 'Pmmux\.Abstractions\.CompactConfig\.Segment') &#129106; PropertiesSegment

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[PropertiesSegment](index.md 'Pmmux\.Abstractions\.CompactConfig\.PropertiesSegment')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [PropertiesSegment\(Dictionary&lt;string,string&gt;\)](PropertiesSegment(Dictionary_string,string_).md 'Pmmux\.Abstractions\.CompactConfig\.PropertiesSegment\.PropertiesSegment\(System\.Collections\.Generic\.Dictionary\<string,string\>\)') | Properties segment \(key\-value pairs\) in the configuration\. |

| Properties | |
| :--- | :--- |
| [Properties](Properties.md 'Pmmux\.Abstractions\.CompactConfig\.PropertiesSegment\.Properties') | The properties dictionary\. |
