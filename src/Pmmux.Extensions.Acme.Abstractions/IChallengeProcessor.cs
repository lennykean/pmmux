using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Certes;

namespace Pmmux.Extensions.Acme.Abstractions;

/// <summary>
/// A challenge processor that handles a specific ACME challenge type.
/// </summary>
public interface IChallengeProcessor
{
    /// <summary>
    /// The ACME challenge type that this processor handles.
    /// </summary>
    string ChallengeType { get; }

    /// <summary>
    /// Prepare challenge responses for the specified challenge.
    /// </summary>
    /// <param name="authorizations">The authorizations and their selected challenge contexts.</param>
    /// <param name="accountKey">The ACME account key used to derive challenge tokens.</param>
    /// <param name="provider">The provider name from the certificate entry, or <c>null</c> for default.</param>
    /// <param name="properties">The per-certificate properties from the certificate entry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A handle that cleans up challenge resources when disposed.</returns>
    Task<IAsyncDisposable> PrepareAsync(
        IEnumerable<AuthorizationInfo> authorizations,
        IKey accountKey,
        string? provider,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken);
}
