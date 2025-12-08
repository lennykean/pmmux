#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[ICommandLineBuilder](index.md 'Pmmux\.Abstractions\.ICommandLineBuilder')

## ICommandLineBuilder\.Add\(Option\) Method

Add a command\-line option to the parser\.

```csharp
Pmmux.Abstractions.ICommandLineBuilder Add(System.CommandLine.Option option);
```
#### Parameters

<a name='Pmmux.Abstractions.ICommandLineBuilder.Add(System.CommandLine.Option).option'></a>

`option` [System\.CommandLine\.Option](https://learn.microsoft.com/en-us/dotnet/api/system.commandline.option 'System\.CommandLine\.Option')

The option to add \(e\.g\., `--my-option` or `-m`\)\.

#### Returns
[ICommandLineBuilder](index.md 'Pmmux\.Abstractions\.ICommandLineBuilder')  
This builder instance for method chaining\.

### Remarks
Options added via this method will be recognized by the command\-line parser,
shown in help text, and can be bound to configuration values\.