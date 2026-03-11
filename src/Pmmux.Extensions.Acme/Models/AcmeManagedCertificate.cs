using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pmmux.Extensions.Acme.Models;

internal sealed class AcmeManagedCertificate
{
    public required string PrimaryDomain { get; init; }
    public required List<string> Domains { get; set; }
    public required string ChallengeType { get; set; }
    public string? Provider { get; set; }
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public Dictionary<string, string> ProviderProperties { get; } = new(StringComparer.OrdinalIgnoreCase);
    public AcmeCertificateStatus Status { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
    public DateTime? LastRenewalAtUtc { get; set; }
    public DateTime? LastAttemptAtUtc { get; set; }
    public int FailureCount { get; set; }
    public string? ErrorMessage { get; set; }
}
