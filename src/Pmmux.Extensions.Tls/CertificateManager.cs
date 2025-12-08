using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;

using Pmmux.Extensions.Tls.Abstractions;

namespace Pmmux.Extensions.Tls;

internal class CertificateManager(ILoggerFactory loggerFactory, TlsConfig tlsConfig) : ICertificateManager
{
    private readonly ConcurrentDictionary<string, X509Certificate> _certificates = [];
    private readonly ConcurrentDictionary<string, X509Certificate> _selfSignedCertificates = [];
    private readonly ConcurrentDictionary<string, string> _certificateMap = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILogger _logger = loggerFactory.CreateLogger("certificate-manager");

    private X509Certificate? _defaultCertificate;

    public IEnumerable<string> GetCertificateNames()
    {
        return _certificates.Keys;
    }

    public IEnumerable<(string Hostname, string CertificateName)> GetMappings()
    {
        return _certificateMap.Select(kvp => (kvp.Key, kvp.Value));
    }

    public bool TryAddCertificate(string name, X509Certificate certificate)
    {
        return _certificates.TryAdd(name, certificate);
    }

    public bool TryAddMapping(string hostname, string certificateName)
    {
        return _certificateMap.TryAdd(hostname, certificateName);
    }

    public bool TryGetCertificate(string? hostname, [NotNullWhen(true)] out X509Certificate? certificate)
    {
        _logger.LogTrace("looking up certificate for host '{Hostname}'", hostname);

        if (hostname is not null)
        {
            if (!_certificateMap.TryGetValue(hostname, out var certificateName))
            {
                foreach (var pattern in _certificateMap.Keys)
                {
                    if (!pattern.Contains('*'))
                    {
                        continue;
                    }

                    var regex = $"^{Regex.Escape(pattern).Replace("\\*", "[a-z0-9-]+")}$";
                    if (Regex.IsMatch(hostname, regex, RegexOptions.IgnoreCase))
                    {
                        certificateName = _certificateMap[pattern];
                        break;
                    }
                }
            }

            if (certificateName is not null && _certificates.TryGetValue(certificateName, out certificate))
            {
                _logger.LogDebug("using certificate '{CertificateName}' host '{Hostname}'", certificateName, hostname);
                return true;
            }
        }

        if (tlsConfig.TlsGenerateCertificates)
        {
            if (hostname is not null)
            {
                certificate = _selfSignedCertificates.GetOrAdd(hostname, GenerateSelfSignedCertificate);
            }
            else
            {
                _defaultCertificate ??= GenerateSelfSignedCertificate(null);
                certificate = _defaultCertificate;
            }
            _logger.LogDebug("using self signed certificate for host '{Hostname}'", hostname);
            return true;
        }

        _logger.LogDebug("could not find a certificate for host '{Hostname}'", hostname);
        certificate = null;
        return false;
    }

    public bool RemoveCertificate(string certificateName)
    {
        return _certificates.TryRemove(certificateName, out _);
    }

    public bool RemoveMapping(string hostname)
    {
        return _certificateMap.Remove(hostname, out _);
    }

    public X509Certificate GenerateSelfSignedCertificate(string? hostName)
    {
        var subjectName = new X500DistinguishedName($"CN={hostName}");

        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var request = new CertificateRequest(subjectName, ecdsa, HashAlgorithmName.SHA256);

        var serverAuthOid = new Oid("1.3.6.1.5.5.7.3.1");
        var keyUsages = X509KeyUsageFlags.DigitalSignature;

        request.CertificateExtensions.Add(new X509KeyUsageExtension(keyUsages, critical: false));
        request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension([serverAuthOid], critical: false));

        if (!string.IsNullOrEmpty(hostName))
        {
            var san = new SubjectAlternativeNameBuilder();
            san.AddDnsName(hostName);
            request.CertificateExtensions.Add(san.Build());
        }

        var ephemeralCert = request.CreateSelfSigned(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(90));
        var pfx = ephemeralCert.Export(X509ContentType.Pkcs12, password: string.Empty);
        var certificate = X509CertificateLoader.LoadPkcs12(pfx, password: string.Empty);

        _logger.LogDebug("created self-signed certificate for '{SubjectName}'", certificate.Subject);

        return certificate;
    }
}

