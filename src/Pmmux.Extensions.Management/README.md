# Management Extension

The Management extension provides a REST API for runtime management of pmmux, including backends, health checks, and port mappings.

## Features

- **Runtime Configuration** - Add, update, and remove backends without restart
- **Health Check Management** - Configure health checks dynamically
- **Port Mapping Control** - Manage NAT port mappings via API
- **Extensible API** - Other extensions can add custom endpoints
- **OpenAPI Documentation** - Auto-generated API specification

## Installation

The following example adds the Management extension to the configuration:

```sh
pmmux --management-enable -x Pmmux.Extensions.Management.dll ...
```

Or in `pmmux.toml`:

```toml
[pmmux]
extensions = ["Pmmux.Extensions.Management.dll"]
management-enable = true
```

## Configuration

Option | Default | Description
-|-|-
`--management-enable` | `false` | Enable management API
`--management-port` | `8900` | API port
`--management-bind-address` | `127.0.0.1` | Bind address

### Example

```sh
pmmux \
  --management-enable \
  --management-port 9000 \
  --management-bind-address 0.0.0.0 \
  ...
```

> **Security Note:** By default, the management API binds to `127.0.0.1` (localhost only). Use caution when exposing it on `0.0.0.0` - consider using a firewall or reverse proxy with authentication.

## API Endpoints

Base URL: `http://<bind-address>:<port>/api`

### Backends

Method | Endpoint | Description
-|-|-
`GET` | `/backends?networkProtocol=Tcp` | List all backends
`POST` | `/backends/{protocol}/{name}?networkProtocol=Tcp` | Add a backend
`PUT` | `/backends/{protocol}/{name}?networkProtocol=Tcp` | Update a backend
`DELETE` | `/backends/{protocol}/{name}?networkProtocol=Tcp` | Remove a backend

#### Add Backend

```sh
curl -X POST "http://localhost:8900/api/backends/pass/web?networkProtocol=Tcp" \
  -H "Content-Type: application/json" \
  -d '{"ip": "127.0.0.1", "port": "3000"}'
```

#### List Backends

```sh
curl "http://localhost:8900/api/backends?networkProtocol=Tcp"
```

### Health Checks

Method | Endpoint | Description
-|-|-
`GET` | `/health-checks` | List all health checks
`POST` | `/health-checks` | Add a health check
`DELETE` | `/health-checks` | Remove a health check

#### Add Health Check

```sh
curl -X POST "http://localhost:8900/api/health-checks" \
  -H "Content-Type: application/json" \
  -d '{
    "protocolName": "pass",
    "backendName": "web",
    "parameters": {
      "interval": "5000",
      "timeout": "2000"
    }
  }'
```

### Port Mappings

Method | Endpoint | Description
-|-|-
`GET` | `/port-maps` | List all port mappings
`POST` | `/port-maps` | Add a port mapping
`DELETE` | `/port-maps` | Remove a port mapping

#### Add Port Mapping

```sh
curl -X POST "http://localhost:8900/api/port-maps" \
  -H "Content-Type: application/json" \
  -d '{
    "networkProtocol": "Tcp",
    "localPort": 8080,
    "publicPort": 80
  }'
```

### OpenAPI

Method | Endpoint | Description
-|-|-
`GET` | `/openapi/v1.json` | OpenAPI specification

## Extensibility

Extensions can add custom API endpoints to the management server. See the [Programming Guide](programming-guide.md) for details on implementing `IManagementEndpointGroup`.

## Example: Dynamic Backend Management

```sh
# Start pmmux with management enabled
pmmux \
  --management-enable \
  -b "primary:pass:ip=127.0.0.1,port=3000" \
  -p 8080:8080:tcp

# Add a secondary backend at runtime
curl -X POST "http://localhost:8900/api/backends/pass/secondary?networkProtocol=Tcp" \
  -H "Content-Type: application/json" \
  -d '{"ip": "127.0.0.1", "port": "3001", "priority": "standby"}'

# Check backends
curl "http://localhost:8900/api/backends?networkProtocol=Tcp"

# Remove the secondary backend
curl -X DELETE "http://localhost:8900/api/backends/pass/secondary?networkProtocol=Tcp"
```
