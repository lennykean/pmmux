using System.CommandLine;

namespace Pmmux.Abstractions;

/// <summary>
/// Registers command-line options with the <see cref="System.CommandLine"/> parser.
/// </summary>
/// <remarks>
/// <para>
/// Extensions use this interface to add custom CLI options and subcommands that are
/// recognized when the extension is loaded.
/// </para>
/// <para>
/// Options added via this builder will appear in help text and can be used in the
/// command line or bound to configuration.
/// </para>
/// </remarks>
/// <example>
/// <para>Adding options and subcommands in an extension:</para>
/// <code>
/// public void RegisterCommandOptions(ICommandLineBuilder builder)
/// {
///     // Add a simple string option
///     var apiKey = new Option&lt;string&gt;(
///         aliases: ["--api-key", "-k"],
///         description: "API key for authentication");
///     builder.Add(apiKey);
///
///     // Add an option with a default value
///     var timeout = new Option&lt;int&gt;(
///         aliases: ["--timeout", "-t"],
///         description: "Request timeout in milliseconds",
///         getDefaultValue: () => 30000);
///     builder.Add(timeout);
///
///     // Add a subcommand with its own handler
///     var validateCommand = new Command("validate", "Validate the configuration");
///     validateCommand.SetHandler(() =>
///     {
///         Console.WriteLine("Configuration is valid!");
///     });
///     builder.AddSubcommand(validateCommand);
///
///     // Add a subcommand with arguments
///     var testCommand = new Command("test-backend", "Test a specific backend");
///     var backendArg = new Argument&lt;string&gt;("backend", "Backend name to test");
///     testCommand.AddArgument(backendArg);
///     testCommand.SetHandler((backend) =>
///     {
///         Console.WriteLine($"Testing backend: {backend}");
///     }, backendArg);
///     builder.AddSubcommand(testCommand);
/// }
/// </code>
/// </example>
public interface ICommandLineBuilder
{
    /// <summary>
    /// Add a command-line option to the parser.
    /// </summary>
    /// <param name="option">The option to add (e.g., <c>--my-option</c> or <c>-m</c>).</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// Options added via this method will be recognized by the command-line parser,
    /// shown in help text, and can be bound to configuration values.
    /// </remarks>
    ICommandLineBuilder Add(Option option);

    /// <summary>
    /// Add a subcommand to the parser.
    /// </summary>
    /// <param name="command">The subcommand to add (e.g., <c>pmmux my-command</c>).</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// Subcommands are available as child commands of the root <c>pmmux</c> command.
    /// Use <c>Command.SetHandler</c> to define the subcommand's behavior.
    /// </remarks>
    ICommandLineBuilder AddSubcommand(Command command);
}
