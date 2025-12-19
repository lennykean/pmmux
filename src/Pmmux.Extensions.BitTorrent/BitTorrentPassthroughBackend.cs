using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;
using Pmmux.Core;

namespace Pmmux.Extensions.BitTorrent;

internal sealed class BitTorrentPassthroughBackend(
    BackendSpec backendSpec,
    ILogger<BitTorrentPassthroughBackend> logger) : PassthroughBackend(backendSpec)
{
    new internal class Protocol(ILogger<BitTorrentPassthroughBackend> logger) : IBackendProtocol
    {
        public string Name => "bittorrent-pass";

        public Task<IBackend> CreateBackendAsync(BackendSpec spec, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IBackend>(new BitTorrentPassthroughBackend(spec, logger));
        }
    }

    /// <inheritdoc />
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

            var isBitTorrent = IsBitTorrent(result.Buffer, out var infoHash, out var peerId);

            if (isBitTorrent is true)
            {
                logger.LogTrace(
                    "received connection from {Client}: info_hash={InfoHash}, peer_id={PeerId}",
                    clientPreview.Client.RemoteEndpoint,
                    infoHash,
                    peerId);

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

    /// <inheritdoc />
    public override Task<bool> CanHandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageMetadata,
        ReadOnlyMemory<byte> message,
        CancellationToken cancellationToken = default)
    {
        if (!MatchesClient(client, messageMetadata))
        {
            return Task.FromResult(false);
        }

        if (TryParseUtp(message.Span, out var utpType, out var connectionId, out var seqNr, out var ackNr))
        {
            logger.LogTrace(
                "received uTP message from {Client}: type={Type}, connection_id={ConnectionId}, " +
                "seq_nr={SequenceNr}, ack_nr={AcknowledgementNr}",
                client.RemoteEndpoint,
                utpType,
                connectionId,
                seqNr,
                ackNr);

            return Task.FromResult(true);
        }

        if (TryParseUdpTracker(message.Span, out var action, out var transactionId))
        {
            logger.LogTrace(
                "received tracker message from {Client}: action={Action}, transaction_id={TransactionId}",
                client.RemoteEndpoint,
                action,
                transactionId);

            return Task.FromResult(true);
        }

        if (TryParseDht(message.Span, out var messageType))
        {
            logger.LogTrace(
                "received DHT message from {Client}: type={Type}",
                client.RemoteEndpoint,
                messageType);

            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    private static bool? IsBitTorrent(ReadOnlySequence<byte> data, out string? infoHash, out string? peerId)
    {
        infoHash = null;
        peerId = null;

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

        data = data.Slice(protocol.Length);

        if (data.Length < 48)
        {
            return null;
        }

        infoHash = Convert.ToHexString(data.Slice(8, 20).ToArray()).ToLowerInvariant();
        peerId = Convert.ToHexString(data.Slice(28, 20).ToArray()).ToLowerInvariant();

        return true;
    }

    private static bool TryParseUtp(
        ReadOnlySpan<byte> data,
        [NotNullWhen(true)] out string? type,
        out uint connectionId,
        out ushort sequence,
        out ushort acknowledgement)
    {
        type = default;
        connectionId = default;
        sequence = default;
        acknowledgement = default;

        if (data.Length < 24)
        {
            return false;
        }

        var typeVersion = data[0];
        var version = typeVersion & 0x0F;

        type = ((typeVersion >> 4) & 0x0F) switch
        {
            0 => "DATA",
            1 => "FIN",
            2 => "STATE",
            3 => "RESET",
            4 => "SYN",
            var t => t.ToString()
        };

        if (version != 1)
        {
            return false;
        }

        connectionId = BinaryPrimitives.ReadUInt32BigEndian(data[4..8]);
        sequence = BinaryPrimitives.ReadUInt16BigEndian(data[20..22]);
        acknowledgement = BinaryPrimitives.ReadUInt16BigEndian(data[22..24]);

        return true;
    }

    private static bool TryParseUdpTracker(
        ReadOnlySpan<byte> data,
        [NotNullWhen(true)] out string? action,
        out uint transactionId)
    {
        if (data.Length < 16 || BinaryPrimitives.ReadUInt64BigEndian(data[0..8]) != 0x41727101980L)
        {
            action = default;
            transactionId = default;

            return false;
        }

        action = BinaryPrimitives.ReadUInt32BigEndian(data[8..12]) switch
        {
            0 => "CONNECT",
            1 => "ANNOUNCE",
            2 => "SCRAPE",
            3 => "ERROR",
            var a => a.ToString()
        };
        transactionId = BinaryPrimitives.ReadUInt32BigEndian(data[12..16]);

        return true;
    }

    private static bool TryParseDht(ReadOnlySpan<byte> data, out string messageType)
    {
        messageType = string.Empty;

        if (data.Length < 8 || data[0] != (byte)'d' || data[^1] != (byte)'e')
        {
            return false;
        }

        var hasTransactionId = false;
        var hasMessageType = false;

        for (var i = 1; i < data.Length - 3; i++)
        {
            if (data[i] == (byte)'1' && data[i + 1] == (byte)':' && data[i + 2] == (byte)'t')
            {
                hasTransactionId = true;
                if (hasMessageType)
                {
                    break;
                }
            }
            else if (data[i] == (byte)'1' && data[i + 1] == (byte)':' && data[i + 2] == (byte)'y')
            {
                hasMessageType = true;
                var valueStart = i + 3;
                if (valueStart < data.Length && data[valueStart] >= (byte)'0' && data[valueStart] <= (byte)'9')
                {
                    var lengthEnd = valueStart;
                    while (lengthEnd < data.Length && data[lengthEnd] >= (byte)'0' && data[lengthEnd] <= (byte)'9')
                    {
                        lengthEnd++;
                    }
                    if (lengthEnd < data.Length && data[lengthEnd] == (byte)':')
                    {
                        var strStart = lengthEnd + 1;
                        if (strStart < data.Length)
                        {
                            messageType = (char)data[strStart] switch
                            {
                                'q' => "query",
                                'r' => "response",
                                'e' => "error",
                                _ => "unknown"
                            };
                        }
                    }
                }
                if (hasTransactionId)
                {
                    break;
                }
            }
        }

        return hasTransactionId && hasMessageType;
    }
}

