using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;

namespace Pmmux.App;

internal sealed class LoggerMetricSink(ILoggerFactory loggerFactory) : IMetricSink
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("metrics");

    public Task ReceiveMetricAsync(Metric metric, DateTimeOffset captured)
    {
        _logger.LogTrace("[{MetricName}:{MetricValue}] captured at {Captured:u} metadata: ({MetricMetadata})",
            metric.Name,
            metric.StringValue(),
            captured,
            string.Join(",", metric.Metadata.Select(x => $"{x.Key}={x.Value}")));

        return Task.CompletedTask;
    }
}
