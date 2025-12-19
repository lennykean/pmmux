using System.Collections.Generic;

namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// Backend specification DTO for API requests.
/// </summary>
public record BackendSpecDto
{
    /// <summary>The unique name identifying the backend.</summary>
    public required string Name { get; set; }

    /// <summary>The name of the protocol that creates the backend.</summary>
    public required string ProtocolName { get; set; }

    /// <summary>Protocol-specific configuration parameters.</summary>
    public Dictionary<string, string> Parameters { get; set; } = [];
}
