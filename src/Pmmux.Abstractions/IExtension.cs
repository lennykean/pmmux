using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pmmux.Abstractions;

/// <summary>
/// Plugin for extending the port multiplexer with additional capabilities.
/// </summary>
/// <remarks>
/// Extensions add backend protocols, routing strategies, connection negotiators, metric sinks,
/// and other functionality to pmmux. Extensions are loaded at startup from DLL files specified
/// via the <c>--extensions</c> CLI option or the <c>extensions</c> configuration setting.
/// </remarks>
public interface IExtension
{
    /// <summary>
    /// Register services provided by this extension.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    /// <param name="hostContext">The host builder context containing configuration and environment information.</param>
    /// <remarks>
    /// Common services include <see cref="IBackendProtocol"/>, <see cref="IRoutingStrategy"/>,
    /// <see cref="IClientConnectionNegotiator"/>, and <see cref="IMetricSink"/>.
    /// </remarks>
    void RegisterServices(IServiceCollection services, HostBuilderContext hostContext);

    /// <summary>
    /// Register command-line options that configure this extension.
    /// </summary>
    /// <param name="builder">The command-line builder to add options to.</param>
    /// <remarks>
    /// Use <see cref="ICommandLineBuilder.Add"/> to add options or
    /// <see cref="ICommandLineBuilder.AddSubcommand"/> to add new commands.
    /// </remarks>
    void RegisterCommandOptions(ICommandLineBuilder builder);
}
