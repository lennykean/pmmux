using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Tls.Abstractions;
using Pmmux.Extensions.Tls.Dtos;

namespace Pmmux.Extensions.Tls;

internal class TlsEndpointGroup(ICertificateManager certificateManager) : IManagementEndpointGroup
{
    string IManagementEndpointGroup.Name => "tls";

    void IManagementEndpointGroup.MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/certificates", () =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return Results.Ok(certificateManager.GetCertificateNames());
            }
        });
        builder.MapPost("/certificates", async (
            string certificateName,
            [FromBody] Stream certificateStream,
            [FromQuery] CertificateType certificateType,
            [FromQuery] string? password) =>
        {
            using var certificateData = new MemoryStream();
            await certificateStream.CopyToAsync(certificateData).ConfigureAwait(false);

            using (ExecutionContext.SuppressFlow())
            {
                return certificateType switch
                {
                    CertificateType.Pfx => Results.Ok(certificateManager.TryAddCertificate(
                            certificateName,
                            X509CertificateLoader.LoadPkcs12(certificateData.ToArray(), password))),
                    CertificateType.Pem or CertificateType.Der => Results.Ok(certificateManager.TryAddCertificate(
                            certificateName,
                            X509CertificateLoader.LoadCertificate(certificateData.ToArray()))),
                    _ => Results.BadRequest("invalid certificate type")
                };
            }
        });
        builder.MapDelete("/certificates", (string certificateName) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return Results.Ok(certificateManager.RemoveCertificate(certificateName));
            }
        });
        builder.MapGet("/certificate-mappings", () =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                var mappings = certificateManager.GetMappings()
                    .Select(m => new CertificateMappingDto(m.Hostname, m.CertificateName))
                    .ToList();
                return Results.Ok(mappings);
            }
        });
        builder.MapPost("/certificate-mappings", ([FromBody] CertificateMappingRequestDto mapping) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return Results.Ok(certificateManager.TryAddMapping(mapping.Hostname, mapping.CertificateName));
            }
        });
        builder.MapDelete("/certificate-mappings", (string hostname) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return Results.Ok(certificateManager.RemoveMapping(hostname));
            }
        });
    }
}
