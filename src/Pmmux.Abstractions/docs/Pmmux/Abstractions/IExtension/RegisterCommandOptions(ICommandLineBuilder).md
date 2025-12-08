#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IExtension](index.md 'Pmmux\.Abstractions\.IExtension')

## IExtension\.RegisterCommandOptions\(ICommandLineBuilder\) Method

Register command\-line options that configure this extension\.

```csharp
void RegisterCommandOptions(Pmmux.Abstractions.ICommandLineBuilder builder);
```
#### Parameters

<a name='Pmmux.Abstractions.IExtension.RegisterCommandOptions(Pmmux.Abstractions.ICommandLineBuilder).builder'></a>

`builder` [ICommandLineBuilder](../ICommandLineBuilder/index.md 'Pmmux\.Abstractions\.ICommandLineBuilder')

The command\-line builder to add options to\.

### Remarks
Use [Add\(Option\)](../ICommandLineBuilder/Add(Option).md 'Pmmux\.Abstractions\.ICommandLineBuilder\.Add\(System\.CommandLine\.Option\)') to add options or
[AddSubcommand\(Command\)](../ICommandLineBuilder/AddSubcommand(Command).md 'Pmmux\.Abstractions\.ICommandLineBuilder\.AddSubcommand\(System\.CommandLine\.Command\)') to add new commands\.