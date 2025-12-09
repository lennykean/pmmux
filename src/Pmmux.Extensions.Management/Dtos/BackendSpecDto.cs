using System.Collections.Generic;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Management.Dtos;

/// <summary>
/// A DTO object representing a backend specification.
/// </summary>
/// <param name="Name">The unique name identifying the backend.</param>
/// <param name="ProtocolName">The name of the protocol that creates the backend.</param>
/// <param name="Parameters">Protocol-specific configuration parameters.</param>
public record BackendSpecDto(
    string Name,
    string ProtocolName,
    IReadOnlyDictionary<string, string> Parameters)
{
    /// <summary>
    /// Create a DTO from a <see cref="BackendSpec"/>.
    /// </summary>
    /// <param name="spec">The backend spec.</param>
    /// <returns>A DTO object.</returns>
    public static BackendSpecDto FromBackendSpec(BackendSpec spec) => new(
        spec.Name,
        spec.ProtocolName,
        spec.Parameters);
}

