namespace Pmmux.Extensions.Tls.Dtos;

/// <summary>
/// A DTO object representing a certificate mapping.
/// </summary>
/// <param name="Hostname">The hostname pattern.</param>
/// <param name="CertificateName">The certificate name.</param>
public record CertificateMappingDto(string Hostname, string CertificateName);

