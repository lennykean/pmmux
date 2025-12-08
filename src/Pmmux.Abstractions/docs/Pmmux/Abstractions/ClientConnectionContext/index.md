#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## ClientConnectionContext Class

Context for client connection negotiators, containing connection details and properties\.

```csharp
public record ClientConnectionContext : System.IEquatable<Pmmux.Abstractions.ClientConnectionContext>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; ClientConnectionContext

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ClientConnectionContext](index.md 'Pmmux\.Abstractions\.ClientConnectionContext')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Properties | |
| :--- | :--- |
| [Client](Client.md 'Pmmux\.Abstractions\.ClientConnectionContext\.Client') | Metadata about the client attempting to connect\. |
| [ClientConnection](ClientConnection.md 'Pmmux\.Abstractions\.ClientConnectionContext\.ClientConnection') | The raw socket connection to negotiate\. |
| [ClientConnectionStream](ClientConnectionStream.md 'Pmmux\.Abstractions\.ClientConnectionContext\.ClientConnectionStream') | Data stream for the client connection\. Negotiators can wrap and replace the stream to apply transformations\. |
| [Properties](Properties.md 'Pmmux\.Abstractions\.ClientConnectionContext\.Properties') | Properties to be populated by connection negotiators\. These will be made available to backends for routing decisions\. |
