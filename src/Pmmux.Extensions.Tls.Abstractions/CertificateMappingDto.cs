namespace Pmmux.Extensions.Tls.Abstractions;

/// <summary>
/// Certificate mapping (hostname to certificate).
/// </summary>
/// <param name="Hostname">The hostname pattern.</param>
/// <param name="CertificateName">The certificate name.</param>
public record CertificateMappingDto(string Hostname, string CertificateName);
