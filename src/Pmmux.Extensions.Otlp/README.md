# OTLP Extension

The OTLP extension enables OpenTelemetry metrics export for observability.

## Quickstart

The following example adds the OTLP extension to the configuration:

```sh
pmmux --otlp-enable -x Pmmux.Extensions.Otlp.dll ...
```

Or in `pmmux.toml`:

```toml
[pmmux]
extensions = ["Pmmux.Extensions.Otlp.dll"]
otlp-enable = true
```

## Configuration

Option | Default | Description
-|-|-
`--otlp-enable` | `false` | Enable OpenTelemetry export
`--otlp-metrics-endpoint` | - | Metrics endpoint URL
`--otlp-metrics-protocol` | `Grpc` | Metrics protocol (`Grpc` or `HttpProtobuf`)
`--otlp-service-name` | `pmmux` | Service name for telemetry

### Example: Prometheus

```sh
pmmux \
  --otlp-enable \
  --otlp-metrics-endpoint "http://localhost:9090/api/v1/otlp/v1/metrics" \
  --otlp-metrics-protocol HttpProtobuf \
  --otlp-service-name "my-pmmux-instance"
  ...
```

### Example: OpenTelemetry Collector

```sh
pmmux \
  --otlp-enable \
  --otlp-metrics-endpoint "http://otel-collector:4317" \
  --otlp-metrics-protocol Grpc
  ...
```

## Configuration File

```toml
[pmmux]
otlp-enable = true
otlp-metrics-endpoint = "http://localhost:9090/api/v1/otlp/v1/metrics"
otlp-metrics-protocol = "HttpProtobuf"
otlp-service-name = "pmmux"
```
