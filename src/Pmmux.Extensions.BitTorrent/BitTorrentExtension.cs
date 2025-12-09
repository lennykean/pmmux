using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.BitTorrent;

/// <summary>
/// BitTorrent protocol extension for pmmux.
/// </summary>
public sealed class BitTorrentExtension : IExtension
{
    void IExtension.RegisterCommandOptions(ICommandLineBuilder builder)
    {
    }

    void IExtension.RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddSingleton<IBackendProtocol, BitTorrentPassthroughBackend.Protocol>();
    }
}
