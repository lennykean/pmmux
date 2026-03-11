using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Certes;

using Microsoft.Extensions.Logging;

using Pmmux.Extensions.Acme.Abstractions;

namespace Pmmux.Extensions.Acme;

internal sealed class DnsChallengeProcessor(
    IEnumerable<IDnsProvider> dnsProviders,
    ILoggerFactory loggerFactory) : IChallengeProcessor
{
    private static readonly TimeSpan PropagationTimeout = TimeSpan.FromMinutes(5);

    private readonly ILogger _logger = loggerFactory.CreateLogger("acme-dns-challenge");

    public string ChallengeType => "dns-01";

    public async Task<IAsyncDisposable> PrepareAsync(
        IEnumerable<AuthorizationInfo> authorizations,
        IKey accountKey,
        string? provider,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken)
    {
        var dnsProvider = SelectProvider(provider);
        _logger.LogDebug("selected DNS provider: {Provider}", dnsProvider.Name);

        var createdRecords = new List<(string RecordName, string TxtValue, string? PropagationToken)>();

        try
        {
            foreach (var auth in authorizations)
            {
                var txtValue = accountKey.DnsTxt(auth.Challenge.Token);
                var domain = auth.Domain.StartsWith("*.", StringComparison.Ordinal)
                    ? auth.Domain.Substring(2)
                    : auth.Domain;
                var recordName = $"_acme-challenge.{domain}";

                _logger.LogDebug("creating TXT record {RecordName} = {TxtValue}", recordName, txtValue);
                var token = await dnsProvider.CreateTxtRecordAsync(recordName, txtValue, properties, cancellationToken)
                    .ConfigureAwait(false);

                createdRecords.Add((recordName, txtValue, token));
            }

            foreach (var (recordName, txtValue, propagationToken) in createdRecords)
            {
                var propagated = await dnsProvider.WaitForPropagationAsync(
                    recordName,
                    txtValue,
                    propagationToken,
                    PropagationTimeout,
                    cancellationToken).ConfigureAwait(false);

                if (!propagated)
                {
                    throw new TimeoutException($"DNS propagation timeout for {recordName}");
                }
            }
        }
        catch
        {
            var cleanupRecords = createdRecords.Select(r => (r.RecordName, r.TxtValue)).ToList();
            var partialCleanup = new DnsChallengeCleanupHandler(dnsProvider, cleanupRecords, properties, _logger);
            await partialCleanup.DisposeAsync().ConfigureAwait(false);
            throw;
        }

        var records = createdRecords.Select(r => (r.RecordName, r.TxtValue)).ToList();
        return new DnsChallengeCleanupHandler(dnsProvider, records, properties, _logger);
    }

    private IDnsProvider SelectProvider(string? providerName)
    {
        if (providerName is not null)
        {
            var provider = dnsProviders
                .FirstOrDefault(p => string.Equals(p.Name, providerName, StringComparison.OrdinalIgnoreCase));

            if (provider is null)
            {
                throw new InvalidOperationException($"unknown DNS provider '{providerName}'");
            }

            return provider;
        }

        var defaultProvider = dnsProviders.FirstOrDefault();

        if (defaultProvider is null)
        {
            throw new InvalidOperationException("no DNS providers are available");
        }

        return defaultProvider;
    }
}
