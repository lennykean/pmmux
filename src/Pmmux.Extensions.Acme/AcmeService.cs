using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Pmmux.Core;
using Pmmux.Extensions.Acme.Abstractions;
using Pmmux.Extensions.Acme.Models;
using Pmmux.Extensions.Tls.Abstractions;

namespace Pmmux.Extensions.Acme;

internal sealed class AcmeService(
    AcmeConfig config,
    AcmeClient acmeClient,
    AcmeStateStore stateStore,
    IEnumerable<IChallengeProcessor> challengeProcessors,
    IEnumerable<ICertificateManager> certificateManagers,
    ILoggerFactory loggerFactory) : IHostedService, IDisposable
{
    private const int MaxFailureCount = 5;
    private static readonly TimeSpan RenewalCheckInterval = TimeSpan.FromHours(12);

    private readonly ICertificateManager? _certificateManager = certificateManagers.FirstOrDefault();
    private readonly ILogger _logger = loggerFactory.CreateLogger("acme-service");

    private enum State { Initial = 0, Starting, Started, Disposed }
    private readonly StateManager<State> _state = new(State.Initial);

    private CancellationTokenSource? _workerCts;
    private Task? _renewalLoopTask;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (config.AcmeDisable)
        {
            _logger.LogDebug("acme is disabled, skipping initialization");
            return;
        }

        if (!config.AcmeCertificates.Any())
        {
            _logger.LogWarning("acme is enabled but no certificates are configured");
            return;
        }

        if (!_state.TryTransition(to: State.Starting, from: State.Initial))
        {
            return;
        }

        _logger.LogInformation("starting acme service");

        try
        {
            await stateStore.InitializeAsync(cancellationToken).ConfigureAwait(false);
            await acmeClient.InitializeAsync(cancellationToken).ConfigureAwait(false);

            var stateData = stateStore.GetStateData();
            var resetRequired = false;

            foreach (var (domain, cert) in stateData.Certificates)
            {
                if (cert.Status == AcmeCertificateStatus.Validating)
                {
                    _logger.LogWarning(
                        "certificate {Domain} was in Validating state on startup, resetting to Pending",
                        domain);

                    cert.Status = AcmeCertificateStatus.Pending;
                    resetRequired = true;
                }

                if (cert.Status == AcmeCertificateStatus.Error)
                {
                    _logger.LogWarning(
                        "certificate {Domain} was in Error state on startup, resetting to Pending",
                        domain);

                    cert.Status = AcmeCertificateStatus.Pending;
                    cert.FailureCount = 0;
                    resetRequired = true;
                }
            }

            if (resetRequired)
            {
                await stateStore.SaveStateDataAsync(stateData, cancellationToken).ConfigureAwait(false);
                resetRequired = false;
            }

            foreach (var (_, cert) in stateData.Certificates)
            {
                if (cert.Status != AcmeCertificateStatus.Valid)
                {
                    continue;
                }

                try
                {
                    var pfxData = await stateStore.LoadCertificateAsync(cert.PrimaryDomain, cancellationToken)
                        .ConfigureAwait(false);

                    if (pfxData is not null)
                    {
                        using var x509 = X509CertificateLoader.LoadPkcs12(pfxData, string.Empty);
                        if (x509.NotAfter.ToUniversalTime() < DateTime.UtcNow)
                        {
                            _logger.LogWarning(
                                "certificate {Domain} has expired ({NotAfter}), skipping installation",
                                cert.PrimaryDomain,
                                x509.NotAfter);

                            cert.Status = AcmeCertificateStatus.Expired;
                            resetRequired = true;
                            continue;
                        }

                        InstallCertificate(cert, pfxData);

                        _logger.LogInformation("loaded persisted certificate for {Domain}", cert.PrimaryDomain);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "failed to load persisted certificate for {Domain}", cert.PrimaryDomain);
                    cert.Status = AcmeCertificateStatus.Pending;
                    cert.FailureCount = 0;
                    resetRequired = true;
                }
            }

            if (resetRequired)
            {
                await stateStore.SaveStateDataAsync(stateData, cancellationToken).ConfigureAwait(false);
            }

            var modified = false;
            foreach (var entry in config.AcmeCertificates)
            {
                var primaryDomain = entry.Domains switch
                {
                    [var d, ..] => d,
                    _ => throw new InvalidOperationException("no primary domain specified")
                };

                if (!stateData.Certificates.TryGetValue(primaryDomain, out var existing))
                {
                    var managed = new AcmeManagedCertificate
                    {
                        PrimaryDomain = primaryDomain,
                        Domains = entry.Domains,
                        ChallengeType = entry.Challenge,
                        Provider = entry.Provider,
                        Status = AcmeCertificateStatus.Pending
                    };

                    foreach (var (key, value) in entry.ProviderProperties)
                    {
                        managed.ProviderProperties[key] = value;
                    }

                    stateData.Certificates[primaryDomain] = managed;
                    modified = true;
                    _logger.LogDebug("added pending certificate for {Domain} from config", primaryDomain);
                }
                else
                {
                    var configChanged = false;

                    if (!existing.Domains.SequenceEqual(entry.Domains, StringComparer.OrdinalIgnoreCase))
                    {
                        existing.Domains = entry.Domains;
                        configChanged = true;
                    }

                    if (!string.Equals(existing.ChallengeType, entry.Challenge, StringComparison.OrdinalIgnoreCase))
                    {
                        existing.ChallengeType = entry.Challenge;
                        configChanged = true;
                    }

                    if (!string.Equals(existing.Provider, entry.Provider, StringComparison.OrdinalIgnoreCase))
                    {
                        existing.Provider = entry.Provider;
                        configChanged = true;
                    }

                    if (!PropertiesEqual(existing.ProviderProperties, entry.ProviderProperties))
                    {
                        existing.ProviderProperties.Clear();
                        foreach (var (key, value) in entry.ProviderProperties)
                        {
                            existing.ProviderProperties[key] = value;
                        }
                        configChanged = true;
                    }

                    if (configChanged)
                    {
                        _logger.LogInformation("configuration changed for {Domain}, reprovisioning", primaryDomain);

                        existing.Status = AcmeCertificateStatus.Pending;
                        existing.FailureCount = 0;
                        existing.ErrorMessage = null;
                        modified = true;
                    }
                }
            }

            var configDomains = new HashSet<string>(
                config.AcmeCertificates.Select(e => e.Domains[0]),
                StringComparer.OrdinalIgnoreCase);
            var staleDomains = stateData.Certificates.Keys.Where(d => !configDomains.Contains(d)).ToList();

            foreach (var domain in staleDomains)
            {
                _logger.LogInformation(
                    "certificate {Domain} removed from config, removing from state",
                    domain);

                if (_certificateManager is not null && stateData.Certificates.TryGetValue(domain, out var staleCert))
                {
                    foreach (var d in staleCert.Domains)
                    {
                        _certificateManager.RemoveMapping(d);
                    }

                    _certificateManager.RemoveCertificate(domain);
                }

                stateData.Certificates.Remove(domain);
                stateStore.DeleteCertificate(domain);
                modified = true;
            }

            if (modified)
            {
                await stateStore.SaveStateDataAsync(stateData, cancellationToken).ConfigureAwait(false);
            }

            _workerCts = new CancellationTokenSource();
            _renewalLoopTask = RenewalLoopAsync(_workerCts.Token);

            _state.TryTransition(to: State.Started, from: State.Starting);
            _logger.LogInformation("acme service started");
        }
        catch (Exception ex)
        {
            _state.TryTransition(to: State.Initial, from: State.Starting);
            _logger.LogError(ex, "acme service failed to start");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!_state.TryTransition(to: State.Disposed, from: [State.Started, State.Starting]))
        {
            return;
        }

        _logger.LogInformation("stopping acme service");

        _workerCts?.Cancel();

        if (_renewalLoopTask is not null)
        {
            await _renewalLoopTask.ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _state.TryTransition(to: State.Disposed, from: [State.Initial, State.Starting, State.Started]);

        try
        {
            _workerCts?.Cancel();
        }
        catch (ObjectDisposedException)
        {
        }

        if (_renewalLoopTask is { IsCompleted: true, IsFaulted: true })
        {
            _renewalLoopTask.Exception?.Handle(_ => true);
        }

        _workerCts?.Dispose();
    }

    private async Task RenewalLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await ProcessAllCertificatesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "unexpected error in renewal cycle");
            }

            try
            {
                await Task.Delay(RenewalCheckInterval, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    private async Task ProcessAllCertificatesAsync(CancellationToken cancellationToken)
    {
        var stateData = stateStore.GetStateData();

        foreach (var cert in stateData.Certificates.Values.ToList())
        {
            cancellationToken.ThrowIfCancellationRequested();

            var renewalDue = cert.Status == AcmeCertificateStatus.Valid && IsRenewalDue(cert);

            var shouldProvision = cert.Status switch
            {
                AcmeCertificateStatus.Pending => true,
                AcmeCertificateStatus.Validating => true,
                AcmeCertificateStatus.Error when cert.FailureCount < MaxFailureCount => true,
                AcmeCertificateStatus.RenewalDue => true,
                AcmeCertificateStatus.Valid when renewalDue => true,
                AcmeCertificateStatus.Expired => true,
                _ => false
            };

            if (!shouldProvision)
            {
                continue;
            }

            if (cert.Status == AcmeCertificateStatus.Valid && cert.ExpiresAtUtc <= DateTime.UtcNow)
            {
                cert.Status = AcmeCertificateStatus.Expired;
                _logger.LogWarning("certificate for {Domain} has expired", cert.PrimaryDomain);
            }
            else if (renewalDue)
            {
                cert.Status = AcmeCertificateStatus.RenewalDue;
                _logger.LogInformation("certificate for {Domain} is due for renewal", cert.PrimaryDomain);
            }

            if (cert.Status is AcmeCertificateStatus.Expired or AcmeCertificateStatus.RenewalDue)
            {
                await stateStore.SaveStateDataAsync(stateData, cancellationToken).ConfigureAwait(false);
            }

            await ProvisionCertificateAsync(cert, cancellationToken).ConfigureAwait(false);
            await stateStore.SaveStateDataAsync(stateData, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task ProvisionCertificateAsync(AcmeManagedCertificate cert, CancellationToken cancellationToken)
    {
        _logger.LogInformation("provisioning certificate for {Domain}", cert.PrimaryDomain);

        cert.Status = AcmeCertificateStatus.Validating;
        cert.LastAttemptAtUtc = DateTime.UtcNow;

        await stateStore.SaveStateDataAsync(stateStore.GetStateData(), cancellationToken).ConfigureAwait(false);

        var processor = challengeProcessors.FirstOrDefault(p =>
            string.Equals(p.ChallengeType, cert.ChallengeType, StringComparison.OrdinalIgnoreCase));

        if (processor is null)
        {
            _logger.LogError("no challenge processor for '{ChallengeType}'", cert.ChallengeType);

            cert.Status = AcmeCertificateStatus.Error;
            cert.FailureCount++;
            cert.ErrorMessage = $"no challenge processor for '{cert.ChallengeType}'";

            return;
        }

        IAsyncDisposable? cleanup = null;

        try
        {
            var orderResult = await acmeClient.OrderCertificateAsync(
                cert.Domains,
                cert.ChallengeType,
                cancellationToken).ConfigureAwait(false);

            cleanup = await processor.PrepareAsync(
                orderResult.Authorizations,
                acmeClient.AccountKey,
                cert.Provider,
                cert.ProviderProperties,
                cancellationToken).ConfigureAwait(false);

            foreach (var auth in orderResult.Authorizations)
            {
                var valid = await acmeClient.ValidateChallengeAsync(
                    auth.Challenge,
                    cancellationToken).ConfigureAwait(false);

                if (!valid)
                {
                    throw new InvalidOperationException($"ACME challenge validation failed for {auth.Domain}");
                }
            }

            var certResult = await acmeClient.FinalizeOrderAsync(
                orderResult.Order,
                cert.Domains,
                cancellationToken).ConfigureAwait(false);

            if (certResult is null)
            {
                throw new InvalidOperationException("certificate finalization failed");
            }

            await stateStore.SaveCertificateAsync(
                cert.PrimaryDomain,
                certResult.Pfx,
                cancellationToken).ConfigureAwait(false);

            InstallCertificate(cert, certResult.Pfx);

            cert.Status = AcmeCertificateStatus.Valid;
            cert.ExpiresAtUtc = certResult.ExpiresAtUtc;
            cert.LastRenewalAtUtc = DateTime.UtcNow;
            cert.FailureCount = 0;
            cert.ErrorMessage = null;

            _logger.LogInformation(
                "certificate provisioned for {Domain}, expires {ExpiresAt}",
                cert.PrimaryDomain,
                cert.ExpiresAtUtc);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "failed to provision certificate for {Domain}", cert.PrimaryDomain);
            cert.Status = AcmeCertificateStatus.Error;
            cert.FailureCount++;
            cert.ErrorMessage = ex.Message;
        }
        finally
        {
            if (cleanup is not null)
            {
                try
                {
                    await cleanup.DisposeAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "failed to clean up challenge resources");
                }
            }
        }
    }

    private void InstallCertificate(AcmeManagedCertificate cert, byte[] pfx)
    {
        if (_certificateManager is null)
        {
            _logger.LogWarning(
                "TLS extension not loaded, certificate for {Domain} saved to disk but not installed",
                cert.PrimaryDomain);
            return;
        }

        var x509 = X509CertificateLoader.LoadPkcs12(pfx, password: string.Empty);

        _certificateManager.ReplaceCertificate(cert.PrimaryDomain, x509);

        foreach (var domain in cert.Domains)
        {
            _certificateManager.TryAddMapping(domain, cert.PrimaryDomain);
        }

        _logger.LogDebug("installed certificate for {Domain} into certificate manager", cert.PrimaryDomain);
    }

    private bool IsRenewalDue(AcmeManagedCertificate cert)
    {
        if (cert.ExpiresAtUtc is null)
        {
            return true;
        }

        return cert.ExpiresAtUtc.Value - DateTime.UtcNow < TimeSpan.FromDays(config.AcmeRenewalLead);
    }

    private static bool PropertiesEqual(Dictionary<string, string> a, Dictionary<string, string> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }

        foreach (var (key, value) in a)
        {
            if (!b.TryGetValue(key, out var other) || !string.Equals(value, other, StringComparison.Ordinal))
            {
                return false;
            }
        }

        return true;
    }
}
