#### [Pmmux\.Extensions\.Tls\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Tls\.Abstractions](../index.md 'Pmmux\.Extensions\.Tls\.Abstractions')

## ICertificateManager Interface

Manages TLS certificates and hostname\-to\-certificate mappings\.

```csharp
public interface ICertificateManager
```

| Methods | |
| :--- | :--- |
| [GetCertificateNames\(\)](GetCertificateNames().md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager\.GetCertificateNames\(\)') | Gets all registered certificate names\. |
| [GetMappings\(\)](GetMappings().md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager\.GetMappings\(\)') | Gets all hostname\-to\-certificate mappings\. |
| [RemoveCertificate\(string\)](RemoveCertificate(string).md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager\.RemoveCertificate\(string\)') | Removes a certificate by name\. |
| [RemoveMapping\(string\)](RemoveMapping(string).md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager\.RemoveMapping\(string\)') | Removes a hostname mapping\. |
| [TryAddCertificate\(string, X509Certificate\)](TryAddCertificate(string,X509Certificate).md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager\.TryAddCertificate\(string, System\.Security\.Cryptography\.X509Certificates\.X509Certificate\)') | Adds a certificate with the specified name\. |
| [TryAddMapping\(string, string\)](TryAddMapping(string,string).md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager\.TryAddMapping\(string, string\)') | Maps a hostname to a certificate name\. |
| [TryGetCertificate\(string, X509Certificate\)](TryGetCertificate(string,X509Certificate).md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager\.TryGetCertificate\(string, System\.Security\.Cryptography\.X509Certificates\.X509Certificate\)') | Gets the certificate for a hostname\. |
