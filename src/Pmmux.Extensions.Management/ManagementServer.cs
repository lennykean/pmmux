using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

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
            options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            options.SerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        var app = builder.Build();

        app.UseCors();
        app.MapOpenApi();
        app.MapGet("/healthz", () => Results.Ok());

        var apiGroup = app.MapGroup("api");

        foreach (var endpointGroup in endpointGroups)
        {
            var group = apiGroup.MapGroup(endpointGroup.Name);
            _logger.LogDebug("mapping endpoints for {EndpointGroupName}", endpointGroup.Name);
            endpointGroup.MapEndpoints(group);
        }

        if (config.ManagementUiEnable)
        {
            app.UseManagementUi(apiBaseAddress: new Uri("/", UriKind.Relative));
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
