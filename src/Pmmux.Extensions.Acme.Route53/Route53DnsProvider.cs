using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Amazon;
using Amazon.Route53;
using Amazon.Route53.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

using Microsoft.Extensions.Logging;

using Pmmux.Extensions.Acme.Abstractions;

namespace Pmmux.Extensions.Acme.Route53;

internal sealed class Route53DnsProvider(Route53Config config, ILoggerFactory loggerFactory) : IDnsProvider, IDisposable
{
    private const long RecordTtl = 60;
    private static readonly TimeSpan PropagationBuffer = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan PropagationPollInterval = TimeSpan.FromSeconds(10);

    private readonly ILogger _logger = loggerFactory.CreateLogger("acme-dns-route53");

    private readonly Lazy<AmazonRoute53Client> _route53Client = new(() => CreateRoute53Client(config));

    public string Name => "route53";

    public async Task<string?> CreateTxtRecordAsync(
        string recordName,
        string txtValue,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken)
    {
        var zoneId = GetHostedZoneId(properties);

        _logger.LogDebug("creating TXT record {RecordName} in hosted zone {ZoneId}", recordName, zoneId);

        var request = new ChangeResourceRecordSetsRequest
        {
            HostedZoneId = zoneId,
            ChangeBatch = new()
            {
                Changes =
                [
                    new()
                    {
                        Action = ChangeAction.UPSERT,
                        ResourceRecordSet = new()
                        {
                            Name = recordName,
                            Type = RRType.TXT,
                            TTL = RecordTtl,
                            ResourceRecords = [new() { Value = $"\"{txtValue}\"" }]
                        }
                    }
                ]
            }
        };

        var response = await _route53Client.Value.ChangeResourceRecordSetsAsync(request, cancellationToken)
            .ConfigureAwait(false);
        var changeId = response.ChangeInfo.Id;

        _logger.LogDebug("TXT record change submitted, change ID {ChangeId}", changeId);

        return changeId;
    }

    public async Task DeleteTxtRecordAsync(
        string recordName,
        string txtValue,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken)
    {
        var zoneId = GetHostedZoneId(properties);
        _logger.LogDebug("deleting TXT record {RecordName}", recordName);

        var request = new ChangeResourceRecordSetsRequest
        {
            HostedZoneId = zoneId,
            ChangeBatch = new()
            {
                Changes =
                [
                    new()
                    {
                        Action = ChangeAction.DELETE,
                        ResourceRecordSet = new()
                        {
                            Name = recordName,
                            Type = RRType.TXT,
                            TTL = RecordTtl,
                            ResourceRecords = [new() { Value = $"\"{txtValue}\"" }]
                        }
                    }
                ]
            }
        };

        await _route53Client.Value.ChangeResourceRecordSetsAsync(request, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("TXT record {RecordName} deleted", recordName);
    }

    public async Task<bool> WaitForPropagationAsync(
        string recordName,
        string txtValue,
        string? changeId,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        if (changeId is null)
        {
            return true;
        }

        _logger.LogDebug("waiting for route53 change {ChangeId} to sync", changeId);
        var deadline = DateTime.UtcNow + timeout;

        while (DateTime.UtcNow < deadline)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var response = await _route53Client.Value.GetChangeAsync(new() { Id = changeId }, cancellationToken)
                .ConfigureAwait(false);

            if (response.ChangeInfo.Status == ChangeStatus.INSYNC)
            {
                _logger.LogDebug("route53 change {changeId} is INSYNC, waiting propagation buffer", changeId);
                await Task.Delay(PropagationBuffer, cancellationToken).ConfigureAwait(false);

                _logger.LogDebug("dns propagation complete for {RecordName}", recordName);
                return true;
            }

            _logger.LogDebug(
                "route53 change {ChangeId} status: {Status}, polling again in {Interval}s",
                changeId,
                response.ChangeInfo.Status,
                PropagationPollInterval.TotalSeconds);

            await Task.Delay(PropagationPollInterval, cancellationToken).ConfigureAwait(false);
        }

        _logger.LogError("dns propagation timeout for {RecordName} after {Timeout}s", recordName, timeout.TotalSeconds);
        return false;
    }

    public void Dispose()
    {
        if (_route53Client.IsValueCreated)
        {
            _route53Client.Value.Dispose();
        }
    }

    private static string GetHostedZoneId(IReadOnlyDictionary<string, string> properties)
    {
        if (!properties.TryGetValue("hostedzoneid", out var zoneId) || string.IsNullOrEmpty(zoneId))
        {
            throw new InvalidOperationException("'hostedzoneid' property is required for route53 provider");
        }
        return zoneId;
    }

    private static AmazonRoute53Client CreateRoute53Client(Route53Config route53Config)
    {
        var accessKeyId = route53Config.AccessKeyId;
        var secretAccessKey = route53Config.SecretAccessKey;
        var profile = route53Config.CredentialProfile;
        var credentialFile = route53Config.CredentialFile;

        var region = RegionEndpoint.GetBySystemName(route53Config.Region);
        var clientConfig = new AmazonRoute53Config { RegionEndpoint = region };

        if (!string.IsNullOrEmpty(accessKeyId) || !string.IsNullOrEmpty(secretAccessKey))
        {
            return new AmazonRoute53Client(new BasicAWSCredentials(accessKeyId, secretAccessKey), clientConfig);
        }

        if (!string.IsNullOrEmpty(profile) || !string.IsNullOrEmpty(credentialFile))
        {
            var chain = new CredentialProfileStoreChain(credentialFile);
            if (!chain.TryGetAWSCredentials(profile ?? "default", out var credentials))
            {
                throw new InvalidOperationException($"AWS credential profile '{profile ?? "default"}' not found");
            }
            return new(credentials, clientConfig);
        }

        return new(clientConfig);
    }
}
