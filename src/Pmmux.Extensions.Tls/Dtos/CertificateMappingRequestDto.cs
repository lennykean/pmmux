namespace Pmmux.Extensions.Tls.Dtos;

/// <summary>
/// A DTO object for certificate mapping add requests.
/// </summary>
/// <param name="Hostname">The hostname pattern.</param>
/// <param name="CertificateName">The certificate name.</param>
public record CertificateMappingRequestDto(string Hostname, string CertificateName);

