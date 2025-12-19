#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## CompactConfig Class

Utility for parsing compact configuration strings\.

```csharp
public abstract record CompactConfig : System.IEquatable<Pmmux.Abstractions.CompactConfig>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; CompactConfig

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[CompactConfig](index.md 'Pmmux\.Abstractions\.CompactConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

### Example
Parsing a backend configuration string:

```csharp
var input = "web:pass:target.ip=127.0.0.1,target.port=3000";
var segments = CompactConfig.Parse(input);
// Returns:
// IdentifierSegment("web"),
// IdentifierSegment("pass"),
// PropertiesSegment({ ["target.ip"] = "127.0.0.1", ["target.port"] = "3000" })
```

### Remarks

Format: segments separated by colons, where each segment is either an identifier
or comma-delimited key=value properties. Values with special characters can be quoted.

| Methods | |
| :--- | :--- |
| [Parse\(string\)](Parse(string).md 'Pmmux\.Abstractions\.CompactConfig\.Parse\(string\)') | Parse compact configuration string into a sequence of segments\. |
