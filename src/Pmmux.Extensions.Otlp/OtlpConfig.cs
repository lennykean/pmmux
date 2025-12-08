using OpenTelemetry.Exporter;

namespace Pmmux.Extensions.Otlp;

internal record OtlpConfig
{
    public bool OtlpEnable { get; init; }
    public string? OtlpMetricsEndpoint { get; init; }
    public string? OtlpTracesEndpoint { get; init; }
    public string? OtlpLogsEndpoint { get; init; }
    public string OtlpServiceName { get; init; } = "pmmux";
    public OtlpExportProtocol OtlpMetricsProtocol { get; init; } = OtlpExportProtocol.Grpc;
    public OtlpExportProtocol OtlpTracesProtocol { get; init; } = OtlpExportProtocol.Grpc;
    public OtlpExportProtocol OtlpLogsProtocol { get; init; } = OtlpExportProtocol.Grpc;
}
