# HTTP Extension

The HTTP extension provides HTTP protocol support for pmmux, including reverse proxying and static response generation.

## Backend Protocols

Protocol | Description
-|-
`http-proxy` | Reverse proxy using [YARP](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/getting-started)
`http-response` | Generate static HTTP responses

## Installation

The following example adds the HTTP extension to the configuration:

```sh
pmmux -x Pmmux.Extensions.Http.dll ...
```

Or in `pmmux.toml`:

```toml
[pmmux]
extensions = ["Pmmux.Extensions.Http.dll"]
```

## `http-proxy` - Reverse Proxy

Forwards HTTP requests to an upstream server using YARP (Yet Another Reverse Proxy).

### Basic Usage

```sh
pmmux -b "web:http-proxy:proxy.address=https://backend.example.com" -p 8080:8080:tcp
```

### Configuration Parameters

Parameter | Required | Default | Description
-|-|-|-
`proxy.address` | Yes | - | Upstream server URL
`proxy.timeout` | No | - | Request timeout in milliseconds
`proxy.activity-timeout` | No | - | Inactivity timeout in milliseconds
`proxy.transform[type.param]` | No | - | YARP transform configuration

### Transforms

YARP transforms can be configured to modify requests/responses:

```
proxy:http-proxy:proxy.address="https://api.example.com",proxy.transform[RequestHeader.X-Forwarded-Host]={host}
```

See [YARP Transforms documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/transforms) for available transforms.

### Health Checks

The `http-proxy` backend supports health checks:

```sh
pmmux \
  -b "api:http-proxy:proxy.address=https://api.example.com" \
  -z "http-proxy:api:path=/health,status=200-299"
```

#### Health Check Parameters

Parameter | Default | Description
-|-|-
`path` | `/` | Path to check
`status` | `200-299` | Expected status code or range

## `http-response` - Static Response

Generates static HTTP responses without proxying. Useful for redirects, error pages, or health check endpoints.

### Basic Usage

HTTP to HTTPS redirect:

```sh
pmmux -b 'redirect:http-response:response.status=301,response.header[location]="https://{host}{path}"'
```

### Configuration Parameters

Parameter | Required | Default | Description
-|-|-|-
`response.status` | No | `200` | HTTP status code
`response.body` | No | - | Response body content
`response.header[name]` | No | - | Response header (multiple allowed)

### Template Variables

Response headers and body support template variables:

Variable | Description
-|-
`{scheme}` | Request scheme (http/https)
`{host}` | Request host
`{port}` | Request port
`{method}` | HTTP method
`{path}` | Request path
`{query}` | Query string

### Examples

**301 Redirect:**
```
redirect:http-response:response.status=301,response.header[location]="https://{host}{path}{query}"
```

**Custom Error Page:**
```
error:http-response:response.status=503,response.body="Service Unavailable",response.header[content-type]="text/plain"
```

**Health Endpoint:**
```
health:http-response:path=/health,response.status=200,response.body="OK"
```

## Standard Backend Parameters

Both `http-proxy` and `http-response` backends support standard matching and routing parameters, including connection properties, IP/port matching, and priority tiers. See the [Configuration Guide](../../docs/configuration.md#common-parameters) for details on these parameters.

### HTTP Request Matching

In addition to standard parameters, both protocols support HTTP-specific matching. All matching parameters use **regular expressions**.

Parameter | Description
-|-
`host` | Match HTTP `Host` header
`path` | Match request path
`method` | Match HTTP method
`protocol-version` | Match HTTP version
`header[name]` | Match request header value

The `!` suffix enables negated matching: `host!`, `path!`, `method!`, `header[name]!`

### Examples

**Match TLS traffic only:**
```
secure:http-proxy:property[tls]=true,proxy.address="https://backend.example.com"
```

**Match specific host:**
```
api:http-proxy:host=api\.example\.com,proxy.address="https://api-backend:8080"
```

**Match path prefix:**
```
static:http-proxy:path=/static/.*,proxy.address="https://cdn.example.com"
```

**Match POST requests:**
```
api-write:http-proxy:method=POST|PUT|PATCH,proxy.address="https://write-backend:8080"
```

**Exclude certain paths:**
```
app:http-proxy:path!=/health.*,proxy.address="https://app-backend:8080"
```

## Complete Example

Route traffic based on TLS and hostname:

```toml
[pmmux]
extensions = [
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Http.dll",
]

port-bindings = ["443:8443:tcp", "80:8080:tcp"]

backends = [
  # HTTPS traffic to api.example.com
  'api:http-proxy:property[tls]=true,host=api\.example\.com,proxy.address="https://api:8080"',

  # HTTPS traffic to www.example.com
  'www:http-proxy:property[tls]=true,host=www\.example\.com,proxy.address="https://web:3000"',

  # Redirect HTTP to HTTPS
  'redirect:http-response:property[tls]!=true,response.status=301,response.header[location]="https://{host}{path}{query}"',

  # Fallback when backends are unavailable
  'unavailable:http-response:priority=standby,response.status=502',

  # Discard non-HTTP traffic
  'blackhole:noop:priority=fallback',
]

health-checks = [
  "http-proxy:path=/health,status=200-299,interval=10000",
]
```
