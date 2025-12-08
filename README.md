# pmmux

**pmmux** (Port Map Multiplexer) is an extensible multi-protocol network load balancer with built-in NAT port forwarding via UPnP/PMP. Incoming TCP/UDP traffic is routed to backend services based on protocol detection, content inspection, and configurable rules.

## Features

- **Extensible Protocol Detection** - Route traffic based on content inspection via pluggable extensions (TLS/SNI, HTTP headers, custom protocols)
- **Health Monitoring** - Automatic health checks with configurable thresholds
- **Multiple Routing Strategies** - First-available, least-requests, or custom strategies
- **Plugin Architecture** - Extend with custom backends, protocols, and routing logic
- **Observability** - Metrics export via OpenTelemetry

## Quick Start

Route traffic from port 8080 to a backend web server:

```sh
pmmux -b web:pass:ip=127.0.0.1,port=3000 -p 8080:8080:tcp
```

The `-p 8080:8080:tcp` option binds port 8080 locally and attempts to configure NAT port mapping via UPnP/PMP, making the service accessible from external networks.

With multiple backends and health checks:

```sh
pmmux \
  -b "primary:pass:ip=127.0.0.1,port=3000" \
  -b "fallback:pass:ip=127.0.0.1,port=3001,priority=fallback" \
  -z "pass:primary" \
  -p 8080:8080:tcp
```

### Configuration File

pmmux can load configuration from a file (`pmmux.toml` by default), supporting TOML, JSON, and YAML.

```toml
[pmmux]
port-bindings = ["8080:8080:tcp"]
routing-strategy = "least-requests"

backends = [
  "web:pass:ip=127.0.0.1,port=3000",
  "api:pass:ip=127.0.0.1,port=4000",
]

health-checks = ["pass"]
```

## Configuration

Configuration is layered with the following precedence (highest to lowest):
1. **Command-line arguments**
2. **Environment variables**
3. **Configuration file** (TOML, YAML, or JSON)

See the [Configuration Guide](docs/configuration.md) for complete documentation.

## Extensibility

pmmux uses a plugin architecture to allow custom extensions:
- Backend protocols (perform protocol detection and handle traffic)
- Routing strategies (customize how backends are selected)
- Connection negotiators (transport-level protocol negotiation like encryption or packet filtering)

See the [Extensibility Guide](docs/extensibility.md) for extension points and the [API Reference](docs/api-reference.md) for full interface documentation.

## Built-in Backend Protocols

Protocol | Description
-|-
`pass` | Passthrough - forwards connections to another TCP/UDP endpoint (`ip`, `port`)
`noop` | No-op - accepts and discards traffic

## Standard Extensions

Extension | Description | Documentation
-|-|-
**HTTP** | HTTP reverse proxy and custom responses | [README](src/Pmmux.Extensions.Http/README.md)
**TLS** | TLS encryption and SNI-based routing | [README](src/Pmmux.Extensions.Tls/README.md)
**Management** | REST API for runtime management | [README](src/Pmmux.Extensions.Management/README.md)
**OTLP** | OpenTelemetry metrics export | [README](src/Pmmux.Extensions.Otlp/README.md)
**BitTorrent** | BitTorrent protocol detection | [README](src/Pmmux.Extensions.BitTorrent/README.md)

## Service Installation

pmmux can be installed as a system service on Linux and Windows platforms:

```sh
# Install
pmmux install

# Uninstall
pmmux uninstall
```

## Documentation

- [Architecture Guide](docs/architecture.md) - System architecture, component design, and data flow
- [Configuration Guide](docs/configuration.md) - Configuration reference
- [Extensibility Guide](docs/extensibility.md) - Extension points and plugin development
- [API Reference](docs/api-reference.md) - API documentation

## License

MIT License. See [LICENSE](LICENSE) for details.
