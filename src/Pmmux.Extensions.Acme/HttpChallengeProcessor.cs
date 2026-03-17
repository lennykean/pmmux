using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Certes;

using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;
using Pmmux.Extensions.Acme.Abstractions;

namespace Pmmux.Extensions.Acme;

internal sealed class HttpChallengeProcessor(
    IRouter router,
    IPortMultiplexer portMultiplexer,
    IPortWarden portWarden,
    HttpChallengeBackend.Protocol backendProtocol,
    ILoggerFactory loggerFactory) : IChallengeProcessor
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("acme-http-challenge");

    private HttpChallengeBatch? _batch;

    public string ChallengeType => "http-01";

    public Task<IAsyncDisposable> InitializeBatchAsync(CancellationToken cancellationToken)
    {
        _batch = new HttpChallengeBatch(router, portMultiplexer, portWarden, backendProtocol, _logger);
        _logger.LogDebug("initialized http-01 batch");
        return Task.FromResult<IAsyncDisposable>(_batch);
    }

    public async Task<IAsyncDisposable> PrepareAsync(
        IEnumerable<AuthorizationInfo> authorizations,
        IKey accountKey,
        string? provider,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken)
    {
        if (_batch is null)
        {
            throw new InvalidOperationException("http-01 processor requires batch initialization");
        }

        await _batch.EnsureInfrastructureAsync(properties, cancellationToken).ConfigureAwait(false);

        var addedTokens = new List<string>();

        try
        {
            foreach (var auth in authorizations)
            {
                var keyAuth = auth.Challenge.KeyAuthz;

                _logger.LogDebug(
                    "registering http-01 challenge for {Domain}, token: {Token}",
                    auth.Domain,
                    auth.Challenge.Token);

                _batch.Backend.AddChallenge(auth.Challenge.Token, keyAuth);
                addedTokens.Add(auth.Challenge.Token);
            }
        }
        catch
        {
            foreach (var token in addedTokens)
            {
                _batch.Backend.RemoveChallenge(token);
            }
            throw;
        }

        return new ChallengeCleanupHandler(_batch.Backend, addedTokens, _logger);
    }

    private sealed class ChallengeCleanupHandler(
        HttpChallengeBackend backend,
        List<string> tokens,
        ILogger logger) : IAsyncDisposable
    {
        public ValueTask DisposeAsync()
        {
            foreach (var token in tokens)
            {
                backend.RemoveChallenge(token);
                logger.LogDebug("removed http-01 challenge token: {Token}", token);
            }

            return default;
        }
    }
}
