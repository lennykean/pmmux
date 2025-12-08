using System;
using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Otlp;

/// <summary>
/// OpenTelemetry extension to add OTLP metrics export.
/// </summary>
public sealed class OtlpExtension : IExtension
{
    internal Option<bool> EnableOption { get; } = new("--otlp-enable")
    {
        Description = "enable OpenTelemetry exporter",
    };

    internal Option<string?> MetricsEndpointOption { get; } = new("--otlp-metrics-endpoint")
    {
        Description = "OpenTelemetry metrics exporter endpoint",
    };

    internal Option<OtlpExportProtocol> MetricsExporterProtocolOption { get; } = new("--otlp-metrics-protocol")
    {
        Description = "OpenTelemetry exporter protocol",
        DefaultValueFactory = _ => OtlpExportProtocol.Grpc
    };

    internal Option<string> ServiceNameOption { get; } = new("--otlp-service-name")
    {
        Description = "service name for OpenTelemetry metrics",
        DefaultValueFactory = _ => "pmmux"
    };

    void IExtension.RegisterCommandOptions(ICommandLineBuilder builder)
    {
        builder.Add(EnableOption);
        builder.Add(MetricsEndpointOption);
        builder.Add(MetricsExporterProtocolOption);
        builder.Add(ServiceNameOption);
    }

    void IExtension.RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        var section = hostContext.Configuration.GetSection("pmmux");

        var config = new OtlpConfig
        {
            OtlpEnable = section.GetValue<bool>(EnableOption.Name),
            OtlpMetricsEndpoint = section.GetValue<string?>(MetricsEndpointOption.Name),
            OtlpMetricsProtocol = section.GetValue<OtlpExportProtocol>(MetricsExporterProtocolOption.Name),
            OtlpServiceName = section.GetValue<string?>(ServiceNameOption.Name) ?? "pmmux",
        };

        services.AddSingleton(config);

        if (config.OtlpEnable)
        {
            services.AddSingleton<IMetricSink, OtlpMetricSink>();

            services
                .AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    if (string.IsNullOrWhiteSpace(config.OtlpMetricsEndpoint))
                    {
                        return;
                    }
                    metrics.AddMeter("*");
                    metrics.AddOtlpExporter(exporter =>
                    {
                        exporter.Endpoint = new Uri(config.OtlpMetricsEndpoint);
                        exporter.Protocol = config.OtlpMetricsProtocol;
                    });
                })
                .ConfigureResource(resource => resource.AddService(config.OtlpServiceName));
        }
    }
}
