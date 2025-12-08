using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Otlp;

internal sealed class OtlpMetricSink(IMeterFactory meterFactory) : IMetricSink, IDisposable
{
    private readonly ConcurrentDictionary<string, Meter> _meters = [];
    private readonly ConcurrentDictionary<string, Counter<double>> _counters = [];
    private readonly ConcurrentDictionary<string, Histogram<double>> _histograms = [];
    private readonly ConcurrentDictionary<string, Gauge<double>> _gauges = [];

    public Task ReceiveMetricAsync(Metric metric, DateTimeOffset captured)
    {
        var tags = ToTags(metric.Metadata).ToArray();
        var meter = _meters.GetOrAdd(metric.Category, _ => meterFactory.Create(metric.Category));

        switch (metric)
        {
            case CounterMetric { Value: var value }:
                _counters.GetOrAdd(
                    $"{metric.Category}:{metric.Name}",
                    _ => meter.CreateCounter<double>(metric.Name)).Add(value, tags);
                break;
            case GaugeMetric { Value: var value }:
                _gauges.GetOrAdd(
                    $"{metric.Category}:{metric.Name}",
                    _ => meter.CreateGauge<double>(metric.Name)).Record(value, tags);
                break;
            case DurationMetric { Value: var value }:
                _histograms.GetOrAdd(
                    $"{metric.Category}:{metric.Name}",
                    _ => meter.CreateHistogram<double>(metric.Name, unit: "ms")).Record(value.TotalMilliseconds, tags);
                break;
            default:
                throw new ArgumentException($"Unsupported metric type: {metric.GetType().Name}");
        }

        return Task.CompletedTask;
    }


    private static IEnumerable<KeyValuePair<string, object?>> ToTags(Dictionary<string, string?> metadata)
    {
        foreach (var (key, value) in metadata)
        {
            if (value is null)
            {
                continue;
            }
            yield return new(key, value);
        }
    }

    public void Dispose()
    {
        foreach (var meter in _meters.Values)
        {
            meter.Dispose();
        }
        _meters.Clear();
        _counters.Clear();
        _histograms.Clear();
        _gauges.Clear();
    }
}
