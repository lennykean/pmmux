using System.Collections.Generic;

using Pmmux.Abstractions.Utilities;

namespace Pmmux.Abstractions;

/// <summary>
/// Represents runtime information about an active backend instance.
/// </summary>
/// <remarks>
/// BackendInfo combines the backend's configuration (<see cref="Spec"/>) with runtime properties
/// and routing metadata.
/// </remarks>
public record BackendInfo
{
    /// <param name="spec">The specification that defines the backend.</param>
    /// <param name="properties">Runtime properties provided by the backend implementation.</param>
    /// <param name="priorityTier">The routing priority tier.</param>
    public BackendInfo(
        BackendSpec spec,
        IDictionary<string, string> properties,
        PriorityTier priorityTier = PriorityTier.Normal)
    {
        Spec = spec;
        Properties = new EquatableDictionary<string, string>(properties);
        PriorityTier = priorityTier;
    }

    /// <summary>
    /// The routing priority tier of the backend.
    /// </summary>
    public PriorityTier PriorityTier { get; init; }

    /// <summary>
    /// The specification that defines the backend.
    /// </summary>
    public BackendSpec Spec { get; init; }

    /// <summary>
    /// Runtime properties provided by the backend implementation.
    /// </summary>
    /// <remarks>
    /// Properties are provided by the specific implementation and may differ between backend protocols.
    /// </remarks>
    public IReadOnlyDictionary<string, string> Properties { get; init; }
}
