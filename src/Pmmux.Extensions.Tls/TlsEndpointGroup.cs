using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Tls.Abstractions;

namespace Pmmux.Extensions.Tls;

internal class TlsEndpointGroup(ICertificateManager certificateManager) : IManagementEndpointGroup
{
    string IManagementEndpointGroup.Name => "tls";

    void IManagementEndpointGroup.MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/certificates", () => ExecutionContextUtility.SuppressFlow(() =>
        {
            return Results.Ok(certificateManager.GetCertificateNames());
        }));
        builder.MapPost("/certificates", (
            string certificateName,
            [FromBody] Stream certificateStream,
            [FromQuery] CertificateType certificateType,
            [FromQuery] string? password) => ExecutionContextUtility.SuppressFlow(async () =>
        {
            using var certificateData = new MemoryStream();

            await certificateStream.CopyToAsync(certificateData).ConfigureAwait(false);
            var certBytes = certificateData.ToArray();

            return certificateType switch
            {
                CertificateType.Pfx => Results.Ok(certificateManager.TryAddCertificate(
                    certificateName,
                    X509CertificateLoader.LoadPkcs12(certBytes, password))),
                CertificateType.Pem or CertificateType.Der => Results.Ok(certificateManager.TryAddCertificate(
                    certificateName,
                    X509CertificateLoader.LoadCertificate(certBytes))),
                _ => Results.BadRequest("invalid certificate type")
            };
        }));

        builder.MapDelete("/certificates", (string certificateName) => ExecutionContextUtility.SuppressFlow(() =>
        {
            return Results.Ok(certificateManager.RemoveCertificate(certificateName));
        }));

        builder.MapGet("/certificate-mappings", () => ExecutionContextUtility.SuppressFlow(() =>
        {
            var mappings = certificateManager.GetMappings();

            return Results.Ok(mappings.Select(m => new CertificateMappingDto(m.Hostname, m.CertificateName)).ToArray());
        }));

        builder.MapPost("/certificate-mappings", (
            [FromBody] CertificateMappingDto mapping) => ExecutionContextUtility.SuppressFlow(() =>
        {
            var hostname = mapping.Hostname;
            var certName = mapping.CertificateName;

            return Results.Ok(certificateManager.TryAddMapping(hostname, certName));
        }));

        builder.MapDelete("/certificate-mappings", (string hostname) => ExecutionContextUtility.SuppressFlow(() =>
        {
            return Results.Ok(certificateManager.RemoveMapping(hostname));
        }));
    }
}
