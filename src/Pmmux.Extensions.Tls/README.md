# TLS Extension

The TLS extension provides TLS/SSL termination and SNI-based routing for pmmux.

## Features

- **TLS Termination** - Decrypt TLS traffic before routing to backends
- **SNI Routing** - Route based on Server Name Indication hostname
- **Certificate Management** - Load certificates from files or configure programmatically
- **Auto-Generation** - Optionally generate self-signed certificates
- **Multiple Protocols** - Support for TLS 1.2 and TLS 1.3

## Installation

The following example adds the TLS extension to the configuration:

```sh
pmmux --tls-enable -x Pmmux.Extensions.Tls.dll ...
```

Or in `pmmux.toml`:

```toml
[pmmux]
extensions = ["Pmmux.Extensions.Tls.dll"]
tls-enable = true
```

## Quick Start

### Basic TLS Setup

TLS with auto-generated self-signed certificates:

```sh
pmmux \
  --tls-enable \
  --tls-generate-certificates \
  -b "web:pass:target.ip=127.0.0.1,target.port=3000" \
  -p 443:8443:tcp
```

### With Custom Certificates

```sh
pmmux \
  --tls-enable \
  --tls-certificate "default:path=/certs/server.pfx,type=pfx,password=secret" \
  --tls-certificate-map "default:example.com" \
  --tls-certificate-map "default:*.example.com" \
  -b "web:pass:target.ip=127.0.0.1,target.port=3000" \
  -p 443:8443:tcp
```

## Configuration

### Key Options

Option | Default | Description
-|-|-
`--tls-enable` | `false` | Enable TLS support
`--tls-enforce` | `false` | Require TLS for all connections
`--tls-protocol` | `Tls12,Tls13` | Supported TLS protocols
`--tls-generate-certificates` | `false` | Auto-generate self-signed certs
`--tls-hello-timeout` | `5000` | TLS handshake timeout (ms)
`--tls-certificate` | - | Certificate configuration
`--tls-certificate-map` | - | Hostname to certificate mapping

### Certificate Format

```
--tls-certificate <name>:path=<file>,type=<pfx|pem|der>,password=<secret>
--tls-certificate-map <cert-name>:<hostname>
```

**Examples:**

```sh
--tls-certificate "my-cert:path=/certs/server.pfx,type=pfx,password=secret"
--tls-certificate-map "my-cert:example.com"
--tls-certificate-map "my-cert:*.example.com"
```

## Connection Properties

The TLS extension adds the following properties to connections:

Property | Description
-|-
`tls` | `"true"` or `"false"`
`tls.sni` | SNI hostname from handshake
`tls.protocol` | Negotiated protocol (e.g., `Tls13`)
`tls.cipher-suite` | Negotiated cipher suite
`tls.application-protocol` | ALPN protocol if negotiated

These can be used for backend matching:

```sh
pmmux \
  --tls-enable \
  -b "secure:http-proxy:property[tls]=true,proxy.address=https://backend:8443" \
  -b "insecure:http-response:property[tls]!=true,response.status=301,response.header[location]=https://{host}{path}"
```

## Programmatic Certificate Management

`ICertificateManager` can be injected for runtime certificate operations:

```csharp
public class MyCertService(ICertificateManager certManager)
{
    public void AddCert(string name, X509Certificate2 cert)
    {
        certManager.TryAddCertificate(name, cert);
        certManager.TryAddMapping("example.com", name);
    }
}
```

**Available methods:**
- `GetCertificateNames()` - List registered certificates
- `TryAddCertificate(name, cert)` - Add a certificate
- `TryAddMapping(hostname, certName)` - Map hostname to certificate
- `RemoveCertificate(name)` / `RemoveMapping(hostname)`
- `TryGetCertificate(hostname, out cert)` - Lookup by hostname (supports wildcards)

## Management API Integration

When the Management extension is loaded, the TLS extension adds API endpoints for runtime certificate management.

### Endpoints

Method | Endpoint | Description
-|-|-
`GET` | `/tls/certificates` | List certificate names
`POST` | `/tls/certificates?certificateName=...&certificateType=...` | Upload certificate
`DELETE` | `/tls/certificates?certificateName=...` | Remove certificate
`GET` | `/tls/certificate-mappings` | List hostname mappings
`POST` | `/tls/certificate-mappings` | Add hostname mapping
`DELETE` | `/tls/certificate-mappings?hostname=...` | Remove hostname mapping

### Examples

**Upload certificate:**
```sh
curl -X POST "http://localhost:8900/api/tls/certificates?certificateName=my-cert&certificateType=Pfx&password=secret" \
  --data-binary @/path/to/cert.pfx
```

**Add certificate mapping:**
```sh
curl -X POST "http://localhost:8900/api/tls/certificate-mappings" \
  -H "Content-Type: application/json" \
  -d '{"hostname": "example.com", "certificateName": "my-cert"}'
```

**List certificates:**
```sh
curl "http://localhost:8900/api/tls/certificates"
```

**Remove certificate:**
```sh
curl -X DELETE "http://localhost:8900/api/tls/certificates?certificateName=my-cert"
```

## Example: Multi-Domain Setup

```toml
[pmmux]
extensions = [
  "Pmmux.Extensions.Tls.dll",
  "Pmmux.Extensions.Http.dll",
]

port-bindings = ["443:8443:tcp"]

tls-enable = true
tls-protocol = ["Tls12", "Tls13"]

tls-certificate = [
  "site1:path=/certs/site1.pfx,type=pfx,password=secret1",
  "site2:path=/certs/site2.pem",
]

tls-certificate-map = [
  "site1:site1.example.com",
  "site1:www.site1.example.com",
  "site2:site2.example.com",
  "site2:*.site2.example.com",
]

backends = [
  'site1:http-proxy:property[tls.sni]=site1\.example\.com,proxy.address="http://site1-backend:8080"',
  'site2:http-proxy:property[tls.sni]=.*site2\.example\.com,proxy.address="http://site2-backend:8080"',
]
```
