using System.Collections.Generic;

using Pmmux.Abstractions.Utilities;

namespace Pmmux.Abstractions;

/// <summary>
/// Specification for creating a backend instance.
/// </summary>
public sealed record BackendSpec
{
    /// <param name="name">The unique name identifying the backend.</param>
    /// <param name="protocolName">The name of the protocol that creates the backend.</param>
    /// <param name="parameters">Protocol-specific configuration parameters.</param>
    public BackendSpec(string name, string protocolName, IDictionary<string, string> parameters)
    {
        Name = name;
        ProtocolName = protocolName;
        Parameters = new EquatableDictionary<string, string>(parameters);
    }

    /// <summary>
    /// The unique name identifying the backend.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// The name of the <see cref="IBackendProtocol"/> that creates the backend instance.
    /// </summary>
    public string ProtocolName { get; init; }

    /// <summary>
    /// Protocol-specific configuration parameters.
    /// </summary>
    public IReadOnlyDictionary<string, string> Parameters { get; }
}
