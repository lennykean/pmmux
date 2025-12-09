# pmmux

**pmmux** (Port Map Multiplexer) is a lightweight, high-performance port multiplexer with protocol-aware routing and automatic NAT port forwarding.

Multiple applications and protocols are multiplexed through the same network port using content inspection with configurable rules and extendable protocol support. Built-in UPnP/PMP mapping automatically configures NAT port forwarding to allow external access to services.

## Features

- **Automatic NAT Forwarding** - UPnP/PMP discovery and configuration allows routing external traffic to specific services
- **Protocol Detection** - Content-based routing and protocol negotiation
- **Health Monitoring** - Configurable health checks with failure thresholds and backend priorities
- **Routing Strategies** - Fine-grained control over traffic routing and load balancing
- **Observability** - Built-in instrumentation
- **Extensibility** - Custom routing, protocol handling, metric exporters, and more via plugins

## Quick Start

Route traffic from port 8080 to a backend web server:

```sh
pmmux -b web:pass:ip=127.0.0.1,port=3000 -p 8080:8080:tcp
```

The `-p 8080:8080:tcp` option binds port 8080 locally and attempts to configure NAT port mapping via UPnP/PMP, making the service accessible from external networks.

Using extensions, HTTP and BitTorrent traffic can share a single port:

```sh
pmmux \
  -x Pmmux.Extensions.Http.dll \
  -x Pmmux.Extensions.BitTorrent.dll \
  -b "web:http-proxy:proxy.address=http://127.0.0.1:3000" \
  -b "torrent:bittorrent-pass:ip=127.0.0.1,port=6881" \
  -p 8080:8080:tcp
```

Health checks monitor backend availability and route traffic only to healthy services:

```sh
pmmux \
  -b "primary:pass:ip=127.0.0.1,port=3000" \
  -b "fallback:pass:ip=127.0.0.1,port=3001,priority=fallback" \
  -z "pass:primary" \
  -p 8080:8080:tcp
```

### Configuration File

Configuration can also be loaded from a file (`pmmux.toml` by default) in TOML, JSON, or YAML format.

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

The plugin architecture allows extending pmmux with custom backend protocols, routing strategies, and connection negotiators. See the [Extensibility Guide](docs/extensibility.md) for extension points and the [API Reference](docs/api-reference.md) for interface documentation.

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

pmmux can run as a system service on Linux (systemd) and Windows:

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
- [API Reference](docs/api-reference.md) - Interface and type documentation

## License

MIT License. See [LICENSE](LICENSE) for details.
