using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Factory for creating backend instances from specifications.
/// </summary>
/// <remarks>
/// Backend protocols interpret <see cref="BackendSpec"/> configurations and create
/// corresponding <see cref="IBackend"/> instances. Each protocol has a unique <see cref="Name"/>
/// that is matched against <see cref="BackendSpec.ProtocolName"/>.
/// </remarks>
public interface IBackendProtocol
{
    /// <summary>
    /// Protocol name used to match against <see cref="BackendSpec.ProtocolName"/>.
    /// </summary>
    /// <remarks>
    /// The name should be lowercase and use hyphens for multi-word names (e.g., "http-proxy").
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Create a backend instance from the given specification.
    /// </summary>
    /// <param name="spec">The backend specification containing name and configuration parameters.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// A task that resolves to the created backend instance. The backend should implement
    /// either <see cref="IConnectionOrientedBackend"/> or <see cref="IConnectionlessBackend"/>.
    /// </returns>
    Task<IBackend> CreateBackendAsync(BackendSpec spec, CancellationToken cancellationToken = default);
}
