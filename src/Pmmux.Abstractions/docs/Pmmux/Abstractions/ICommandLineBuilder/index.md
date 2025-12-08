#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## ICommandLineBuilder Interface

Registers command\-line options with the [System\.CommandLine](https://learn.microsoft.com/en-us/dotnet/api/system.commandline 'System\.CommandLine') parser\.

```csharp
public interface ICommandLineBuilder
```

### Example

Adding options and subcommands in an extension:

```csharp
public void RegisterCommandOptions(ICommandLineBuilder builder)
{
    // Add a simple string option
    var apiKey = new Option<string>(
        aliases: ["--api-key", "-k"],
        description: "API key for authentication");
    builder.Add(apiKey);

    // Add an option with a default value
    var timeout = new Option<int>(
        aliases: ["--timeout", "-t"],
        description: "Request timeout in milliseconds",
        getDefaultValue: () => 30000);
    builder.Add(timeout);

    // Add a subcommand with its own handler
    var validateCommand = new Command("validate", "Validate the configuration");
    validateCommand.SetHandler(() =>
    {
        Console.WriteLine("Configuration is valid!");
    });
    builder.AddSubcommand(validateCommand);

    // Add a subcommand with arguments
    var testCommand = new Command("test-backend", "Test a specific backend");
    var backendArg = new Argument<string>("backend", "Backend name to test");
    testCommand.AddArgument(backendArg);
    testCommand.SetHandler((backend) =>
    {
        Console.WriteLine($"Testing backend: {backend}");
    }, backendArg);
    builder.AddSubcommand(testCommand);
}
```

### Remarks

Extensions use this interface to add custom CLI options and subcommands that are
recognized when the extension is loaded.

Options added via this builder will appear in help text and can be used in the
command line or bound to configuration.

| Methods | |
| :--- | :--- |
| [Add\(Option\)](Add(Option).md 'Pmmux\.Abstractions\.ICommandLineBuilder\.Add\(System\.CommandLine\.Option\)') | Add a command\-line option to the parser\. |
| [AddSubcommand\(Command\)](AddSubcommand(Command).md 'Pmmux\.Abstractions\.ICommandLineBuilder\.AddSubcommand\(System\.CommandLine\.Command\)') | Add a subcommand to the parser\. |
