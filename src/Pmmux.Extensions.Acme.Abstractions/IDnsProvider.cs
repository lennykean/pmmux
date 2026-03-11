using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.Extensions.Acme.Abstractions;

/// <summary>
/// Provider for ACME DNS-01 challenges.
/// </summary>
public interface IDnsProvider
{
    /// <summary>
    /// The provider name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Create a TXT record for the ACME challenge.
    /// </summary>
    /// <param name="recordName">The fully qualified DNS record name.</param>
    /// <param name="txtValue">The TXT record value.</param>
    /// <param name="properties">The per-certificate provider properties from the certificate entry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>An optional provider-specific propagation token, or <c>null</c> if not applicable.</returns>
    Task<string?> CreateTxtRecordAsync(
        string recordName,
        string txtValue,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken);

    /// <summary>
    /// Delete a TXT record.
    /// </summary>
    /// <param name="recordName">The fully qualified DNS record name.</param>
    /// <param name="txtValue">The TXT record value to delete.</param>
    /// <param name="properties">The per-certificate provider properties from the certificate entry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteTxtRecordAsync(
        string recordName,
        string txtValue,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken);

    /// <summary>
    /// Wait for the TXT record to propagate and become visible to DNS resolvers.
    /// </summary>
    /// <param name="recordName">The fully qualified DNS record name.</param>
    /// <param name="txtValue">The expected TXT record value.</param>
    /// <param name="propagationToken">
    /// The provider-specific propagation token returned by <see cref="CreateTxtRecordAsync"/>.
    /// </param>
    /// <param name="timeout">The maximum time to wait for propagation.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><c>true</c> if the record propagated within the timeout; otherwise <c>false</c>.</returns>
    Task<bool> WaitForPropagationAsync(
        string recordName,
        string txtValue,
        string? propagationToken,
        TimeSpan timeout,
        CancellationToken cancellationToken);
}
