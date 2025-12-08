using System;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Receives metrics from the <see cref="IMetricReporter"/>.
/// </summary>
/// <remarks>
/// Metric sinks process metrics emitted by the multiplexer and extensions.
/// Common implementations include logging sinks, time-series database exporters,
/// and monitoring system integrations. Multiple sinks can be registered.
/// </remarks>
public interface IMetricSink
{
    /// <summary>
    /// Receive and process a metric asynchronously.
    /// </summary>
    /// <param name="metric">
    /// The metric to process. May be a <see cref="CounterMetric"/>, <see cref="GaugeMetric"/>,
    /// <see cref="DurationMetric"/>, or other metric type.
    /// </param>
    /// <param name="captured">The timestamp when the metric was captured.</param>
    /// <returns>A task representing the asynchronous processing operation.</returns>
    Task ReceiveMetricAsync(Metric metric, DateTimeOffset captured);
}
