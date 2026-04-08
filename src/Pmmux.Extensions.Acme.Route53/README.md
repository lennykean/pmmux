# ACME Route 53 Extension

The Route 53 extension allows the [ACME extension](../Pmmux.Extensions.Acme/README.md) to use AWS Route 53 for DNS-01 challenges.

## Installation

**Linux / macOS:**

```sh
curl -fsSL https://raw.githubusercontent.com/lennykean/pmmux/main/scripts/install-extension.sh | sh -s acme-route53
```

**Windows (PowerShell):**

```powershell
& ([scriptblock]::Create((irm https://raw.githubusercontent.com/lennykean/pmmux/main/scripts/install-extension.ps1))) acme-route53
```

Load the Route 53 extension alongside the [ACME extension](../Pmmux.Extensions.Acme/README.md) and the [TLS extension](../Pmmux.Extensions.Tls/README.md):

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

```sh
pmmux \
  --acme-email "admin@example.com" \
  --acme-certificate "example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID" \
  --tls-enable \
  -x Pmmux.Extensions.Tls.dll \
  -x Pmmux.Extensions.Acme.dll \
  -x Pmmux.Extensions.Acme.Route53.dll \
  -b "web:pass:target.ip=127.0.0.1,target.port=3000" \
  -p 443:8443:tcp
```

## Configuration

### Extension Options

Option | Default | Description
-|-|-
`--acme-route53-access-key-id` | - | AWS access key ID
`--acme-route53-secret-access-key` | - | AWS secret access key
`--acme-route53-credential-profile` | - | AWS credential profile name
`--acme-route53-credential-file` | - | path to AWS shared credentials file

If no credential options are specified, the default AWS SDK credential chain is used (environment variables, shared credentials file, instance profile).

### Per-Certificate Properties

The hosted zone ID is specified per certificate via `--acme-certificate`:

Property | Required | Description
-|-|-
`hostedzoneid` | Yes | AWS Route 53 hosted zone ID

```sh
--acme-certificate "example.com:dns-01:provider=route53,hostedzoneid=HOSTED_ZONE_ID"
```

Different certificates can use different hosted zones:

```sh
--acme-certificate "example.com:dns-01:provider=route53,hostedzoneid=ZONE_A"
--acme-certificate "other.com:dns-01:provider=route53,hostedzoneid=ZONE_B"
```

## See Also

- [ACME Extension](../Pmmux.Extensions.Acme/README.md): Certificate lifecycle and configuration
- [ACME Abstractions](../Pmmux.Extensions.Acme.Abstractions/README.md): Implementing custom DNS providers
