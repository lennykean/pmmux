using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;
using Pmmux.Core;
using Pmmux.Core.Configuration;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130

/// <summary>
/// Extension methods for registering Pmmux services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register core Pmmux services and services from the provided extensions.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="hostContext">The host builder context.</param>
    /// <param name="listenerConfigFactory">The listener configuration factory.</param>
    /// <param name="portWardenConfigFactory">The port warden configuration factory.</param>
    /// <param name="routerConfigFactory">The router configuration factory.</param>
    /// <param name="extensions">The extensions to load.</param>
    public static IServiceCollection AddPmmux(
        this IServiceCollection services,
        HostBuilderContext hostContext,
        Func<ListenerConfig> listenerConfigFactory,
        Func<PortWardenConfig> portWardenConfigFactory,
        Func<RouterConfig> routerConfigFactory,
        IEnumerable<IExtension> extensions)
    {
        services.AddSingleton<EventBroker>();
        services.AddSingleton<IEventNotifier>(sp => sp.GetRequiredService<EventBroker>());
        services.AddSingleton<IEventSender>(sp => sp.GetRequiredService<EventBroker>());
        services.AddSingleton<IMetricReporter, MetricReporter>();

        services.AddSingleton<IClientConnectionNegotiator, SocketClientConnectionNegotiator>();
        services.AddSingleton<IRoutingStrategy, FirstAvailableRoutingStrategy>();
        services.AddSingleton<IRoutingStrategy, LeastRequestsRoutingStrategy>();
        services.AddSingleton<IBackendProtocol, NoopBackend.Protocol>();
        services.AddSingleton<IBackendProtocol, PassthroughBackend.Protocol>();

        foreach (var extension in extensions)
        {
            extension.RegisterServices(services, hostContext);
        }

        services.AddSingleton(_ => listenerConfigFactory());
        services.AddSingleton(_ => portWardenConfigFactory());
        services.AddSingleton(_ => routerConfigFactory());

        services.TryAddSingleton<IPortWarden, PortWarden>();
        services.TryAddSingleton<IRouter, Router>();
        services.TryAddSingleton<IBackendMonitor, BackendMonitor>();
        services.TryAddSingleton<IPortMultiplexer, PortMultiplexer>();

        return services;
    }
}
