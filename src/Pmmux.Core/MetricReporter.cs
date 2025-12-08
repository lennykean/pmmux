using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;

namespace Pmmux.Core;

/// <summary>
/// Default implementation of <see cref="IMetricReporter"/> that dispatches metrics to sinks.
/// </summary>
/// <param name="sinks">The collection of metric sinks to dispatch metrics to.</param>
/// <param name="loggerFactory">The logger factory.</param>
public sealed class MetricReporter(
    IEnumerable<IMetricSink> sinks,
    ILoggerFactory loggerFactory) : IMetricReporter, IAsyncDisposable
{
    private readonly record struct Capture(Metric Metric, DateTimeOffset Captured);

    private readonly Channel<Capture> _channel = Channel.CreateBounded<Capture>(new BoundedChannelOptions(1000)
    {
        FullMode = BoundedChannelFullMode.DropOldest,
        SingleReader = true,
        SingleWriter = false
    });
    private readonly CancellationTokenSource _disposalCts = new();
    private readonly ILogger _logger = loggerFactory.CreateLogger("metrics-reporter");

    private volatile int _started = 0;
    private Task _dispatcherTask = Task.CompletedTask;

    void IMetricReporter.ReportMetric<T>(Metric<T> metric)
    {
        EnsureDispatcherStarted();

        var captured = new Capture(metric, DateTimeOffset.UtcNow);

        if (!_channel.Writer.TryWrite(captured))
        {
            _logger.LogWarning("metrics buffer is full, some metrics may be dropped");
        }
    }

    private void EnsureDispatcherStarted()
    {
        if (Interlocked.CompareExchange(ref _started, 1, 0) != 0)
        {
            return;
        }
        _dispatcherTask = DispatchMetricsAsync(_disposalCts.Token);
    }

    private async Task DispatchMetricsAsync(CancellationToken cancellationToken)
    {
        await foreach (var captured in _channel.Reader.ReadAllAsync(cancellationToken).ConfigureAwait(false))
        {
            foreach (var sink in sinks)
            {
                try
                {
                    await sink.ReceiveMetricAsync(captured.Metric, captured.Captured).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "error dispatching metric {MetricName} to sink {SinkType}",
                        captured.Metric.Name,
                        captured.Metric.GetType().Name);
                }
            }
        }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        _channel.Writer.Complete();

        if (_dispatcherTask is not null)
        {
            try
            {
                await _dispatcherTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
        }

        _disposalCts.Cancel();
        _disposalCts.Dispose();

        _logger.LogDebug("metrics reporter disposed");
    }
}
