using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Pmmux.Extensions.Management.Abstractions;

namespace Pmmux.Extensions.Management;

internal partial class ManagementServer(
    ManagementConfig config,
    IEnumerable<IManagementEndpointGroup> endpointGroups,
    ILoggerFactory loggerFactory) : IHostedService
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("management-server");

    private IHost? _host;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!config.ManagementEnable)
        {
            return;
        }

        var builder = WebApplication.CreateBuilder();

        builder.Services.AddOpenApi();

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(config.ManagementBindAddress, config.ManagementPort);
            options.AddServerHeader = false;
        });

        builder.Logging
            .SetMinimumLevel(LogLevel.Trace)
            .AddFilter("Microsoft", LogLevel.Warning)
            .ClearProviders()
            .AddProvider(new ProxyLoggerProvider(loggerFactory))
            .Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.None);

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
        });

        var app = builder.Build();

        app.MapOpenApi();

        var apiGroup = app.MapGroup("api");

        foreach (var endpointGroup in endpointGroups)
        {
            var group = apiGroup.MapGroup(endpointGroup.Name);
            endpointGroup.MapEndpoints(group);
        }

        await app.StartAsync(cancellationToken).ConfigureAwait(false);

        _host = app;

        _logger.LogInformation(
            "management api started on {ManagementEndpoint}",
            new IPEndPoint(config.ManagementBindAddress, config.ManagementPort));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_host is not null)
        {
            try
            {
                await _host.StopAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _host.Dispose();
            }
        }
    }
}
