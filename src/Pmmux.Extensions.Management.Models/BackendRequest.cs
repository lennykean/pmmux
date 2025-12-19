using System.Collections.Generic;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Management.Models;

/// <summary>
/// Request to create or update a backend.
/// </summary>
public class BackendRequest
{
    /// <summary>Backend parameters.</summary>
    public Dictionary<string, string> Parameters { get; set; } = [];

    /// <summary>Priority tier.</summary>
    public PriorityTier PriorityTier { get; set; } = PriorityTier.Normal;

    /// <summary>Network protocol.</summary>
    public Mono.Nat.Protocol NetworkProtocol { get; set; }
}
