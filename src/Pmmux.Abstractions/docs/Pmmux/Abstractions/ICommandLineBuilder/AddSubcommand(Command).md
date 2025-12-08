#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[ICommandLineBuilder](index.md 'Pmmux\.Abstractions\.ICommandLineBuilder')

## ICommandLineBuilder\.AddSubcommand\(Command\) Method

Add a subcommand to the parser\.

```csharp
Pmmux.Abstractions.ICommandLineBuilder AddSubcommand(System.CommandLine.Command command);
```
#### Parameters

<a name='Pmmux.Abstractions.ICommandLineBuilder.AddSubcommand(System.CommandLine.Command).command'></a>

`command` [System\.CommandLine\.Command](https://learn.microsoft.com/en-us/dotnet/api/system.commandline.command 'System\.CommandLine\.Command')

The subcommand to add \(e\.g\., `pmmux my-command`\)\.

#### Returns
[ICommandLineBuilder](index.md 'Pmmux\.Abstractions\.ICommandLineBuilder')  
This builder instance for method chaining\.

### Remarks
Subcommands are available as child commands of the root `pmmux` command\.
Use `Command.SetHandler` to define the subcommand's behavior\.