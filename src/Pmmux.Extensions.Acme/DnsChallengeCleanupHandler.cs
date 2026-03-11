using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Extensions.Acme.Abstractions;

namespace Pmmux.Extensions.Acme;

internal sealed class DnsChallengeCleanupHandler(
    IDnsProvider dnsProvider,
    List<(string RecordName, string TxtValue)> records,
    IReadOnlyDictionary<string, string> properties,
    ILogger logger) : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        foreach (var (recordName, txtValue) in records)
        {
            try
            {
                await dnsProvider.DeleteTxtRecordAsync(
                    recordName,
                    txtValue,
                    properties,
                    CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "failed to clean up DNS record {RecordName}", recordName);
            }
        }
    }
}
