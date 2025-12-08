#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IExtension Interface

Plugin for extending the port multiplexer with additional capabilities\.

```csharp
public interface IExtension
```

### Remarks
Extensions add backend protocols, routing strategies, connection negotiators, metric sinks,
and other functionality to pmmux\. Extensions are loaded at startup from DLL files specified
via the `--extensions` CLI option or the `extensions` configuration setting\.

| Methods | |
| :--- | :--- |
| [RegisterCommandOptions\(ICommandLineBuilder\)](RegisterCommandOptions(ICommandLineBuilder).md 'Pmmux\.Abstractions\.IExtension\.RegisterCommandOptions\(Pmmux\.Abstractions\.ICommandLineBuilder\)') | Register command\-line options that configure this extension\. |
| [RegisterServices\(IServiceCollection, HostBuilderContext\)](RegisterServices(IServiceCollection,HostBuilderContext).md 'Pmmux\.Abstractions\.IExtension\.RegisterServices\(Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, Microsoft\.Extensions\.Hosting\.HostBuilderContext\)') | Register services provided by this extension\. |
