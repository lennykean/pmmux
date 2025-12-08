using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Pmmux.Extensions.Tls.Abstractions;

/// <summary>Manages TLS certificates and hostname-to-certificate mappings.</summary>
public interface ICertificateManager
{
    /// <summary>Gets all registered certificate names.</summary>
    IEnumerable<string> GetCertificateNames();
    /// <summary>Gets all hostname-to-certificate mappings.</summary>
    IEnumerable<(string Hostname, string CertificateName)> GetMappings();
    /// <summary>Adds a certificate with the specified name.</summary>
    bool TryAddCertificate(string certificateName, X509Certificate certificate);
    /// <summary>Maps a hostname to a certificate name.</summary>
    bool TryAddMapping(string hostname, string certificateName);
    /// <summary>Removes a certificate by name.</summary>
    bool RemoveCertificate(string certificateName);
    /// <summary>Removes a hostname mapping.</summary>
    bool RemoveMapping(string hostname);
    /// <summary>Gets the certificate for a hostname.</summary>
    bool TryGetCertificate(string? hostname, [NotNullWhen(true)] out X509Certificate? certificate);
}

