using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal abstract class BackendBroker(
    Protocol networkProtocol,
    IBackend backend,
    IBackendMonitor monitor,
    ILogger logger) : IAsyncDisposable
{
    private readonly CancellationTokenSource _workerCts = new();

    private Task _workerTask = Task.CompletedTask;

    public BackendInfo Backend => backend.Backend;
    public BackendStatusInfo Status { get; protected set; } = new(backend.Backend, BackendStatus.Unknown);

    protected ILogger Logger => logger;

    public virtual Task InitializeAsync(
        IClientWriterFactory clientWriterFactory,
        CancellationToken cancellationToken = default)
    {
        Status = new(backend.Backend, BackendStatus.Unknown);

        if (backend is IHealthCheckBackend healthCheckableBackend)
        {
            _workerTask = MonitorBackendAsync(healthCheckableBackend);
        }
        return Task.CompletedTask;
    }

    public virtual async ValueTask DisposeAsync()
    {
        await StopMonitorAsync().ConfigureAwait(false);
        await backend.DisposeAsync().ConfigureAwait(false);

        _workerCts.Dispose();
    }

    protected async Task StopMonitorAsync()
    {
        try
        {
            _workerCts.Cancel();

            await _workerTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task MonitorBackendAsync(IHealthCheckBackend healthCheckableBackend)
    {
        logger.LogTrace("monitoring backend");

        try
        {
            await foreach (var status in monitor.MonitorAsync(healthCheckableBackend, networkProtocol, _workerCts.Token)
                .WithCancellation(_workerCts.Token)
                .ConfigureAwait(false))
            {
                if (status.Status != BackendStatus.Unknown)
                {
                    logger.LogDebug(
                        "status changed: {BackendStatus}{StatusReason}",
                        status.Status,
                        status.StatusReason is null ? "" : $" ({status.StatusReason})");
                }
                Status = status;
            }
        }
        catch (OperationCanceledException)
        {
        }

        logger.LogTrace("monitoring stopped");
    }
}
