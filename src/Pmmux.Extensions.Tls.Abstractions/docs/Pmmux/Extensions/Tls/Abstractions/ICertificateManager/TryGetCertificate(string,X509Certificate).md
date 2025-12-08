#### [Pmmux\.Extensions\.Tls\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Tls\.Abstractions](../index.md 'Pmmux\.Extensions\.Tls\.Abstractions').[ICertificateManager](index.md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager')

## ICertificateManager\.TryGetCertificate\(string, X509Certificate\) Method

Gets the certificate for a hostname\.

```csharp
bool TryGetCertificate(string? hostname, out System.Security.Cryptography.X509Certificates.X509Certificate? certificate);
```
#### Parameters

<a name='Pmmux.Extensions.Tls.Abstractions.ICertificateManager.TryGetCertificate(string,System.Security.Cryptography.X509Certificates.X509Certificate).hostname'></a>

`hostname` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Pmmux.Extensions.Tls.Abstractions.ICertificateManager.TryGetCertificate(string,System.Security.Cryptography.X509Certificates.X509Certificate).certificate'></a>

`certificate` [System\.Security\.Cryptography\.X509Certificates\.X509Certificate](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate 'System\.Security\.Cryptography\.X509Certificates\.X509Certificate')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')