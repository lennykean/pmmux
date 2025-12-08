using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Http;

/// <summary>
/// HTTP extension providing backend protocols for HTTP response and proxying.
/// </summary>
public sealed class HttpExtension : IExtension
{
    void IExtension.RegisterCommandOptions(ICommandLineBuilder builder)
    {
    }

    void IExtension.RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddSingleton<IBackendProtocol, HttpResponseBackend.Protocol>();
        services.AddSingleton<IBackendProtocol, HttpProxyBackend.Protocol>();
    }
}
