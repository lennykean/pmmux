#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[ClientConnectionContext](index.md 'Pmmux\.Abstractions\.ClientConnectionContext')

## ClientConnectionContext\.Properties Property

Properties to be populated by connection negotiators\.
These will be made available to backends for routing decisions\.

```csharp
public System.Collections.Concurrent.ConcurrentDictionary<string,string> Properties { get; init; }
```

#### Property Value
[System\.Collections\.Concurrent\.ConcurrentDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2 'System\.Collections\.Concurrent\.ConcurrentDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2 'System\.Collections\.Concurrent\.ConcurrentDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2 'System\.Collections\.Concurrent\.ConcurrentDictionary\`2')