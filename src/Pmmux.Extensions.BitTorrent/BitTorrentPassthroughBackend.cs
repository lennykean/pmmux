using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Pmmux.Abstractions;
using Pmmux.Core;

namespace Pmmux.Extensions.BitTorrent;

internal sealed class BitTorrentPassthroughBackend(BackendSpec backendSpec) : PassthroughBackend(backendSpec)
{
    new internal class Protocol : IBackendProtocol
    {
        public string Name => "bittorrent-pass";

        public Task<IBackend> CreateBackendAsync(BackendSpec spec, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IBackend>(new BitTorrentPassthroughBackend(spec));
        }
    }

    public override Task<bool> CanHandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageMetadata,
        ReadOnlyMemory<byte> message,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(IsBitTorrent(new ReadOnlySequence<byte>(message)) is true);
    }

    public override async Task<bool> CanHandleConnectionAsync(
        IClientConnectionPreview clientPreview,
        CancellationToken cancellationToken = default)
    {
        var preview = clientPreview.Ingress;

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await preview.ReadAsync(cancellationToken).ConfigureAwait(false);
            if (result.IsCanceled)
            {
                return false;
            }

            var isBitTorrent = IsBitTorrent(result.Buffer);

            if (isBitTorrent is true)
            {
                return true;
            }
            else if (isBitTorrent is false || result.IsCompleted)
            {
                return false;
            }

            preview.AdvanceTo(result.Buffer.Start, result.Buffer.End);
        }

        return false;
    }

    private static bool? IsBitTorrent(ReadOnlySequence<byte> data)
    {
        if (data.Length < 1)
        {
            return null;
        }

        var protocolLength = data.FirstSpan[0];
        if (protocolLength != 19)
        {
            return false;
        }
        data = data.Slice(1);

        var protocol = "BitTorrent protocol"u8;
        var protocolIndex = 0;
        foreach (var chunk in data)
        {
            var memory = chunk.Span;

            for (var i = 0; i < memory.Length && protocolIndex < protocol.Length; i++)
            {
                if (memory[i] != protocol[protocolIndex++])
                {
                    return false;
                }
            }
        }

        if (protocolIndex < protocol.Length)
        {
            return null;
        }

        return true;
    }
}

