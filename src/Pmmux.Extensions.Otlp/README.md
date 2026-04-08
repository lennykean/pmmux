# OTLP Extension

The OTLP extension enables OpenTelemetry metrics export for observability.

## Installation

**Linux / macOS:**

```sh
curl -fsSL https://raw.githubusercontent.com/lennykean/pmmux/main/scripts/install-extension.sh | sh -s otlp
```

**Windows (PowerShell):**

```powershell
& ([scriptblock]::Create((irm https://raw.githubusercontent.com/lennykean/pmmux/main/scripts/install-extension.ps1))) otlp
```

## Quickstart

Once installed, the extension is loaded via the `-x` flag or configuration file:

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
