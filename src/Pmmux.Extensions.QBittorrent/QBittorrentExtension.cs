using System;
using System.CommandLine;
using System.Net.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;

using QBittorrent.Client;

namespace Pmmux.Extensions.QBittorrent;

/// <summary>
/// Requests port mappings through PortWarden and synchronizes qBittorrent with the assigned port.
/// </summary>
public sealed class QBittorrentExtension : IExtension
{
    internal Option<bool> EnableOption { get; } = new("--qbittorrent-enable")
    {
        Description = "enable qBittorrent port synchronization",
    };

    internal Option<string> UrlOption { get; } = new("--qbittorrent-url")
    {
        Description = "qBittorrent Web UI base URL",
        DefaultValueFactory = _ => "http://127.0.0.1:18080/"
    };

    internal Option<string> PeerPortOption { get; } = new("--qbittorrent-peer-port")
    {
        Description = "incoming peer connection port, or ? for automatic allocation",
        DefaultValueFactory = _ => "?"
    };

    internal Option<int> SyncSecondsOption { get; } = new("--qbittorrent-sync-seconds")
    {
        Description = "periodic reconciliation interval",
        DefaultValueFactory = _ => 30
    };

    internal Option<string?> UsernameOption { get; } = new("--qbittorrent-username")
    {
        Description = "optional Web UI login name",
    };

    internal Option<string?> PasswordEnvironmentOption { get; } = new("--qbittorrent-password-env")
    {
        Description = "name of environment variable containing the Web UI password",
    };

    void IExtension.RegisterCommandOptions(ICommandLineBuilder builder)
    {
        builder.Add(EnableOption);
        builder.Add(UrlOption);
        builder.Add(PeerPortOption);
        builder.Add(SyncSecondsOption);
        builder.Add(UsernameOption);
        builder.Add(PasswordEnvironmentOption);
    }

    void IExtension.RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        var section = hostContext.Configuration.GetSection("pmmux");

        if (!section.GetValue<bool>(EnableOption.Name))
        {
            return;
        }

        services.AddSingleton(_ =>
        {
            var endpoint = section.GetValue<string>(UrlOption.Name) ?? "http://127.0.0.1:18080/";
            var port = section.GetValue<string>(PeerPortOption.Name) ?? "?";

            if (port != "?" && (!int.TryParse(port, out var parsedPort) || parsedPort is < 1 or > 65535))
            {
                throw new ArgumentException("qBittorrent peer port must be ? or a port number from 1 to 65535.");
            }

            var config = new QBittorrentConfig
            {
                Endpoint = new Uri(endpoint.TrimEnd('/') + "/"),
                PeerPort = port == "?" ? null : int.Parse(port),
                Interval = TimeSpan.FromSeconds(section.GetValue<int?>(SyncSecondsOption.Name) ?? 30),
                Username = section.GetValue<string>(UsernameOption.Name),
                PasswordEnvironment = section.GetValue<string>(PasswordEnvironmentOption.Name)
            };

            ValidateConfiguration(config);

            return config;
        });

        services.AddSingleton<IQBittorrentClient>(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<QBittorrentConfig>();
            var client = new QBittorrentClient(config.Endpoint, ApiLevel.V2, new HttpClientHandler
            {
                UseProxy = false,
                AllowAutoRedirect = false
            }, disposeHandler: true)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            client.DefaultRequestHeaders.Referrer = config.Endpoint;

            return client;
        });
        services.AddSingleton<QBittorrentSynchronizer>();
        services.AddHostedService(serviceProvider => serviceProvider.GetRequiredService<QBittorrentSynchronizer>());
        services.AddSingleton<IManagementEndpointGroup, QBittorrentEndpointGroup>();
    }

    private static void ValidateConfiguration(QBittorrentConfig config)
    {
        if (!config.Endpoint.IsAbsoluteUri || !string.IsNullOrEmpty(config.Endpoint.UserInfo) ||
            !string.IsNullOrEmpty(config.Endpoint.Query) || !string.IsNullOrEmpty(config.Endpoint.Fragment) ||
            (config.Endpoint.Scheme != "https" && !(config.Endpoint.Scheme == "http" && config.Endpoint.IsLoopback)))
        {
            throw new ArgumentException(
                "qBittorrent endpoint must use HTTPS or loopback HTTP, without credentials, query, or fragment.");
        }
        if (config.Interval < TimeSpan.FromSeconds(1) || config.Interval > TimeSpan.FromHours(1))
        {
            throw new ArgumentException("qBittorrent reconciliation interval must be 1–3600 seconds.");
        }
        if (string.IsNullOrWhiteSpace(config.Username) != string.IsNullOrWhiteSpace(config.PasswordEnvironment))
        {
            throw new ArgumentException(
                "Specify both qBittorrent username and password environment variable, or neither.");
        }
    }
}
