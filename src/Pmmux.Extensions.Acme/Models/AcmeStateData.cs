using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pmmux.Extensions.Acme.Models;

internal sealed class AcmeStateData
{
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public Dictionary<string, AcmeManagedCertificate> Certificates { get; } = new(StringComparer.OrdinalIgnoreCase);
}
