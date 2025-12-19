using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;

using Yarp.ReverseProxy.Configuration;

namespace Pmmux.Extensions.Http;

/// <summary>
/// HTTP backend that proxies requests to an upstream server using YARP.
/// </summary>
/// <param name="backendSpec">The backend specification.</param>
/// <param name="loggerFactory">The logger factory.</param>
/// <param name="priorityTier">The priority tier of the backend.</param>
public sealed class HttpProxyBackend(
    BackendSpec backendSpec,
    ILoggerFactory loggerFactory,
    PriorityTier priorityTier = PriorityTier.Normal)
    : HttpBackendBase(backendSpec, loggerFactory, priorityTier), IHealthCheckBackend
{

    /// <summary>
    /// Protocol factory for <see cref="HttpProxyBackend"/>.
    /// </summary>
    /// <param name="loggerFactory">The logger factory.</param>
    public sealed class Protocol(ILoggerFactory loggerFactory) : IBackendProtocol
    {
        /// <inheritdoc />
        public string Name => "http-proxy";

        /// <inheritdoc />
        public Task<IBackend> CreateBackendAsync(BackendSpec spec, CancellationToken cancellationToken = default)
        {
            var priority = PriorityTier.Normal;

            if (spec.Parameters.TryGetValue("priority", out var priorityString))
            {
                if (!Enum.TryParse(priorityString, ignoreCase: true, out priority))
                {
                    var values = string.Join(',', Enum.GetValues<PriorityTier>()).ToLowerInvariant();
                    throw new InvalidOperationException($"invalid priority {priorityString}, must be one of {values}");
                }
            }

            return Task.FromResult<IBackend>(new HttpProxyBackend(spec, loggerFactory, priority));
        }
    }

    /// <inheritdoc />
    protected override WebApplication Build(WebApplicationBuilder builder)
    {
        if (!Backend.Spec.Parameters.TryGetValue("proxy.address", out var addressString))
        {
            throw new ArgumentException("address is required");
        }
        if (!Uri.TryCreate(addressString, UriKind.RelativeOrAbsolute, out var address) || !address.IsAbsoluteUri)
        {
            throw new ArgumentException("invalid address");
        }
        var timeout = Backend.Spec.Parameters.TryGetValue("proxy.timeout", out var timeoutString) switch
        {
            true when int.TryParse(timeoutString, out var ms) => TimeSpan.FromMilliseconds(ms),
            true => throw new ArgumentException("invalid timeout"),
            _ => default(TimeSpan?)
        };
        var activityTimeout = Backend.Spec.Parameters.TryGetValue(
            "proxy.activity-timeout",
            out var activityTimeoutString) switch
        {
            true when int.TryParse(activityTimeoutString, out var ms) => TimeSpan.FromMilliseconds(ms),
            true => throw new ArgumentException("invalid activity timeout"),
            _ => default(TimeSpan?)
        };

        var transforms = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var (key, value) in Backend.Spec.Parameters)
        {
            if (key.StartsWith("proxy.transform[") && key.EndsWith(']'))
            {
                var transformName = key[16..^1];
                if (transformName.Split('.') is not [var transformType, var transformParameter])
                {
                    transformType = transformName;
                    transformParameter = transformName;
                }
                transforms.TryAdd(transformType, []);
                transforms[transformType][transformParameter] = value;
            }
        }

        builder.Logging.AddFilter("Yarp.ReverseProxy", LogLevel.Error);

        builder.Services
            .AddRequestTimeouts()
            .AddReverseProxy()
            .LoadFromMemory(
                routes: [new()
                    {
                        RouteId = Backend.Spec.Name,
                        ClusterId = Backend.Spec.Name,
                        Match = new()
                        {
                            Path = "{**catch-all}"
                        },
                        Timeout = timeout,
                        Transforms = transforms.Values.ToList()
                    }
                ],
                clusters: [new ()
                {
                    ClusterId = Backend.Spec.Name,
                    Destinations = new Dictionary<string,DestinationConfig>()
                    {
                        ["destination"] = new DestinationConfig()
                        {
                            Address = address.ToString()
                        }
                    },
                    HttpRequest = new()
                    {
                        ActivityTimeout = activityTimeout
                    }
                }
            ]);

        var app = builder.Build();

        app.UseRequestTimeouts();
        app.MapReverseProxy();

        return app;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> HealthCheckAsync(
        Mono.Nat.Protocol _,
        IReadOnlyDictionary<string, string> parameters,
        CancellationToken cancellationToken = default)
    {
        if (!Backend.Spec.Parameters.TryGetValue("proxy.address", out var a))
        {
            throw new ArgumentException("address is required");
        }
        if (!Uri.TryCreate(a, UriKind.RelativeOrAbsolute, out var address) || !address.IsAbsoluteUri)
        {
            throw new ArgumentException("invalid address");
        }
        if (!parameters.TryGetValue("path", out var pathString))
        {
            pathString = "";
        }
        if (!Uri.TryCreate(address, pathString, out var uri))
        {
            return new HealthCheckResult(false, "invalid path parameter");
        }
        if (!parameters.TryGetValue("method", out var method))
        {
            method = "GET";
        }
        if (!parameters.TryGetValue("status", out var statusString))
        {
            statusString = "200-299";
        }
        var statusRanges =
            from status in statusString.Split(',', ';')
            select status.Split('-') switch
            {
                [var s, var e] when int.TryParse(s, out var start) && int.TryParse(e, out var end) => (start, end),
                [var s] when int.TryParse(s, out var start) => (start, end: start),
                [..] => throw new ArgumentException($"invalid status {status}")
            };

        try
        {

            using var httpClient = new HttpClient();

            var response = await httpClient.SendAsync(new(new(method), uri), cancellationToken).ConfigureAwait(false);
            var status = (int)response.StatusCode;
            if (!statusRanges.Any(range => status >= range.start && status <= range.end))
            {
                return new HealthCheckResult(false, $"unexpected status code: {status}");
            }
            return new HealthCheckResult(true, null);
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(false, $"error sending health check: {ex.Message}");
        }
    }
}

