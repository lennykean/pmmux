using System;
using System.Collections.Generic;

namespace Pmmux.Extensions.Acme.Models;

internal record AcmeCertificateEntry
{
    public List<string> Domains { get; init; } = [];
    public required string Challenge { get; init; }
    public string? Provider { get; init; }
    public Dictionary<string, string> ProviderProperties { get; init; } = new(StringComparer.OrdinalIgnoreCase);
}
