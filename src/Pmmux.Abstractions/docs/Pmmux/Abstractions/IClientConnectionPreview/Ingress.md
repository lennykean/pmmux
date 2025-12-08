#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IClientConnectionPreview](index.md 'Pmmux\.Abstractions\.IClientConnectionPreview')

## IClientConnectionPreview\.Ingress Property

Pipeline reader for peeking at incoming data\.

```csharp
System.IO.Pipelines.PipeReader Ingress { get; }
```

#### Property Value
[System\.IO\.Pipelines\.PipeReader](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipelines.pipereader 'System\.IO\.Pipelines\.PipeReader')

### Remarks
Use [System\.IO\.Pipelines\.PipeReader\.ReadAsync\(System\.Threading\.CancellationToken\)](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipelines.pipereader.readasync#system-io-pipelines-pipereader-readasync(system-threading-cancellationtoken) 'System\.IO\.Pipelines\.PipeReader\.ReadAsync\(System\.Threading\.CancellationToken\)') to examine buffered data, then call
[System\.IO\.Pipelines\.PipeReader\.AdvanceTo\(System\.SequencePosition,System\.SequencePosition\)](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipelines.pipereader.advanceto#system-io-pipelines-pipereader-advanceto(system-sequenceposition-system-sequenceposition) 'System\.IO\.Pipelines\.PipeReader\.AdvanceTo\(System\.SequencePosition,System\.SequencePosition\)')
with the start position to avoid consuming the data\. Configuration may limit buffered data\.