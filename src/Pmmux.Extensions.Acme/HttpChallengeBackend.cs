using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Pmmux.Abstractions;
using Pmmux.Core;

namespace Pmmux.Extensions.Acme;

internal sealed class HttpChallengeBackend(BackendSpec spec)
    : BackendBase(spec, [], PriorityTier.Vip), IConnectionOrientedBackend
{
    internal sealed class Protocol : IBackendProtocol
    {
        public string Name => ProtocolName;

        public HttpChallengeBackend? LastCreated { get; private set; }

        public Task<IBackend> CreateBackendAsync(BackendSpec spec, CancellationToken cancellationToken = default)
        {
            var backend = new HttpChallengeBackend(spec);
            LastCreated = backend;
            return Task.FromResult<IBackend>(backend);
        }
    }

    private const string ChallengePath = "/.well-known/acme-challenge/";
    private static readonly byte[] ChallengePathBytes = Encoding.ASCII.GetBytes(ChallengePath);

    internal const string ProtocolName = "acme-http-challenge";

    private readonly ConcurrentDictionary<string, string> _challenges = new(StringComparer.Ordinal);

    internal void AddChallenge(string token, string keyAuthorization)
    {
        _challenges[token] = keyAuthorization;
    }

    internal bool RemoveChallenge(string token)
    {
        return _challenges.TryRemove(token, out _);
    }

    public override Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task<bool> CanHandleConnectionAsync(
        IClientConnectionPreview client,
        CancellationToken cancellationToken)
    {
        var result = await client.Ingress.ReadAsync(cancellationToken).ConfigureAwait(false);
        if (result.IsCanceled)
        {
            return false;
        }

        var buffer = result.Buffer;

        try
        {
            return TryMatchChallengePath(buffer);
        }
        finally
        {
            client.Ingress.AdvanceTo(buffer.Start, buffer.End);
        }
    }

    public Task<IConnection> CreateBackendConnectionAsync(
        IClientConnection client,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IConnection>(new ChallengeConnection(client, _challenges));
    }

    public override ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        _challenges.Clear();
        return default;
    }

    private bool TryMatchChallengePath(ReadOnlySequence<byte> buffer)
    {
        Span<byte> scratch = stackalloc byte[Math.Min((int)buffer.Length, 512)];
        buffer.Slice(0, scratch.Length).CopyTo(scratch);

        var token = ExtractChallengeToken(scratch);
        return token is not null && _challenges.ContainsKey(token);
    }

    private static string? ExtractChallengeToken(ReadOnlySpan<byte> data)
    {
        var requestLine = data;
        var newlineIndex = requestLine.IndexOf((byte)'\n');
        if (newlineIndex >= 0)
        {
            requestLine = requestLine[..newlineIndex];
        }

        var spaceIndex = requestLine.IndexOf((byte)' ');
        if (spaceIndex < 0)
        {
            return null;
        }

        var afterMethod = requestLine[(spaceIndex + 1)..];
        var pathEnd = afterMethod.IndexOf((byte)' ');
        if (pathEnd < 0)
        {
            pathEnd = afterMethod.Length;
        }

        var pathBytes = afterMethod[..pathEnd];
        if (pathBytes.Length <= ChallengePathBytes.Length ||
            !pathBytes[..ChallengePathBytes.Length].SequenceEqual(ChallengePathBytes))
        {
            return null;
        }

        return Encoding.ASCII.GetString(pathBytes[ChallengePathBytes.Length..]);
    }

    private sealed class ChallengeConnection : IConnection
    {
        private readonly Pipe _responsePipe = new();
        private readonly Task _writeTask;

        public ChallengeConnection(IClientConnection client, ConcurrentDictionary<string, string> challenges)
        {
            _writeTask = WriteResponseAsync(client, challenges);
        }

        public IReadOnlyDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public PipeReader GetReader() => _responsePipe.Reader;
        public PipeWriter GetWriter() => _responsePipe.Writer;

        public async Task CloseAsync()
        {
            await _writeTask.ConfigureAwait(false);
            await _responsePipe.Reader.CompleteAsync().ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await CloseAsync().ConfigureAwait(false);
        }

        private async Task WriteResponseAsync(IClientConnection client, ConcurrentDictionary<string, string> challenges)
        {
            var token = await ExtractTokenAsync(client).ConfigureAwait(false);

            string body;
            int statusCode;

            if (token is not null && challenges.TryGetValue(token, out var keyAuth))
            {
                body = keyAuth;
                statusCode = 200;
            }
            else
            {
                body = "not found";
                statusCode = 404;
            }

            var bodyBytes = Encoding.UTF8.GetBytes(body);
            var response = $"HTTP/1.1 {statusCode} {(statusCode == 200 ? "OK" : "Not Found")}\r\n"
                + $"Content-Length: {bodyBytes.Length}\r\n"
                + "Content-Type: application/octet-stream\r\n"
                + "Connection: close\r\n"
                + "\r\n";

            var responseBytes = Encoding.ASCII.GetBytes(response);

            var writer = _responsePipe.Writer;
            await writer.WriteAsync(responseBytes).ConfigureAwait(false);
            await writer.WriteAsync(bodyBytes).ConfigureAwait(false);
            await writer.CompleteAsync().ConfigureAwait(false);
        }

        private static async Task<string?> ExtractTokenAsync(IClientConnection client)
        {
            var reader = client.GetReader();
            var result = await reader.ReadAsync().ConfigureAwait(false);
            var buffer = result.Buffer;

            try
            {
                Span<byte> scratch = stackalloc byte[Math.Min((int)buffer.Length, 512)];
                buffer.Slice(0, scratch.Length).CopyTo(scratch);

                return ExtractChallengeToken(scratch);
            }
            finally
            {
                reader.AdvanceTo(buffer.Start, buffer.End);
            }
        }
    }
}
