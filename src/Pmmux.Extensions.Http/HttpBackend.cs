using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;
using Pmmux.Core;

namespace Pmmux.Extensions.Http;

/// <summary>
/// Base class for HTTP-based backends.
/// </summary>
/// <param name="backendSpec">The backend specification.</param>
/// <param name="loggerFactory">The logger factory.</param>
/// <param name="priority">The default priority tier.</param>
public abstract class HttpBackendBase(
    BackendSpec backendSpec,
    ILoggerFactory loggerFactory,
    PriorityTier priority) : BackendBase(backendSpec, [], priority), IConnectionOrientedBackend
{
    private enum State
    {
        Initial,
        Starting,
        Serving,
        Dispose
    }

    private class Connection(PipeReader ingress, PipeWriter egress) : IConnection
    {
        public IReadOnlyDictionary<string, string> Properties => new Dictionary<string, string>();

        public PipeReader GetReader() => ingress;
        public PipeWriter GetWriter() => egress;

        public async Task CloseAsync()
        {
            await Task.WhenAll(ingress.CompleteAsync().AsTask(), egress.CompleteAsync().AsTask()).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await CloseAsync().ConfigureAwait(false);
        }
    }

    private readonly StateManager<State> _state = new(State.Initial);
    private readonly Channel<ConnectionContext?> _channel = Channel.CreateUnbounded<ConnectionContext?>();
    private Matcher<Regex>[] _versionMatchers = [];
    private Matcher<Regex>[] _methodMatchers = [];
    private Matcher<Regex>[] _pathMatchers = [];
    private Matcher<Regex>[] _hostMatchers = [];
    private IReadOnlyDictionary<string, IEnumerable<Matcher<Regex>>> _headerMatchers
        = new Dictionary<string, IEnumerable<Matcher<Regex>>>();

    private IHost? _app;

    /// <summary>
    /// Logger for the backend.
    /// </summary>
    protected ILogger Logger => loggerFactory.CreateLogger($"{Backend.Spec.ProtocolName}-backend");

    /// <summary>
    /// Builds the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="builder">The builder for the web application.</param>
    /// <returns>A configured <see cref="WebApplication"/>.</returns>
    protected abstract WebApplication Build(WebApplicationBuilder builder);

    /// <inheritdoc />
    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_state.Is(State.Dispose), nameof(HttpBackendBase));

        if (!_state.TryTransition(from: State.Initial, to: State.Starting))
        {
            throw new InvalidOperationException();
        }

        var matchers = Backend.Spec.GetMatchers();

        _versionMatchers = matchers.TryGetValue("protocol-version", out var versionMatcher)
            ? [.. versionMatcher.AsMultiValue().AsRegex()]
            : [];

        _methodMatchers = matchers.TryGetValue("method", out var methodMatcher)
            ? [.. methodMatcher.AsMultiValue().AsRegex()]
            : [];

        _pathMatchers = matchers.TryGetValue("path", out var pathMatcher)
            ? [.. pathMatcher.AsMultiValue().AsRegex()]
            : [];

        _hostMatchers = matchers.TryGetValue("host", out var hostMatcher)
            ? [.. hostMatcher.AsMultiValue().AsRegex()]
            : [];

        _headerMatchers = matchers
            .AsMultiValueIndexed("header")
            .ToDictionary(kv => kv.Key, kv => kv.Value.AsRegex(), StringComparer.OrdinalIgnoreCase);

        try
        {
            var builder = WebApplication.CreateBuilder();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false;
            });

            builder.Logging
                .SetMinimumLevel(LogLevel.Trace)
                .AddFilter("Microsoft", LogLevel.Warning)
                .ClearProviders()
                .AddProvider(new ProxyLoggerProvider(loggerFactory))
                .Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.None);

            builder.Services.AddSingleton<IConnectionListenerFactory>(_ =>
            {
                return new PmmuxConnectionListener.Factory(_channel);
            });

            var app = Build(builder);

            app.Use(async (context, next) =>
            {
                Task task;
                using (ExecutionContext.SuppressFlow())
                {
                    task = Task.Run(async () =>
                    {
                        Activity.Current = null;

                        using (Logger.BeginScope("{RemoteEndpoint}", new IPEndPoint(
                            context.Connection.RemoteIpAddress ?? IPAddress.Any,
                            context.Connection.RemotePort)))
                        using (Logger.BeginScope("{BackendName}:{BackendProtocol}",
                            Backend.Spec.Name,
                            Backend.Spec.ProtocolName))
                        {
                            Logger.LogTrace(
                                "request received {Method} {PathAndQuery} - {{{RequestHeaders}}}",
                                context.Request.Method,
                                context.Request.Path + context.Request.QueryString,
                                string.Join(",", context.Request.Headers
                                    .Select(kv => $"\"{kv.Key}\":\"{string.Join(";", kv.Value.ToArray())}\"")));

                            await next().ConfigureAwait(false);

                            Logger.LogTrace(
                                "response sent {Method} {PathAndQuery} - {Status} - {{{ResponseHeaders}}}",
                                context.Request.Method,
                                context.Request.Path + context.Request.QueryString,
                                context.Response.StatusCode,
                                string.Join(",", context.Response.Headers
                                    .Select(kv => $"\"{kv.Key}\":\"{string.Join(";", kv.Value.ToArray())}\"")));
                        }
                    });
                }
                await task.ConfigureAwait(false);
            });

            await app.StartAsync(cancellationToken).ConfigureAwait(false);

            _app = app;

            Logger.LogInformation("http server started");

            _state.TryTransition(from: State.Starting, to: State.Serving);
        }
        catch
        {
            _state.TryTransition(from: State.Starting, to: State.Initial);

            throw;
        }
    }

    /// <inheritdoc />
    public virtual async Task<bool> CanHandleConnectionAsync(
        IClientConnectionPreview clientPreview,
        CancellationToken cancellationToken = default)
    {
        if (!MatchesProperties(clientPreview.Properties))
        {
            return false;
        }

        var preview = clientPreview.Ingress;
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await preview.ReadAsync(cancellationToken).ConfigureAwait(false);
            if (result.IsCanceled)
            {
                return false;
            }

            switch (StreamParser.TryParseHttp(result.Buffer, out var method, out var path, out var version))
            {
                case null:
                    if (result.IsCompleted)
                    {
                        return false;
                    }
                    preview.AdvanceTo(result.Buffer.Start, result.Buffer.End);
                    continue;
                case false:
                    return false;
            }

            if (!_versionMatchers.HasMatch(r => r.IsMatch(version!)))
            {
                return false;
            }
            if (!_methodMatchers.HasMatch(r => r.IsMatch(method!)))
            {
                return false;
            }
            if (!_pathMatchers.HasMatch(r => r.IsMatch(path!)))
            {
                return false;
            }

            if (_hostMatchers.Length > 0)
            {
                var hostExists = clientPreview.Properties.TryGetValue("tls.sni", out var host);
                if (!hostExists)
                {
                    switch (StreamParser.TryExtractHttpHeaders(result.Buffer, ["host"], out var values))
                    {
                        case null:
                            if (result.IsCompleted)
                            {
                                return false;
                            }
                            preview.AdvanceTo(result.Buffer.Start, result.Buffer.End);
                            continue;
                    }
                    hostExists = values?.TryGetValue("host", out host) is true;
                }
                if (!_hostMatchers.HasMatch(r => hostExists && r.IsMatch(host!)))
                {
                    return false;
                }
            }
            if (_headerMatchers.Count > 0)
            {
                var inspectHeaders = _headerMatchers.Keys.ToArray();

                switch (StreamParser.TryExtractHttpHeaders(
                    result.Buffer,
                    inspectHeaders,
                    out var headers,
                    includePartial: true))
                {
                    case null:
                        if (result.IsCompleted)
                        {
                            return false;
                        }
                        preview.AdvanceTo(result.Buffer.Start, result.Buffer.End);
                        continue;
                }
                foreach (var (header, matchers) in _headerMatchers)
                {
                    var headerValue = default(string);
                    var headerExists = headers?.TryGetValue(header, out headerValue) is true;
                    if (!matchers.HasMatch(r => headerExists && r.IsMatch(headerValue!)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public async Task<IConnection> CreateBackendConnectionAsync(
        IClientConnection client,
        CancellationToken cancellationToken = default)
    {
        var ingressPipe = new Pipe();
        var egressPipe = new Pipe();

        var connectionContext = new PmmuxConnectionContext(
            client: client.Client,
            input: ingressPipe.Reader,
            output: egressPipe.Writer);

        await _channel.Writer.WriteAsync(connectionContext, cancellationToken).ConfigureAwait(false);

        return new Connection(egressPipe.Reader, ingressPipe.Writer);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (!_state.TryTransition(to: State.Dispose))
        {
            return;
        }

        if (_app is not null)
        {
            try
            {
                await _app.StopAsync().ConfigureAwait(false);
            }
            finally
            {
                _app.Dispose();
            }
        }
    }
}
