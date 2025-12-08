using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Pmmux.Extensions.Tls.Abstractions;

namespace Pmmux.Extensions.Tls;

internal class CertificateLoader(
    ICertificateManager certificateManager,
    ILoggerFactory loggerFactory,
    TlsConfig tlsConfig) : IHostedService
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("certificate-loader");

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("loading certificates");

        foreach (var certConfig in tlsConfig.TlsCertificates)
        {
            try
            {
                var certificate = certConfig.Type switch
                {
                    CertificateType.Pfx when certConfig.FilePath is not null =>
                        X509CertificateLoader.LoadPkcs12FromFile(certConfig.FilePath, certConfig.Password),
                    CertificateType.Pfx when certConfig.CertificateData is not null =>
                        X509CertificateLoader.LoadPkcs12(
                            Convert.FromBase64String(certConfig.CertificateData),
                            certConfig.Password),
                    CertificateType.Pem or CertificateType.Der or null when certConfig.FilePath is not null =>
                        X509CertificateLoader.LoadCertificateFromFile(certConfig.FilePath),
                    CertificateType.Pem or CertificateType.Der or null when certConfig.CertificateData is not null =>
                        X509CertificateLoader.LoadCertificate(Convert.FromBase64String(certConfig.CertificateData)),
                    _ =>
                        throw new InvalidOperationException($"invalid certificate configuration: {certConfig}")
                };

                if (!certificateManager.TryAddCertificate(certConfig.Name, certificate))
                {
                    throw new InvalidOperationException("certificate name already in use");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error loading certificate {Name}", certConfig.Name);
                throw;
            }

            _logger.LogDebug("loaded certificate {Name}", certConfig.Name);
        }

        foreach (var mapping in tlsConfig.TlsCertificateMaps)
        {
            if (!certificateManager.TryAddMapping(mapping.Domain, mapping.CertificateName))
            {
                _logger.LogError(
                    "error mapping certificate {CertificateName} to domain {Domain}",
                    mapping.CertificateName,
                    mapping.Domain);

                throw new InvalidOperationException($"certificate mapping conflict");
            }

            _logger.LogDebug(
                "mapped certificate {CertificateName} to domain {Domain}",
                mapping.CertificateName,
                mapping.Domain);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
