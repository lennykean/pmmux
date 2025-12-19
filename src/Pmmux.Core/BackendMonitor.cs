using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Mono.Nat;

using Pmmux.Abstractions;
using Pmmux.Core.Configuration;

namespace Pmmux.Core;

/// <summary>
/// Default implementation of <see cref="IBackendMonitor"/>.
/// </summary>
/// <param name="routerConfig">The router configuration.</param>
/// <param name="loggerFactory">The logger factory.</param>
/// <param name="metricReporter">The metric reporter service.</param>
/// <param name="eventSender">The event sender service.</param>
public sealed class BackendMonitor(
    RouterConfig routerConfig,
    ILoggerFactory loggerFactory,
    IMetricReporter metricReporter,
    IEventSender eventSender) : IBackendMonitor
{
    private readonly SemaphoreSlim _notifyChanged = new(initialCount: 0);
    private readonly CancellationTokenSource _disposalCts = new();
    private readonly ILogger _logger = loggerFactory.CreateLogger("backend-monitor");
    private readonly ConcurrentDictionary<HealthCheckSpec, HealthCheckSpec> _healthChecks = new(
        routerConfig.HealthChecks.ToDictionary(spec => spec, spec => spec));

    /// <inheritdoc />
    public async IAsyncEnumerable<BackendStatusInfo> MonitorAsync(
        IHealthCheckBackend backend,
        Protocol networkProtocol,
        CancellationToken cancellationToken,
        [EnumeratorCancellation] CancellationToken enumeratorCancellationToken)
    {
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _disposalCts.Token);

        long failureCount = 0;
        long successCount = 0;
        HealthCheckResult? lastResult = null;
        HealthCheckSpec? lastSpec = null;

        var lastStatus = BackendStatus.Unknown;


        while (!linkedCts.Token.IsCancellationRequested && !enumeratorCancellationToken.IsCancellationRequested)
        {
            HealthCheckSpec? spec = null;

            var changeTask = _notifyChanged.WaitAsync(linkedCts.Token);
            try
            {
                spec = _healthChecks.Values.FirstOrDefault(s =>
                    s.BackendName == backend.Backend.Spec.Name &&
                    s.ProtocolName == backend.Backend.Spec.ProtocolName);

                spec ??= _healthChecks.Values.FirstOrDefault(s =>
                    s.ProtocolName == backend.Backend.Spec.ProtocolName &&
                    s.BackendName is null);

                spec ??= _healthChecks.Values.FirstOrDefault(s =>
                    s.BackendName is null &&
                    s.ProtocolName is null);

                if (spec == null)
                {
                    failureCount = 0;
                    successCount = 0;
                    lastResult = null;
                    lastStatus = BackendStatus.Unknown;

                    await changeTask.ConfigureAwait(false);

                    continue;
                }
            }
            finally
            {
                _notifyChanged.Release();
            }

            var baseMetadata = new Dictionary<string, string?>
            {
                ["backend"] = backend.Backend.Spec.Name,
                ["protocol"] = backend.Backend.Spec.ProtocolName,
                ["network_protocol"] = networkProtocol.ToString()
            };

            if (spec != lastSpec)
            {
                _logger.LogDebug(
                    "starting {NetworkProtocol} health checks (initial-delay:{InitialDelay}," +
                    "interval:{Interval},timeout:{Timeout})",
                    networkProtocol,
                    spec.InitialDelay,
                    spec.Interval,
                    spec.Timeout);

                await Task.Delay(spec.InitialDelay, linkedCts.Token).ConfigureAwait(false);

                lastSpec = spec;
            }
            else
            {
                await Task.Delay(spec.Interval, linkedCts.Token).ConfigureAwait(false);
            }

            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(linkedCts.Token);
            timeoutCts.CancelAfter(spec.Timeout);

            HealthCheckResult result;
            using (metricReporter.MeasureDuration("backend.health_check.duration", "multiplexer", baseMetadata))
            {
                try
                {
                    result = await backend.HealthCheckAsync(
                        networkProtocol,
                        spec.Parameters,
                        timeoutCts.Token).WithTimeout(timeoutCts.Token).ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    result = new HealthCheckResult(IsSuccess: false, Reason: ex.Message);
                }
            }

            metricReporter.ReportEvent("backend.health_check.total", "multiplexer", new(baseMetadata)
            {
                ["result"] = result.IsSuccess ? "success" : "failure"
            });

            if (result.IsSuccess)
            {
                successCount++;
                failureCount = 0;
            }
            else
            {
                failureCount++;
                successCount = 0;
            }

            var status = result.IsSuccess switch
            {
                true when lastStatus != BackendStatus.Healthy && successCount >= spec.RecoveryThreshold =>
                    BackendStatus.Healthy,
                false when lastStatus != BackendStatus.Unhealthy && failureCount >= spec.FailureThreshold =>
                    BackendStatus.Unhealthy,
                true when lastResult is { IsSuccess: false } =>
                    BackendStatus.Stabilizing,
                false when lastResult is { IsSuccess: true } =>
                    BackendStatus.Degraded,
                true when lastStatus is BackendStatus.Stabilizing =>
                    BackendStatus.Stabilizing,
                false when lastStatus is BackendStatus.Degraded =>
                    BackendStatus.Degraded,
                true =>
                    BackendStatus.Healthy,
                false =>
                    BackendStatus.Unhealthy,
            };

            _logger.LogTrace(
                "{NetworkProtocol} health check result {HealthCheckResult}:{BackendStatus} {Reason}",
                networkProtocol,
                result.IsSuccess is true ? "success" : "fail",
                status,
                result.Reason is null ? "" : $"({result.Reason})");

            metricReporter.ReportGauge("backend.status", "multiplexer", (int)status, baseMetadata);
            metricReporter.ReportGauge(
                "backend.health_check.consecutive_failures",
                "multiplexer",
                failureCount,
                baseMetadata);
            metricReporter.ReportGauge(
                "backend.health_check.consecutive_successes",
                "multiplexer",
                successCount,
                baseMetadata);

            yield return new(backend.Backend, status, result.Reason, DateTime.UtcNow, failureCount, successCount);

            lastStatus = status;
            lastResult = result;
        }

        _logger.LogDebug("stopped health checks");
    }

    /// <inheritdoc />
    public IEnumerable<HealthCheckSpec> GetHealthChecks()
    {
        return _healthChecks.Values;
    }

    /// <inheritdoc />
    public bool TryAddHealthCheck(HealthCheckSpec healthCheckSpec)
    {
        _logger.LogTrace(
            "adding health check for {ProtocolName}:{BackendName}",
            healthCheckSpec.ProtocolName ?? "*",
            healthCheckSpec.BackendName ?? "*");

        var added = _healthChecks.TryAdd(healthCheckSpec, healthCheckSpec);
        if (added)
        {
            _logger.LogDebug(
                "added health check for {ProtocolName}:{BackendName}",
                healthCheckSpec.ProtocolName ?? "*",
                healthCheckSpec.BackendName ?? "*");
            eventSender.RaiseHealthCheckAdded(this, healthCheckSpec);
        }

        _notifyChanged.Release();

        return added;
    }

    /// <inheritdoc />
    public bool TryRemoveHealthCheck(HealthCheckSpec healthCheckSpec)
    {
        _logger.LogTrace(
            "removing health check for {ProtocolName}:{BackendName}",
            healthCheckSpec.ProtocolName ?? "*",
            healthCheckSpec.BackendName ?? "*");

        var removed = _healthChecks.TryRemove(healthCheckSpec, out _);
        if (removed)
        {
            _logger.LogDebug(
                "removed health check for {ProtocolName}:{BackendName}",
                healthCheckSpec.ProtocolName ?? "*",
                healthCheckSpec.BackendName ?? "*");
            eventSender.RaiseHealthCheckRemoved(this, healthCheckSpec);
        }

        return removed;
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        _disposalCts.Cancel();
        _disposalCts.Dispose();

        _logger.LogDebug("backend monitor disposed");

        return default;
    }
}
