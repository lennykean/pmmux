using System.Collections.Generic;

namespace Pmmux.Extensions.Acme.Models;

internal record AcmeConfig
{
    public bool AcmeDisable { get; init; }
    public required string AcmeEmail { get; init; }
    public required string AcmeStoragePath { get; init; }
    public bool AcmeStaging { get; init; }
    public string? AcmeServerUrl { get; init; }
    public required int AcmeRenewalLead { get; init; }
    public IEnumerable<AcmeCertificateEntry> AcmeCertificates { get; init; } = [];
}
