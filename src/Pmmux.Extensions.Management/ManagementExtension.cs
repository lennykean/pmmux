using System.CommandLine;
using System.Net;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;

namespace Pmmux.Extensions.Management;

/// <summary>
/// Management extension providing API endpoints for managing the system.
/// </summary>
public sealed class ManagementExtension : IExtension
{
    internal Option<bool> EnableOption { get; } = new("--management-enable")
    {
        Description = "enable management API",
    };

    internal Option<bool> UiEnableOption { get; } = new("--management-ui-enable")
    {
        Description = "enable management web UI",
    };

    internal Option<int> PortOption { get; } = new("--management-port")
    {
        Description = "management API port",
        DefaultValueFactory = _ => 8900
    };

    internal Option<string> BindAddressOption { get; } = new("--management-bind-address")
    {
        Description = "management API bind address",
        DefaultValueFactory = _ => IPAddress.Loopback.ToString()
    };

    void IExtension.RegisterCommandOptions(ICommandLineBuilder builder)
    {
        builder.Add(EnableOption);
        builder.Add(UiEnableOption);
        builder.Add(PortOption);
        builder.Add(BindAddressOption);
    }

    void IExtension.RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddSingleton(serviceProvider =>
        {
            var section = hostContext.Configuration.GetSection("pmmux");
            var bindAddress = IPAddress.TryParse(section.GetValue<string>(BindAddressOption.Name), out var address)
                ? address
                : IPAddress.Loopback;

            return new ManagementConfig
            {
                ManagementEnable = section.GetValue<bool>(EnableOption.Name),
                ManagementUiEnable = section.GetValue<bool>(UiEnableOption.Name),
                ManagementPort = section.GetValue<int>(PortOption.Name),
                ManagementBindAddress = bindAddress
            };
        });

        services.AddSingleton<IManagementEndpointGroup, HealthCheckEndpointGroup>();
        services.AddSingleton<IManagementEndpointGroup, BackendsEndpointGroup>();
        services.AddSingleton<IManagementEndpointGroup, PortmapEndpointGroup>();
        services.AddSingleton<IManagementEndpointGroup, ListenersEndpointGroup>();

        services.AddHostedService<ManagementServer>();
    }
}
