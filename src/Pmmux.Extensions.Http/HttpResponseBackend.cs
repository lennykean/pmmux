
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Http;

internal class HttpResponseBackend(
    BackendSpec backendSpec,
    ILoggerFactory loggerFactory,
    PriorityTier priorityTier = PriorityTier.Normal) : HttpBackend(backendSpec, loggerFactory, priorityTier)
{
    private int _responseStatus = 200;
    private string? _responseBody = null;

    private readonly Dictionary<string, string[]> _responseHeaders = [];

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await base.InitializeAsync(cancellationToken);

        if (Backend.Spec.Parameters.TryGetValue("response.status", out var s) && int.TryParse(s, out var status))
        {
            _responseStatus = status;
        }
        if (Backend.Spec.Parameters.TryGetValue("response.body", out var body))
        {
            _responseBody = body;
        }
        foreach (var (name, value) in Backend.Spec.Parameters)
        {
            if (name.StartsWith("response.header[") && name.EndsWith(']'))
            {
                var headerName = name[16..^1];
                _responseHeaders[headerName] = value.Split(';');
            }
        }
    }

    internal class Protocol(ILoggerFactory loggerFactory) : IBackendProtocol
    {
        public string Name => "http-response";

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

            return Task.FromResult<IBackend>(new HttpResponseBackend(spec, loggerFactory, priority));
        }
    }

    protected override WebApplication Build(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        app.MapFallback(async context =>
        {
            context.Response.StatusCode = _responseStatus;

            foreach (var (name, values) in _responseHeaders)
            {
                foreach (var value in values)
                {
                    context.Response.Headers.Append(name, Replace(value, context.Request));
                }
            }
            if (!string.IsNullOrEmpty(_responseBody))
            {
                await context.Response.WriteAsync(Replace(_responseBody, context.Request)).ConfigureAwait(false);
            }
        });

        return app;
    }

    private static string Replace(string name, HttpRequest request)
    {
        return Regex.Replace(name, "{(.+?)}", match => match.Groups is not [_, var group]
            ? match.Value
            : group.Value.ToLowerInvariant() switch
            {
                "scheme" => request.Scheme,
                "host" => request.Host.Host,
                "port" => request.Host.Port?.ToString() ?? string.Empty,
                "method" => request.Method,
                "path" => request.Path,
                "query" => request.QueryString.ToString(),
                _ => match.Value
            });
    }
}
