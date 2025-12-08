# Configuration Guide

pmmux uses a layered configuration system with the following precedence (highest to lowest):

1. **Command-line arguments**
2. **Environment variables**
3. **Configuration file** (TOML, YAML, or JSON)

## Command-Line Interface

### Core Options

Option | Short | Default | Description
-|-|-|-
`--config-file` | `-c` | `pmmux.toml` | Path to configuration file
`--backend` | `-b` | - | Backend specification (multiple may be specified)
`--port-binding` | `-p` | - | Port binding specification (multiple may be specified)
`--routing-strategy` | `-r` | `first-available` | Routing strategy name
`--health-check` | `-z` | - | Health check specification (multiple may be specified)
`--extensions` | `-x` | - | Extension DLL paths (multiple may be specified)
`--mux-only` | - | `false` | Multiplexer only - disable port forwarding
`--portmap-only` | - | `false` | Port mapping only - disable the traffic multiplexer

#### Port Binding Format

Port bindings use the format `<public-port>:<local-port>:<protocol>`:

- `public-port` - Public port on the NAT device to forward to `local-port`
- `local-port` - Local port that will receive traffic
- `protocol` - bind only `tcp` or `udp` port (both are enabled by default)

```sh
-p 8080:8080:tcp # Forward WAN port 8080 and receive the traffic on local port 8080
-p 443:8443:tcp # Forward WAN port 443 and receive the traffic on local port 8443
```

##### Disable Port Forwarding

The `!` character can be used as the public port to receive LAN traffic without port forwarding.

```sh
-p !:8080:tcp # Bind to local port 8080 without port forwarding
```

##### Auto-Assignment

The `?` character can be used to let the NAT device automatically assign an available port. This works for both public and local ports.

```sh
-p ?:8080:tcp # Auto-assign a WAN port and map to local 8080
-p 8080:?:tcp # Forward WAN port 8080 to an auto-assigned local port
-p ?:?:tcp # Auto-assign both ports
```

> **Note:** auto-assignment may not be supported by all NAT devices.

### Routing & Timeouts

Option | Short | Default | Description
-|-|-|-
`--routing-strategy` | `-r` | `first-available` | Backend selection strategy
`--selection-timeout` | `-s` | `5000` | Timeout (ms) for backend selection
`--queue-length` | `-q` | `100` | Maximum pending connection queue
`--preview-buffer-limit` | - | unlimited | Maximum bytes a backend can preview for protocol detection

## Port Forwarding

pmmux can automatically configure port mappings on NAT devices that support UPnP or NAT-PMP, making locally-bound ports accessible from external networks. By default, it attempts to discover NAT devices on all network interfaces using both protocols.

Option | Short | Default | Description
-|-|-|-
`--gateway` | `-g` | auto-detect | Query a specific gateway IP address for NAT devices
`--network-interface` | `-i` | all | Query for NAT devices on a specific network interface
`--port-map-protocol` | `-m` | both | NAT protocol to use: `Upnp` or `Pmp` (default: tries both)
`--port-map-lifetime` | - | NAT device settings | Port mapping lifetime in seconds (auto-renewed upon expiration)
`--port-map-renewal-lead` | - | `10` | Renewal lead time in seconds (renew before expiration to prevent gaps)

**Examples:**

UPnP protocol on a specific gateway:
```sh
pmmux --gateway 192.168.1.1 --port-map-protocol upnp -p 8080:8080:tcp
```

Specific network interface:
```sh
pmmux --network-interface eth0 -p 8080:8080:tcp
```

Custom port mapping lifetime with early renewal:
```sh
pmmux --port-map-lifetime 3600 --port-map-renewal-lead 60 -p 8080:8080:tcp
```

## Backends

Backends define how traffic is handled. Each backend must have a unique name, protocol type, and optionally configuration parameters. Multiple backends may handle the same connection. [Routing Strategies](#routing-strategies) determine which backend receives the connection, enabling load balancing and failover.

Backends are evaluated in the order defined. How selection works depends on the routing strategy. The default strategy (`first-available`) routes to the first matching backend, while `least-requests` considers priority tier and request count.

### Backend Specification Format

```
<name>:<protocol>:<parameters>
```

- `name` - Unique identifier for the backend
- `protocol` - Backend protocol name (`pass`, `noop`, etc.)
- `parameters` - Comma-separated `key=value` pairs

Values containing special characters (commas, colons, equals, etc) must be quoted:

```sh
-b 'proxy:http-proxy:proxy.address="https://example.com:8080"'
```

### Common Parameters

These parameters are supported by most standard backends:

Parameter | Values | Description
-|-|-
`priority` | `vip`, `normal`, `standby`, `fallback` | Routing priority tier (backend-specific default, usually `normal`)
`property[key]` | regex | Match connection property (e.g., `property[tls.sni]=api\.example\.com`)
`property[key]!` | regex | Negated match (e.g., `property[tls]!=true`)

### Priority Tiers

Priority tiers allow organizing backends by importance. How priority tiers are used depends on the routing strategy. See the [Routing Strategies](#routing-strategies) section for details.

Priority | Typical Use
-|-
`vip` | High-priority backends
`normal` | Standard production traffic
`standby` | Backup backends
`fallback` | Last resort backends

### Built-in Backend Protocols

#### Passthrough (`pass`)

Forwards traffic to another TCP/UDP endpoint.

Parameter | Required | Description
-|-|-
`ip` | Yes | Target IP address
`port` | Yes | Target port

**Examples:**
```sh
# simple passthrough
-b "web:pass:ip=127.0.0.1,port=3000"
```

#### No-op (`noop`)

Accepts and discards all traffic. Useful as a fallback backend. Defaults to `fallback` priority tier.

**Examples:**
```sh
# pass through ipv6 traffic, discard other traffic
-b "web:pass:property[socket.address-family]=internetwork,ip=127.0.0.1,port=3000" -b "blackhole:noop"
```

### Extension Protocols

Extensions provide additional backend protocols:

Protocol | Extension | Description
-|-|-
`http-proxy` | [HTTP](../src/Pmmux.Extensions.Http/README.md) | Reverse proxy to HTTP upstream
`http-response` | [HTTP](../src/Pmmux.Extensions.Http/README.md) | Static HTTP response generator
`bittorrent-pass` | [BitTorrent](../src/Pmmux.Extensions.BitTorrent/README.md) | BitTorrent-aware passthrough

See extension documentation for protocol-specific parameters.

## Health Checks

Health checks monitor backend availability. When a backend fails health checks, it's removed from rotation until it recovers. Some backend protocols may not support health checks - consult specific backend documentation.

### Health Check Specification Format

```
<protocol>:[backend-name]:<parameters>
```

- `protocol` - Health check protocol (matches backend protocol)
- `backend-name` - Target specific backend (optional)
- `parameters` - Comma-separated configuration

**Targeting:**
- `pass:interval=5000` - Checks **all backends** using the `pass` protocol
- `pass:web:interval=5000` - Checks **only the backend named `web`**
- Multiple health checks can be specified with different parameters for different backends

### Common Parameters

These parameters are supported by all backend protocols:

Parameter | Default | Description
-|-|-
`initial-delay` | `0` | Delay before first check (ms)
`interval` | `10000` | Time between checks (ms)
`timeout` | `5000` | Check timeout (ms)
`failure-threshold` | `3` | Consecutive failures before marking unhealthy
`recovery-threshold` | `5` | Consecutive successes before marking healthy

### Examples

```sh
# Check all passthrough backends every 5s
-z "pass:interval=5000,timeout=2000"

# Check specific backend with custom thresholds
-z "pass:web:failure-threshold=5,recovery-threshold=2"

# Multiple health check configurations
-z "pass:interval=10000" -z "http-proxy:api:interval=5000"
```

Extension protocols may support additional health check parameters.

## Routing Strategies

Routing strategies determine which backend receives traffic when multiple backends can handle a connection.

Strategy | Description
-|-
`first-available` | Select first healthy backend in configuration order (default)
`least-requests` | Select healthy backend with fewest routed requests. Evaluates by priority tier first, then selects within the highest priority tier

Configured via `--routing-strategy` / `-r`:

```sh
# Use least-requests strategy
pmmux -r least-requests -b web1:pass:ip=10.0.1.10,port=80 -b web2:pass:ip=10.0.1.11,port=80 -p 8080:8080:tcp
```

## Logging

pmmux supports console logging and optional file-based logging with automatic rotation.

Option | Short | Default | Description
-|-|-|-
`--verbosity` | `-v` | `information` | Log level: `trace`, `debug`, `information`, `warning`, `error`, `critical`
`--log-directory` | `-d` | - | Directory for log files (enables file logging when set)
`--log-file-max-size` | - | - | Maximum log file size in bytes before rotation
`--log-file-max-retain` | - | - | Maximum number of log files to retain (oldest deleted first)
`--log-metrics` | - | `false` | Report metrics in the logs (see [OTLP Extension](../src/Pmmux.Extensions.Otlp/README.md) for OpenTelemetry metrics support)

**Examples:**

Debug logging to console:
```sh
pmmux --verbosity debug -b web:pass:ip=127.0.0.1,port=3000 -p 8080:8080:tcp
```

File logging with rotation:
```sh
pmmux --log-directory ./logs --log-file-max-size 10485760 --log-file-max-retain 5 -p 8080:8080:tcp
```

Verbose logging with metrics:
```sh
pmmux -v trace --log-metrics --log-directory ./logs -p 8080:8080:tcp
```

## Extensions

Extensions add functionality like additional backend protocols and connection negotiators. Load extensions using `--extensions` / `-x`, specifying paths to extension DLL files:

```sh
pmmux -x Pmmux.Extensions.Http.dll -x Pmmux.Extensions.Tls.dll -p 8080:8080:tcp
```

In configuration files, use the `extensions` array:

```toml
[pmmux]
extensions = [
  "Pmmux.Extensions.Http.dll",
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Management.dll",
]
```

### Standard Extensions

Extension | Description | Documentation
-|-|-
**HTTP** | HTTP reverse proxy and response generation | [README](../src/Pmmux.Extensions.Http/README.md)
**TLS** | TLS termination and SNI-based routing | [README](../src/Pmmux.Extensions.Tls/README.md)
**Management** | REST API for runtime management | [README](../src/Pmmux.Extensions.Management/README.md)
**OTLP** | OpenTelemetry metrics export | [README](../src/Pmmux.Extensions.Otlp/README.md)
**BitTorrent** | BitTorrent protocol support | [README](../src/Pmmux.Extensions.BitTorrent/README.md)

Extensions may add additional CLI options and configuration sections.

## Complete Examples

Basic passthrough to a local server:

```sh
pmmux -b "web:pass:ip=127.0.0.1,port=3000" -p 8080:8080:tcp
```

Multiple backends with routing strategy:

```sh
pmmux \
  -p 8080:8080:tcp \
  -b "primary:pass:ip=127.0.0.1,port=3000" \
  -b "secondary:pass:ip=127.0.0.1,port=3001" \
  -b "blackhole:noop" \
  -r least-requests
```

Health check with per-backend settings:

```sh
pmmux \
  -p 8080:8080:tcp \
  -b "primary:pass:ip=127.0.0.1,port=3000" \
  -b "secondary:pass:ip=127.0.0.1,port=3001" \
  -b "blackhole:noop" \
  -z "pass:primary:interval=10000" \
  -z "pass:secondary:interval=30000" \
  -r least-requests
```

## Configuration File

The configuration file provides a structured way to define complex setups. By default, pmmux looks for `pmmux.toml` in the working directory. Configuration files use the same parameter names as command-line options, optionally omitting the `--` prefix.

### File Formats

pmmux supports:
- **TOML** (recommended) - `.toml`
- **YAML** - `.yaml`, `.yml`
- **JSON** - `.json`

### TOML Structure

All pmmux configuration lives under the `[pmmux]` section:

```toml
[pmmux]

# Extensions to load
extensions = [
  "Pmmux.Extensions.Http.dll",
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Management.dll",
]

# Port bindings
port-bindings = [
  "8080:8080:tcp",
  "8080:8080:udp",
  "!:9000:tcp", # Local only, no NAT mapping
]

# Routing
routing-strategy = "least-requests"
selection-timeout = 5000
queue-length = 100
preview-buffer-limit = 65536

# Backends
backends = [
  "web:pass:ip=127.0.0.1,port=3000",
  "api:pass:ip=127.0.0.1,port=4000,priority=normal",
  "fallback:noop:priority=fallback",
]

# Health checks
health-checks = [
  "pass:interval=10000,timeout=5000",
  "pass:web:interval=5000,failure-threshold=5",
]

# Logging
verbosity = "information"
log-directory = "logs"
log-file-max-size = 10485760
log-file-max-retain = 5
log-metrics = false

# NAT/Port forwarding
mux-only = false
portmap-only = false
port-map-protocol = "Upnp"
port-map-lifetime = 3600
port-map-renewal-lead = 60
```

### Extension Configuration

Extensions add their own configuration options under the same `[pmmux]` section:

```toml
[pmmux]
extensions = [
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Management.dll",
  "Pmmux.Extensions.Otlp.dll",
]

# TLS Extension
tls-enable = true
tls-enforce = false
tls-protocol = ["tls12", "tls13"]
tls-generate-certificates = false
tls-certificate = [
  "default:path=/certs/server.pfx,type=pfx,password=super-secret-password",
]
tls-certificate-map = [
  "default:example.com",
  "default:*.example.com",
]

# Management Extension
management-enable = true
management-port = 8900
management-bind-address = "127.0.0.1"

# OTLP Extension
otlp-enable = true
otlp-metrics-endpoint = "http://localhost:9090/api/v1/otlp/v1/metrics"
otlp-metrics-protocol = "HttpProtobuf"
otlp-service-name = "pmmux"
```

## Environment Variables

Environment variables use the same parameter names as command-line options with the prefix `PMMUX__` and double underscores separating nested keys. Option names are converted to uppercase with hyphens replaced by underscores.

Examples:
```sh
# Simple values
export PMMUX__VERBOSITY="debug"
export PMMUX__ROUTING_STRATEGY="least-requests"
export PMMUX__LOG_DIRECTORY="./logs"

# Array values use indexed keys
export PMMUX__BACKENDS__0="web:pass:ip=127.0.0.1,port=3000"
export PMMUX__BACKENDS__1="api:pass:ip=127.0.0.1,port=4000"
export PMMUX__PORT_BINDINGS__0="8080:8080:tcp"
```

