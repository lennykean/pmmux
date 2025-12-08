using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal class ClientConnectionPreview : IClientConnectionPreview
{
    private readonly CancellationTokenSource _cts = new();
    private readonly Pipe _previewPipe;
    private readonly Task _feedTask;
    private readonly Action? _onDisposed;

    public ClientConnectionPreview(
        ClientInfo client,
        IReadOnlyDictionary<string, string> properties,
        Stream clientStream,
        MemoryStream accumulator,
        (int pauseWriterThreshold, int resumeWriterThreshold) flowControl,
        long? previewSizeLimit = null,
        Action? onDisposed = null)
    {
        Client = client;
        Properties = properties;
        _onDisposed = onDisposed;

        var pipeOptions = new PipeOptions(
            pauseWriterThreshold: flowControl.pauseWriterThreshold,
            resumeWriterThreshold: flowControl.resumeWriterThreshold);

        _previewPipe = new Pipe(pipeOptions);

        _feedTask = FeedPreviewAsync(clientStream, accumulator, previewSizeLimit);
    }

    public ClientInfo Client { get; }
    public IReadOnlyDictionary<string, string> Properties { get; }

    public PipeReader Ingress => _previewPipe.Reader;

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();

        await _previewPipe.Writer.CompleteAsync().ConfigureAwait(false);
        await _previewPipe.Reader.CompleteAsync().ConfigureAwait(false);

        await _feedTask.ConfigureAwait(false);

        _onDisposed?.Invoke();

        _cts.Dispose();
    }

    private async Task FeedPreviewAsync(Stream clientStream, MemoryStream accumulator, long? previewSizeLimit)
    {
        try
        {
            var preload = accumulator.ToArray();
            long previewSize = preload.Length;

            await _previewPipe.Writer.WriteAsync(preload, _cts.Token).ConfigureAwait(false);

            while (!_cts.IsCancellationRequested)
            {
                await _previewPipe.Writer.FlushAsync(_cts.Token).ConfigureAwait(false);

                var pipeMemory = _previewPipe.Writer.GetMemory();
                var readBytes = pipeMemory.Span.Length;
                if (readBytes + previewSize > previewSizeLimit)
                {
                    readBytes = (int)(previewSizeLimit - previewSize);
                }
                var received = await clientStream.ReadAsync(pipeMemory[..readBytes], _cts.Token).ConfigureAwait(false);

                if (received == 0)
                {
                    break;
                }
                previewSize += received;

                accumulator.Write(pipeMemory[..received].Span);
                _previewPipe.Writer.Advance(received);

                if (previewSize >= previewSizeLimit)
                {
                    break;
                }
            }
        }
        catch (Exception ex) when (
            ex is OperationCanceledException ||
            (ex is SocketException sEx && sEx.IsShutdownSignal()) ||
            (ex is IOException iEx && iEx.IsShutdownSignal()))
        {
        }
        catch (Exception ex)
        {
            await _previewPipe.Writer.FlushAsync().ConfigureAwait(false);
            await _previewPipe.Writer.CompleteAsync(ex).ConfigureAwait(false);
            throw;
        }
        await _previewPipe.Writer.FlushAsync().ConfigureAwait(false);
        await _previewPipe.Writer.CompleteAsync().ConfigureAwait(false);
    }
}
