using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal sealed class NoopBackend(BackendSpec spec) : IConnectionOrientedBackend, IConnectionlessBackend
{
    public sealed class Protocol : IBackendProtocol
    {
        public string Name => "noop";

        public Task<IBackend> CreateBackendAsync(BackendSpec spec, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IBackend>(new NoopBackend(spec));
        }
    }

    private class NoopConnection : IConnection
    {
        private readonly PipeReader _reader;
        private readonly PipeWriter _writer;

        public NoopConnection()
        {
            _reader = PipeReader.Create(Stream.Null);
            _writer = PipeWriter.Create(Stream.Null);
        }

        public IReadOnlyDictionary<string, string> Properties => new Dictionary<string, string>();

        public PipeReader GetReader() => _reader;
        public PipeWriter GetWriter() => _writer;

        public async Task CloseAsync()
        {
            await _reader.CompleteAsync().ConfigureAwait(false);
            await _writer.CompleteAsync().ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await CloseAsync().ConfigureAwait(false);
        }
    }

    public BackendInfo Backend { get; } = new(spec, new Dictionary<string, string>(), PriorityTier.Fallback);

    public Task<IConnection> CreateBackendConnectionAsync(
        IClientConnection client,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IConnection>(new NoopConnection());
    }

    public Task HandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageMetadata,
        byte[] message,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync(
        IClientWriterFactory clientWriterFactory,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<bool> CanHandleConnectionAsync(
        IClientConnectionPreview preview,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task<bool> CanHandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageMetadata,
        ReadOnlyMemory<byte> message,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public ValueTask DisposeAsync()
    {
        return default;
    }
}

