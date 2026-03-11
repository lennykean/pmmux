using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using Certes;
using Certes.Acme;
using Certes.Acme.Resource;

using Microsoft.Extensions.Logging;

using Pmmux.Core;
using Pmmux.Extensions.Acme.Abstractions;
using Pmmux.Extensions.Acme.Models;

namespace Pmmux.Extensions.Acme;

internal sealed class AcmeClient(
    AcmeConfig config,
    AcmeStateStore stateStore,
    ILoggerFactory loggerFactory) : IDisposable
{
    private enum State
    {
        Initial = 0,
        Initializing,
        Initialized,
        Disposed
    }

    private static readonly TimeSpan ChallengeValidationTimeout = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan ChallengeValidationPollInterval = TimeSpan.FromSeconds(10);
    private static readonly TimeSpan ChallengeFinalizationTimeout = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan ChallengeFinalizationPollInterval = TimeSpan.FromSeconds(10);

    private readonly StateManager<State> _state = new(State.Initial);

    private readonly ILogger _logger = loggerFactory.CreateLogger("acme-client");
    private AcmeContext? _acmeContext;

    public IKey AccountKey => _acmeContext?.AccountKey ?? throw new InvalidOperationException();

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (_state.Is(State.Initialized))
        {
            return;
        }
        if (_state.Is(State.Disposed))
        {
            throw new ObjectDisposedException(nameof(AcmeClient));
        }
        if (!_state.TryTransition(to: State.Initializing, from: State.Initial))
        {
            throw new InvalidOperationException();
        }

        try
        {
            var serverUri = ResolveServerUri();
            _logger.LogDebug("using acme server {ServerUri}", serverUri);

            var existingAccount = stateStore.GetAccountInfo();
            if (existingAccount?.AccountKeyPem is not null)
            {
                var accountKey = KeyFactory.FromPem(existingAccount.AccountKeyPem);
                _acmeContext = new AcmeContext(serverUri, accountKey);

                await _acmeContext.Account().ConfigureAwait(false);
                _logger.LogDebug("loaded acme account {AccountUri}", existingAccount.AccountUri);
            }
            else
            {
                _acmeContext = new AcmeContext(serverUri);

                var contact = string.IsNullOrEmpty(config.AcmeEmail)
                    ? Array.Empty<string>()
                    : [$"mailto:{config.AcmeEmail}"];
                var account = await _acmeContext.NewAccount(contact, termsOfServiceAgreed: true).ConfigureAwait(false);
                var accountUri = account.Location;

                await stateStore.SaveAccountInfoAsync(new()
                {
                    AccountKeyPem = _acmeContext.AccountKey.ToPem(),
                    Email = config.AcmeEmail,
                    AccountUri = accountUri?.ToString()
                }, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("registered acme account with {ServerUri}", serverUri);
            }

            _state.TryTransition(to: State.Initialized, from: State.Initializing);
        }
        catch
        {
            _state.TryTransition(to: State.Initial, from: State.Initializing);
            throw;
        }
    }

    public async Task<AcmeOrderResult> OrderCertificateAsync(
        IReadOnlyList<string> domains,
        string challengeType,
        CancellationToken cancellationToken)
    {
        if (!_state.Is(State.Initialized))
        {
            throw new InvalidOperationException("ACME client not initialized");
        }

        _logger.LogDebug("creating certificate order for {Domains}", string.Join(", ", domains));

        var order = await _acmeContext!.NewOrder([.. domains]).ConfigureAwait(false);
        var authorizations = new List<AuthorizationInfo>();

        foreach (var auth in await order.Authorizations().ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var authResource = await auth.Resource().ConfigureAwait(false);
            var challenge = await GetChallengeContextAsync(auth, challengeType).ConfigureAwait(false);
            var domain = authResource.Identifier.Value;

            authorizations.Add(new(domain, challenge));

            _logger.LogDebug("obtained {ChallengeType} challenge for {Domain}", challengeType, domain);
        }

        return new(order, authorizations);
    }

    public async Task<bool> ValidateChallengeAsync(IChallengeContext challenge, CancellationToken cancellationToken)
    {
        if (!_state.Is(State.Initialized))
        {
            throw new InvalidOperationException("ACME client not initialized");
        }

        _logger.LogDebug("validating challenge");

        await challenge.Validate().ConfigureAwait(false);

        using var timeoutToken = new CancellationTokenSource(ChallengeValidationTimeout);
        using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutToken.Token);

        while (!linkedToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(ChallengeValidationPollInterval, linkedToken.Token).ConfigureAwait(false);

                var resource = await challenge.Resource().ConfigureAwait(false);

                _logger.LogDebug("challenge status: {Status}", resource.Status);

                if (resource.Status == ChallengeStatus.Valid)
                {
                    _logger.LogDebug("challenge validated");
                    return true;
                }
                if (resource.Status == ChallengeStatus.Invalid)
                {
                    _logger.LogError("challenge failed: {Error}", resource.Error?.Detail);
                    return false;
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "error occurred while validating challenge");
            }
        }

        _logger.LogError("challenge validation timed out after {Timeout}", ChallengeValidationTimeout);
        return false;
    }

    public async Task<AcmeCertificateResult?> FinalizeOrderAsync(
        IOrderContext order,
        IReadOnlyList<string> domains,
        CancellationToken cancellationToken)
    {
        if (!_state.Is(State.Initialized))
        {
            throw new InvalidOperationException("ACME client not initialized");
        }

        var domain = domains switch
        {
            [var d, ..] => d,
            _ => throw new ArgumentException("domains cannot be empty", nameof(domains))
        };
        _logger.LogDebug("finalizing certificate order for {Domain}", domain);

        var privateKey = KeyFactory.NewKey(KeyAlgorithm.ES256);
        var csrInfo = new CsrInfo { CommonName = domain };

        var orderResource = await order.Resource().ConfigureAwait(false);

        if (orderResource.Status == OrderStatus.Ready)
        {
            await order.Finalize(csrInfo, privateKey).ConfigureAwait(false);
        }

        using var timeoutToken = new CancellationTokenSource(ChallengeFinalizationTimeout);
        using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutToken.Token);

        while (!linkedToken.IsCancellationRequested)
        {
            orderResource = await order.Resource().ConfigureAwait(false);

            switch (orderResource.Status)
            {
                case OrderStatus.Valid:
                    break;
                case OrderStatus.Processing:
                    _logger.LogDebug(
                        "order is processing, polling again in {Interval}s",
                        ChallengeFinalizationPollInterval.TotalSeconds);
                    await Task.Delay(ChallengeFinalizationPollInterval, linkedToken.Token).ConfigureAwait(false);
                    continue;
                case OrderStatus.Invalid:
                    _logger.LogError("order is invalid, aborting finalization");
                    return null;
                default:
                    _logger.LogError("unexpected order status {Status}", orderResource.Status);
                    return null;
            }

            break;
        }

        if (orderResource.Status != OrderStatus.Valid)
        {
            _logger.LogError("certificate finalization timed out");
            return null;
        }

        var certChain = await order.Download().ConfigureAwait(false);

        byte[] pfx;
        if (config.AcmeStaging)
        {
            var leafPem = new string(PemEncoding.Write("CERTIFICATE", certChain.Certificate.ToDer()));
            var keyPem = privateKey.ToPem();

            using var ephemeral = X509Certificate2.CreateFromPem(leafPem, keyPem);

            pfx = ephemeral.Export(X509ContentType.Pkcs12, string.Empty);
        }
        else
        {
            pfx = certChain.ToPfx(privateKey).Build(domain, string.Empty);
        }

        using var x509 = X509CertificateLoader.LoadPkcs12(pfx, string.Empty);

        var expiresAtUtc = x509.NotAfter.ToUniversalTime();

        _logger.LogDebug("certificate issued for {Domain}, expires {ExpiresAt}", domain, expiresAtUtc);

        return new AcmeCertificateResult(pfx, expiresAtUtc);
    }

    public void Dispose()
    {
        _state.TryTransition(to: State.Disposed, from: [State.Initial, State.Initializing, State.Initialized]);
    }

    private static async Task<IChallengeContext> GetChallengeContextAsync(
        IAuthorizationContext authContext,
        string challengeType)
    {
        return challengeType switch
        {
            "dns-01" => await authContext.Dns().ConfigureAwait(false),
            _ => throw new ArgumentException($"unsupported challenge type: {challengeType}")
        };
    }

    private Uri ResolveServerUri()
    {
        if (!string.IsNullOrEmpty(config.AcmeServerUrl))
        {
            return new Uri(config.AcmeServerUrl);
        }

        return config.AcmeStaging ? WellKnownServers.LetsEncryptStagingV2 : WellKnownServers.LetsEncryptV2;
    }
}
