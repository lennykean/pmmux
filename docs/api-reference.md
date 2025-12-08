# API Reference

## Core Libraries

Assembly | Description | Documentation
-|-|-
**Pmmux.Abstractions** | Core abstractions and interfaces for pmmux extensibility | [API Docs](../src/Pmmux.Abstractions/docs/index.md)
**Pmmux.Core** | Core implementation of the pmmux port multiplexing and routing engine | [API Docs](../src/Pmmux.Core/docs/index.md)
**Pmmux.App** | pmmux command-line application and service installer | [API Docs](../src/Pmmux.App/docs/index.md)

## Extensions

Assembly | Description | Documentation
-|-|-
**Pmmux.Extensions.Http** | HTTP protocol support extension | [API Docs](../src/Pmmux.Extensions.Http/docs/index.md)
**Pmmux.Extensions.Tls** | TLS termination and SNI-based routing extension | [API Docs](../src/Pmmux.Extensions.Tls/docs/index.md)
**Pmmux.Extensions.Tls.Abstractions** | TLS certificate management abstractions | [API Docs](../src/Pmmux.Extensions.Tls.Abstractions/docs/index.md)
**Pmmux.Extensions.Management** | REST API management interface | [API Docs](../src/Pmmux.Extensions.Management/docs/index.md)
**Pmmux.Extensions.Management.Abstractions** | Management API extension abstractions | [API Docs](../src/Pmmux.Extensions.Management.Abstractions/docs/index.md)
**Pmmux.Extensions.Otlp** | OpenTelemetry Protocol (OTLP) metrics export | [API Docs](../src/Pmmux.Extensions.Otlp/docs/index.md)
**Pmmux.Extensions.BitTorrent** | BitTorrent protocol detection and routing | [API Docs](../src/Pmmux.Extensions.BitTorrent/docs/index.md)

For extension points and plugin development, see the [Extensibility Guide](extensibility.md).
