using System;
using System.Collections.Generic;

namespace Pmmux.Abstractions;

/// <summary>
/// Base class for all metric types.
/// </summary>
/// <param name="Name">The metric name identifying what is being measured.</param>
/// <param name="Category">The category for grouping related metrics.</param>
/// <param name="Metadata">Additional dimensions and context for the metric.</param>
public abstract record Metric(string Name, string Category, Dictionary<string, string?> Metadata)
{
    /// <summary>
    /// Get string representation of the metric's value.
    /// </summary>
    /// <returns>Metric value as a string.</returns>
    public abstract string StringValue();
};

/// <summary>
/// Base class for metrics with a specific value type.
/// </summary>
/// <typeparam name="T">Type of the metric value.</typeparam>
/// <param name="Name">The metric name identifying what is being measured.</param>
/// <param name="Category">The category for grouping related metrics.</param>
/// <param name="Metadata">Additional dimensions and context for the metric.</param>
/// <param name="Value">The measured value.</param>
public abstract record Metric<T>(
    string Name,
    string Category,
    Dictionary<string, string?> Metadata,
    T Value) : Metric(Name, Category, Metadata)
{
    /// <inheritdoc/>
    public override string StringValue() => Value?.ToString() ?? "<null>";
}

/// <summary>
/// Counter metric that tracks cumulative values.
/// </summary>
/// <param name="Name">The metric name.</param>
/// <param name="Category">The metric category.</param>
/// <param name="Metadata">Additional dimensions and context.</param>
/// <param name="Value">The counter value.</param>
public record CounterMetric(
    string Name,
    string Category,
    Dictionary<string, string?> Metadata,
    double Value) : Metric<double>(Name, Category, Metadata, Value);

/// <summary>
/// Gauge metric that tracks instantaneous values.
/// </summary>
/// <param name="Name">The metric name.</param>
/// <param name="Category">The metric category.</param>
/// <param name="Metadata">Additional dimensions and context.</param>
/// <param name="Value">The current gauge value.</param>
public record GaugeMetric(
    string Name,
    string Category,
    Dictionary<string, string?> Metadata,
    double Value) : Metric<double>(Name, Category, Metadata, Value);

/// <summary>
/// Duration metric that tracks time-based measurements.
/// </summary>
/// <param name="Name">The metric name.</param>
/// <param name="Category">The metric category.</param>
/// <param name="Metadata">Additional dimensions and context.</param>
/// <param name="Value">The duration measured.</param>
public record DurationMetric(
    string Name,
    string Category,
    Dictionary<string, string?> Metadata,
    TimeSpan Value) : Metric<TimeSpan>(Name, Category, Metadata, Value);
