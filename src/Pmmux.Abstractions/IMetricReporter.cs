using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Reports metrics from port multiplexer components.
/// </summary>
public interface IMetricReporter
{
    private sealed class DurationMeasurement(
        IMetricReporter reporter,
        string name,
        string category,
        Dictionary<string, string?> metadata) : IDisposable
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public void Dispose()
        {
            _stopwatch.Stop();
            reporter.ReportDuration(name, category, _stopwatch.Elapsed, metadata);
        }
    }

    /// <summary>
    /// Report a typed metric.
    /// </summary>
    /// <typeparam name="T">Type of the metric value.</typeparam>
    /// <param name="metric">The metric to report.</param>
    void ReportMetric<T>(Metric<T> metric);

    /// <summary>
    /// Report counter metric with a specified value.
    /// </summary>
    /// <param name="name">The metric name.</param>
    /// <param name="category">The metric category.</param>
    /// <param name="value">The counter value.</param>
    /// <param name="metadata">Additional dimensions for the metric.</param>
    void ReportCounter(string name, string category, double value, Dictionary<string, string?> metadata)
    {
        ReportMetric(new CounterMetric(name, category, metadata, value));
    }

    /// <summary>
    /// Report a single count of a metric.
    /// </summary>
    /// <param name="name">The event name.</param>
    /// <param name="category">The event category for grouping.</param>
    /// <param name="metadata">Additional dimensions for the event.</param>
    void ReportEvent(string name, string category, Dictionary<string, string?> metadata)
    {
        ReportMetric(new CounterMetric(name, category, metadata, 1));
    }

    /// <summary>
    /// Report gauge metric with a specified current value.
    /// </summary>
    /// <param name="name">The metric name.</param>
    /// <param name="category">The metric category for grouping.</param>
    /// <param name="value">The current gauge value.</param>
    /// <param name="metadata">Additional dimensions for the metric.</param>
    void ReportGauge(string name, string category, double value, Dictionary<string, string?> metadata)
    {
        ReportMetric(new GaugeMetric(name, category, metadata, value));
    }

    /// <summary>
    /// Report duration metric with a specified time span.
    /// </summary>
    /// <param name="name">The metric name.</param>
    /// <param name="category">The metric category for grouping.</param>
    /// <param name="value">The duration to report.</param>
    /// <param name="metadata">Additional dimensions for the metric.</param>
    void ReportDuration(string name, string category, TimeSpan value, Dictionary<string, string?> metadata)
    {
        ReportMetric(new DurationMetric(name, category, metadata, value));
    }

    /// <summary>
    /// Create a disposable object that measures and reports duration from creation to disposal.
    /// </summary>
    /// <param name="name">The metric name.</param>
    /// <param name="category">The metric category for grouping.</param>
    /// <param name="metadata">Additional dimensions for the metric.</param>
    /// <returns>
    /// <see cref="IDisposable"/> that starts timing immediately and reports the duration when disposed.
    /// </returns>
    /// <example>
    /// <code>
    /// using (metricReporter.MeasureDuration("operation_duration", "backend"))
    /// {
    ///     // Code block to measure
    /// }
    /// </code>
    /// </example>
    IDisposable MeasureDuration(string name, string category, Dictionary<string, string?> metadata)
    {
        return new DurationMeasurement(this, name, category, metadata);
    }

    /// <summary>
    /// Execute an operation and measure its duration.
    /// </summary>
    /// <typeparam name="T">Return type of the operation.</typeparam>
    /// <param name="name">The metric name.</param>
    /// <param name="category">The metric category for grouping.</param>
    /// <param name="metadata">Additional dimensions for the metric.</param>
    /// <param name="operation">The operation to measure.</param>
    /// <returns>Result of the operation.</returns>
    T MeasureDuration<T>(string name, string category, Dictionary<string, string?> metadata, Func<T> operation)
    {
        using (MeasureDuration(name, category, metadata))
        {
            return operation();
        }
    }

    /// <summary>
    /// Execute an asynchronous operation and measure its duration.
    /// </summary>
    /// <typeparam name="T">Return type of the operation.</typeparam>
    /// <param name="name">The metric name.</param>
    /// <param name="category">The metric category for grouping.</param>
    /// <param name="metadata">Additional dimensions for the metric.</param>
    /// <param name="operation">The asynchronous operation to measure.</param>
    /// <returns>A task representing the operation.</returns>
    async Task<T> MeasureDurationAsync<T>(
        string name,
        string category,
        Dictionary<string, string?> metadata,
        Func<Task<T>> operation)
    {
        using (MeasureDuration(name, category, metadata))
        {
            return await operation().ConfigureAwait(false);
        }
    }
}
