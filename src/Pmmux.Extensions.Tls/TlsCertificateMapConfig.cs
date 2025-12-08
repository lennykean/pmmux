namespace Pmmux.Extensions.Tls;

/// <summary>Configuration for mapping a certificate to a domain name.</summary>
public record TlsCertificateMapConfig(string CertificateName, string Domain);
