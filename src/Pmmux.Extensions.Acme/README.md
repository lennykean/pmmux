# ACME Extension

The ACME extension automates TLS certificate provisioning and renewal via Let's Encrypt or any ACME-compatible CA. Issued certificates are installed into the [TLS extension](../Pmmux.Extensions.Tls/README.md), and renewed automatically before they expire.

## Installation

**Linux / macOS:**

```sh
curl -fsSL https://raw.githubusercontent.com/lennykean/pmmux/main/scripts/install-extension.sh | sh -s acme
```

**Windows (PowerShell):**

```powershell
& ([scriptblock]::Create((irm https://raw.githubusercontent.com/lennykean/pmmux/main/scripts/install-extension.ps1))) acme
```

Once installed, the extension is loaded via the `-x` flag or configuration file:

```sh
pmmux \
  -x Pmmux.Extensions.Tls.dll \
  -x Pmmux.Extensions.Acme.dll ...
```

Or in `pmmux.toml`:

```toml
[pmmux]
extensions = [
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Acme.dll",
]
```

## Quick Start

Obtain a certificate for `example.com` using HTTP-01 validation:

```sh
pmmux \
  --acme-email "admin@example.com" \
  --acme-certificate "example.com:http-01:san=www.example.com" \
  --tls-enable \
  -x Pmmux.Extensions.Tls.dll \
  -x Pmmux.Extensions.Acme.dll \
  -b "web:pass:target.ip=127.0.0.1,target.port=3000" \
  -p 443:8443:tcp
```

On first startup the extension registers an ACME account, completes the configured challenge, and obtains the certificate. On subsequent startups it loads the certificate from disk and renews it when expiration approaches.

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
`challenge-type` | ACME challenge type (`http-01` or `dns-01`); defaults to `dns-01`
`properties` | Comma-separated key=value pairs

The following properties are handled by the ACME extension itself:

Property | Challenge | Description
-|-|-
`san` | any | additional Subject Alternative Names (semicolon-separated for multiple)
`auto-map` | `http-01` | controls whether a NAT port mapping is created for the HTTP challenge port (default `true`)
`provider` | `dns-01` | DNS provider name (e.g., `route53`)

Properties not listed above are forwarded to the challenge processor. For `dns-01`, this means the DNS provider, see the provider's documentation for supported properties.

**Examples:**

```sh
# HTTP-01 challenge (no additional extensions needed)
--acme-certificate "example.com:http-01"

# HTTP-01 with SANs
--acme-certificate "example.com:http-01:san=www.example.com;api.example.com"

# HTTP-01 without automatic NAT mapping
--acme-certificate "example.com:http-01:auto-map=false"

# DNS-01 (requires a DNS provider extension)
--acme-certificate "example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID"

# DNS-01 wildcard
--acme-certificate "*.example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID"

# Domain only, defaults to dns-01
--acme-certificate example.com
```

## Challenge Types

### http-01

The `http-01` challenge validates domain ownership over HTTP on port 80. The ACME extension handles this automatically. A port 80 listener and NAT mapping are created as needed during validation and torn down afterward. No additional extensions are required.

### dns-01

The `dns-01` challenge proves domain ownership by creating a DNS TXT record. It supports wildcard certificates and does not require inbound HTTP access, but requires a DNS provider extension to manage records. See [DNS Provider Extensions](#dns-provider-extensions) for available providers.

## Certificate Lifecycle

Certificates are checked every 12 hours. Each certificate transitions through:

State | Description
-|-
`Pending` | Newly configured, not yet provisioned
`Validating` | ACME order placed, challenge in progress
`Valid` | Certificate issued and installed
`RenewalDue` | Expiration is within the renewal window
`Expired` | Certificate has passed its expiration date
`Error` | Provisioning or renewal failed (retried up to 5 times)

Failures are isolated per certificate, one failing does not block others. On restart, stuck `Validating` and `Error` states are reset to `Pending`. Configuration changes (domain, provider, or properties) also trigger re-provisioning.

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

- [Pmmux.Extensions.Acme.Route53](../Pmmux.Extensions.Acme.Route53/README.md): AWS Route 53

When multiple providers are loaded, the `provider` property in the certificate entry selects which one to use. If omitted, the first available provider is used. Provider-level configuration such as credentials is handled through the provider extension's own CLI options.

Custom providers can be implemented using the [ACME Abstractions](../Pmmux.Extensions.Acme.Abstractions/README.md) package.

## Complete Example

```toml
[pmmux]
extensions = [
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Acme.dll",
]

port-bindings = ["443:8443:tcp"]

# TLS
tls-enable = true

# ACME
acme-email = "admin@example.com"
acme-certificate = [
  "example.com:http-01:san=www.example.com",
]

backends = [
  "web:pass:target.ip=127.0.0.1,target.port=3000",
]
```

The port 80 listener and NAT mapping are created automatically during challenge validation.

## See Also

- [TLS Extension](../Pmmux.Extensions.Tls/README.md): TLS termination and certificate management
- [ACME Abstractions](../Pmmux.Extensions.Acme.Abstractions/README.md): Interfaces for custom challenge processors and DNS providers
- [Route 53 Extension](../Pmmux.Extensions.Acme.Route53/README.md): AWS Route 53 DNS provider
