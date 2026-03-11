# ACME Extension

The ACME extension automates TLS certificate provisioning and renewal via Let's Encrypt or any ACME-compatible CA. Issued certificates are installed into the [TLS extension](../Pmmux.Extensions.Tls/README.md), and renewed automatically before they expire.

## Installation

The following example loads the ACME extension with Route 53 as the DNS provider:

```sh
pmmux \
  -x Pmmux.Extensions.Tls.dll \
  -x Pmmux.Extensions.Acme.dll \
  -x Pmmux.Extensions.Acme.Route53.dll ...
```

Or in `pmmux.toml`:

```toml
[pmmux]
extensions = [
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Acme.dll",
  "Pmmux.Extensions.Acme.Route53.dll",
]
```

## Quick Start

Obtain a wildcard certificate for `example.com` using Route 53 for DNS validation:

```sh
pmmux \
  --acme-email "admin@example.com" \
  --acme-certificate "*.example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID" \
  --tls-enable \
  -x Pmmux.Extensions.Tls.dll \
  -x Pmmux.Extensions.Acme.dll \
  -x Pmmux.Extensions.Acme.Route53.dll \
  -b "web:pass:target.ip=127.0.0.1,target.port=3000" \
  -p 443:8443:tcp
```

On first startup the extension registers an ACME account, places a DNS-01 challenge, and obtains the certificate. On subsequent startups it loads the certificate from disk and renews it when expiration approaches.

## Configuration

### Options

Option | Default | Description
-|-|-
`--acme-disable` | `false` | disable ACME certificate automation
`--acme-email` | - | email address for ACME account registration
`--acme-storage-path` | `./acme` | directory path for account and certificate storage
`--acme-staging` | `false` | use Let's Encrypt staging environment
`--acme-server-url` | - | override ACME server URL (e.g., a non-Let's Encrypt CA)
`--acme-renewal-lead` | `30` | renew certificates this many days before expiration
`--acme-certificate` | - | certificate to manage (repeatable)

### Certificate Entry Format

The `--acme-certificate` option uses the compact config format:

```
<domain>[:<challenge-type>[:<properties>]]
```

Segment | Description
-|-
`domain` | Primary domain (e.g., `example.com`, `*.example.com`)
`challenge-type` | ACME challenge type; defaults to `dns-01`
`properties` | Comma-separated key=value pairs

The following properties are handled by the ACME extension itself:

Property | Description
-|-
`provider` | DNS provider name (e.g., `route53`)
`san` | Additional Subject Alternative Names (semicolon-separated for multiple)

All other properties are forwarded to the DNS provider. See the provider's documentation for supported properties (e.g., `hostedzoneid` for [Route 53](../Pmmux.Extensions.Acme.Route53/README.md)).

**Examples:**

```sh
# Domain only, defaults to dns-01
--acme-certificate example.com

# Explicit challenge type
--acme-certificate "*.example.com:dns-01"

# With DNS provider and provider-specific properties
--acme-certificate "example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID"

# With SANs
--acme-certificate "example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID,san=www.example.com;api.example.com"
```

Wildcard domains (`*.example.com`) require the `dns-01` challenge type.

## Certificate Lifecycle

Certificates are checked every 12 hours. Each certificate transitions through:

State | Description
-|-
`Pending` | Newly configured, not yet provisioned
`Validating` | ACME order placed, DNS challenge in progress
`Valid` | Certificate issued and installed
`RenewalDue` | Expiration is within the renewal window
`Expired` | Certificate has passed its expiration date
`Error` | Provisioning or renewal failed (retried up to 5 times)

Failures are isolated per certificate -- one failing does not block others. On restart, stuck `Validating` and `Error` states are reset to `Pending`. Configuration changes (domain, provider, or properties) also trigger re-provisioning.

## Storage

All ACME data is stored under `--acme-storage-path` (default `./acme`):

```
acme/
  account.json        # ACME account key and registration
  state.json          # Certificate states and metadata
  certificates/
    example.com.pfx   # Certificate and private key (PKCS#12)
```

When `--acme-staging` is enabled, files are stored under an `acme/staging/` subdirectory instead of `acme/` directly, keeping staging and production data separate.

The storage directory is created with restrictive permissions (owner-only on Unix, current user + SYSTEM + Admins on Windows). Wildcard certificates are stored with `_wildcard_` replacing the `*` character.

## DNS Provider Extensions

DNS record management is delegated to provider extensions. At least one provider must be loaded when using `dns-01` challenges. Available providers:

- [Pmmux.Extensions.Acme.Route53](../Pmmux.Extensions.Acme.Route53/README.md) -- AWS Route 53

When multiple providers are loaded, the `provider` property in the certificate entry selects which one to use. If omitted, the first available provider is used. Provider-level configuration such as credentials is handled through the provider extension's own CLI options.

Custom providers can be implemented using the [ACME Abstractions](../Pmmux.Extensions.Acme.Abstractions/README.md) package.

## Complete Example

```toml
[pmmux]
extensions = [
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Http.dll",
  "Pmmux.Extensions.Acme.dll",
  "Pmmux.Extensions.Acme.Route53.dll",
]

port-bindings = ["443:8443:tcp", "80:8080:tcp"]

# TLS
tls-enable = true

# ACME
acme-email = "admin@example.com"
acme-storage-path = "/var/lib/pmmux/acme"
acme-renewal-lead = 30

acme-certificate = [
  "example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID,san=www.example.com",
  "*.example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID",
]

backends = [
  # HTTPS traffic
  'web:http-proxy:property[tls]=true,proxy.address="http://web-backend:3000"',

  # Redirect HTTP to HTTPS
  'redirect:http-response:property[tls]!=true,response.status=301,response.header[location]="https://{host}{path}{query}"',
]
```

## See Also

- [TLS Extension](../Pmmux.Extensions.Tls/README.md) -- TLS termination and certificate management
- [ACME Abstractions](../Pmmux.Extensions.Acme.Abstractions/README.md) -- Interfaces for custom DNS providers
- [Route 53 Extension](../Pmmux.Extensions.Acme.Route53/README.md) -- AWS Route 53 DNS provider
