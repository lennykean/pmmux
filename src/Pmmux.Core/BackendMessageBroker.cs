using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal class BackendMessageBroker(
    IConnectionlessBackend backend,
    IBackendMonitor monitor,
    ILoggerFactory loggerFactory,
    IMetricReporter metricReporter)
    : BackendBroker(Protocol.Udp, backend, monitor, loggerFactory.CreateLogger("message-broker"))
{
    private enum State
    {
        Initial = 0,
        Initializing = 1,
        Initialized = 2,
        Dispose = 3
    }

    private readonly TaskCompletionSource<bool> _disposedTsc = new();
    private readonly StateManager<State> _state = new(State.Initial);
    private readonly Dictionary<string, string?> _baseMetadata = new()
    {
        ["backend"] = backend.Backend.Spec.Name
    };

    private IDisposable? _logScope;

    public override async Task InitializeAsync(
        IClientWriterFactory clientWriterFactory,
        CancellationToken cancellationToken = default)
    {
        if (!_state.TryTransition(to: State.Initializing, from: State.Initial))
        {
            throw new InvalidOperationException();
        }

        _logScope = Logger.BeginScope("{BackendName}:{BackendProtocol}", Backend.Spec.Name, Backend.Spec.ProtocolName);

        try
        {
            await backend.InitializeAsync(clientWriterFactory, cancellationToken).ConfigureAwait(false);
            await base.InitializeAsync(clientWriterFactory, cancellationToken).ConfigureAwait(false);

            Logger.LogTrace("initialized");

            _state.TryTransition(to: State.Initialized, from: State.Initializing);
        }
        catch
        {
            _state.TryTransition(to: State.Initial, from: State.Initializing);
        }
    }

    public async Task<bool> CanHandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> metadata,
        ReadOnlyMemory<byte> message,
        CancellationToken cancellationToken = default)
    {
        if (!_state.Is(State.Initialized))
        {
            return false;
        }
        Logger.LogTrace("backend probing {MessageLength} byte message from {RemoteEndpoint}",
            message.Length,
            client.RemoteEndpoint);

        var canHandle = await metricReporter.MeasureDurationAsync(
            "backend.message.probe.duration",
            "multiplexer",
            _baseMetadata,
            async () => await backend.CanHandleMessageAsync(
                client,
                metadata,
                message,
                cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

        if (canHandle is true)
        {
            Logger.LogTrace(
                "backend can handle {MessageLength} byte message from {RemoteEndpoint}",
                message.Length,
                client.RemoteEndpoint);
        }
        return canHandle;
    }

    public async Task HandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageMetadata,
        byte[] message,
        CancellationToken cancellationToken = default)
    {
        if (_state.Is(State.Dispose))
        {
            throw new ObjectDisposedException(nameof(BackendMessageBroker));
        }
        if (!_state.Is(State.Initialized))
        {
            throw new InvalidOperationException();
        }
        Logger.LogTrace("handling {MessageLength} byte message from {RemoteEndpoint} [{Metadata}]",
            message.Length,
            client.RemoteEndpoint,
            string.Join(",", messageMetadata.Select(x => $"{x.Key}: {x.Value}")));

        metricReporter.ReportEvent("backend.message.accepted", "multiplexer", _baseMetadata);
        metricReporter.ReportCounter("backend.message.bytes", "multiplexer", message.Length, _baseMetadata);

        using (metricReporter.MeasureDuration("backend.message.handle.duration", "multiplexer", _baseMetadata))
        {
            await backend.HandleMessageAsync(client, messageMetadata, message, cancellationToken).ConfigureAwait(false);
        }

        Logger.LogTrace(
            "backend accepted {MessageLength} byte message from {RemoteEndpoint}",
            message.Length,
            client.RemoteEndpoint);
    }

    public override async ValueTask DisposeAsync()
    {
        if (_state.Is(State.Dispose) ||
            !_state.TryTransition(to: State.Dispose, from: [State.Initialized, State.Initializing, State.Initial]))
        {
            await _disposedTsc.Task.ConfigureAwait(false);
            return;
        }

        await base.DisposeAsync().ConfigureAwait(false);

        Logger.LogTrace("message broker disposed");
        _logScope?.Dispose();

        if (Status.Status is not BackendStatus.Stopped)
        {
            Status = new(Backend, BackendStatus.Stopped);
            Logger.LogDebug(
                "status changed: {BackendStatus}{StatusReason}",
                Status.Status,
                Status.StatusReason is null ? "" : $" ({Status.StatusReason})");
        }

        _disposedTsc.SetResult(true);
    }
}
