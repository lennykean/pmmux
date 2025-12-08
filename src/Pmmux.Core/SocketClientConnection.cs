using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal class SocketClientConnection(
    ClientInfo client,
    IReadOnlyDictionary<string, string> properties,
    Socket clientSocket,
    Stream clientStream,
    ILoggerFactory loggerFactory,
    int mtu,
    long? previewSizeLimit,
    IMetricReporter metricReporter) : IClientConnection
{
    private enum State
    {
        Initial = 0,
        Preview = 1,
        Started = 2,
        Disposed = 3
    }

    private readonly Pipe _ingressPipe = new();
    private readonly Pipe _egressPipe = new();
    private readonly MemoryStream _previewBuffer = new();
    private readonly TaskCompletionSource<bool> _disposedTsc = new();
    private readonly CancellationTokenSource _workerCts = new();
    private readonly StateManager<State> _state = new(State.Initial);
    private readonly ILogger _logger = loggerFactory.CreateLogger("client-connection");
    private readonly Dictionary<string, string?> _baseMetadata = new()
    {
        ["client"] = client.RemoteEndpoint?.ToString()
    };

    private Task _ingressTask = Task.CompletedTask;
    private Task _egressTask = Task.CompletedTask;
    private ClientConnectionPreview? _activePreview;

    public ClientInfo Client => client;
    public IReadOnlyDictionary<string, string> Properties => properties;

    public PipeReader GetReader()
    {
        if (_state.Is(State.Disposed))
        {
            throw new ObjectDisposedException(nameof(SocketClientConnection));
        }
        if (_state.TryTransition(to: State.Started, from: State.Initial))
        {
            _ingressTask = ReadClientStreamAsync();
            _egressTask = WriteClientStreamAsync();
        }
        if (!_state.Is(State.Started))
        {
            throw new InvalidOperationException();
        }
        return _ingressPipe.Reader;
    }

    public PipeWriter GetWriter()
    {
        if (_state.Is(State.Disposed))
        {
            throw new ObjectDisposedException(nameof(SocketClientConnection));
        }
        if (_state.TryTransition(to: State.Started, from: State.Initial))
        {
            _ingressTask = ReadClientStreamAsync();
            _egressTask = WriteClientStreamAsync();
        }
        if (!_state.Is(State.Started))
        {
            throw new InvalidOperationException();
        }
        return _egressPipe.Writer;
    }

    public async Task CloseAsync()
    {
        if (_state.Is(State.Disposed))
        {
            throw new ObjectDisposedException(nameof(SocketClientConnection));
        }

        await CloseInternalAsync().ConfigureAwait(false);
    }

    public IClientConnectionPreview Preview()
    {
        if (!_state.TryTransition(to: State.Preview, from: State.Initial))
        {
            throw new InvalidOperationException();
        }

        _logger.LogTrace("starting preview with {PreviewLength} byte buffer", _previewBuffer.Length);
        _activePreview = new ClientConnectionPreview(
            Client,
            Properties,
            clientStream,
            _previewBuffer,
            (mtu, mtu),
            previewSizeLimit,
            () =>
            {
                _activePreview = null;
                _state.TryTransition(to: State.Initial, from: State.Preview);
                _logger.LogTrace("preview ended with {PreviewLength} byte buffer", _previewBuffer.Length);
            });

        return _activePreview;
    }

    public async ValueTask DisposeAsync()
    {
        if (!_state.TryTransition(to: State.Disposed, from: [State.Started, State.Preview, State.Initial]))
        {
            await _disposedTsc.Task.ConfigureAwait(false);
            return;
        }

        if (_activePreview is not null)
        {
            await _activePreview.DisposeAsync().ConfigureAwait(false);
        }

        await CloseInternalAsync().ConfigureAwait(false);

        _disposedTsc.SetResult(true);

        _workerCts.Dispose();
        _previewBuffer.Dispose();
        clientStream.Dispose();
        clientSocket.Dispose();
    }

    private async Task ReadClientStreamAsync()
    {
        var previewBuffer = _previewBuffer.ToArray();

        metricReporter.ReportCounter("client.ingress.bytes", "multiplexer", previewBuffer.Length, new(_baseMetadata)
        {
            ["source"] = "preview_buffer"
        });

        await _ingressPipe.Writer.WriteAsync(previewBuffer).ConfigureAwait(false);

        _previewBuffer.Seek(0, SeekOrigin.Begin);
        _previewBuffer.SetLength(0);

        while (!_workerCts.IsCancellationRequested)
        {
            try
            {
                var pipeMemory = _ingressPipe.Writer.GetMemory();

                var received = await metricReporter.MeasureDurationAsync(
                    "client.ingress.read_duration",
                    "multiplexer",
                    _baseMetadata,
                    async () => await clientStream.ReadAsync(
                        pipeMemory,
                        _workerCts.Token).ConfigureAwait(false)).ConfigureAwait(false);

                if (received == 0)
                {
                    break;
                }

                metricReporter.ReportCounter("client.ingress.bytes", "multiplexer", received, new(_baseMetadata)
                {
                    ["source"] = "stream"
                });

                _ingressPipe.Writer.Advance(received);

                using (metricReporter.MeasureDuration("client.ingress.flush_duration", "multiplexer", _baseMetadata))
                {
                    await _ingressPipe.Writer.FlushAsync(_workerCts.Token).ConfigureAwait(false);
                }
            }
            catch (Exception ex) when (
                ex is OperationCanceledException ||
                (ex is SocketException sEx && sEx.IsShutdownSignal()) ||
                (ex is IOException iEx && iEx.IsShutdownSignal()))
            {
                break;
            }
            catch (Exception ex)
            {
                await _ingressPipe.Writer.FlushAsync(_workerCts.Token).ConfigureAwait(false);
                await _ingressPipe.Writer.CompleteAsync(ex).ConfigureAwait(false);

                return;
            }
        }
        await _ingressPipe.Writer.FlushAsync(_workerCts.Token).ConfigureAwait(false);
        await _ingressPipe.Writer.CompleteAsync().ConfigureAwait(false);
    }

    private async Task WriteClientStreamAsync()
    {
        while (!_workerCts.IsCancellationRequested)
        {
            try
            {
                var result = await metricReporter.MeasureDurationAsync(
                    "client.egress.read_duration",
                    "multiplexer",
                    _baseMetadata,
                    async () => await _egressPipe.Reader.ReadAsync(
                        _workerCts.Token).ConfigureAwait(false)).ConfigureAwait(false);

                if (result.IsCanceled)
                {
                    break;
                }

                foreach (var segment in result.Buffer)
                {
                    using (metricReporter.MeasureDuration("client.egress.write_duration", "multiplexer", _baseMetadata))
                    {
                        await clientStream.WriteAsync(segment, _workerCts.Token).ConfigureAwait(false);
                    }
                    metricReporter.ReportCounter("client.egress.bytes", "multiplexer", segment.Length, _baseMetadata);
                }
                _egressPipe.Reader.AdvanceTo(result.Buffer.End);
            }
            catch (Exception ex) when (
                ex is OperationCanceledException ||
                (ex is SocketException sEx && sEx.IsShutdownSignal()) ||
                (ex is IOException iEx && iEx.IsShutdownSignal()))
            {
                break;
            }
            catch (Exception ex)
            {
                await _egressPipe.Reader.CompleteAsync(ex).ConfigureAwait(false);

                return;
            }
        }
        await _egressPipe.Reader.CompleteAsync().ConfigureAwait(false);
    }

    private async Task CloseInternalAsync()
    {
        _workerCts.Cancel();

        try
        {
            if (clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            clientSocket.Close();
        }
        catch
        {
        }

        try
        {
            clientStream.Close();
        }
        catch
        {
        }

        await Task.WhenAll(_ingressTask, _egressTask).ConfigureAwait(false);
    }
}

