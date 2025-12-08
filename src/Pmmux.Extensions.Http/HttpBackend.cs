using System;
using System.Collections.Generic;
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
public abstract class HttpBackend(
    BackendSpec backendSpec,
    ILoggerFactory loggerFactory,
    PriorityTier priority) : IConnectionOrientedBackend
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

    private record Matcher(bool Negate, Regex Expression);

    private readonly StateManager<State> _state = new(State.Initial);
    private readonly Channel<ConnectionContext?> _channel = Channel.CreateUnbounded<ConnectionContext?>();
    private readonly List<Matcher> _versionMatchers = [];
    private readonly List<Matcher> _methodMatchers = [];
    private readonly List<Matcher> _pathMatchers = [];
    private readonly List<Matcher> _hostMatchers = [];
    private readonly Dictionary<string, List<Matcher>> _headerMatchers = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<Matcher>> _propertyMatchers = [];

    private IHost? _app;

    /// <summary>
    /// Logger for the backend.
    /// </summary>
    protected ILogger Logger { get; } = loggerFactory.CreateLogger($"{backendSpec.ProtocolName}-backend");

    /// <inheritdoc />
    public BackendInfo Backend => new(backendSpec, new Dictionary<string, string>(), priority);

    /// <summary>
    /// Builds the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="builder">The builder for the web application.</param>
    /// <returns>A configured <see cref="WebApplication"/>.</returns>
    protected abstract WebApplication Build(WebApplicationBuilder builder);

    /// <inheritdoc />
    public virtual async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_state.Is(State.Dispose), nameof(HttpBackend));

        if (!_state.TryTransition(from: State.Initial, to: State.Starting))
        {
            throw new InvalidOperationException();
        }

        if (backendSpec.Parameters.TryGetValue("protocol-version", out var versionMatchString))
        {
            versionMatchString = $"^(?:{versionMatchString.TrimStart('^').TrimEnd('$')})$";
            _versionMatchers.Add(new(false, new(versionMatchString, RegexOptions.Compiled)));
        }
        else if (backendSpec.Parameters.TryGetValue("protocol-version!", out var negVersionMatchString))
        {
            negVersionMatchString = $"^(?:{negVersionMatchString.TrimStart('^').TrimEnd('$')})$";
            _versionMatchers.Add(new(true, new(negVersionMatchString, RegexOptions.Compiled)));
        }
        if (backendSpec.Parameters.TryGetValue("method", out var methodMatchString))
        {
            methodMatchString = $"^(?:{methodMatchString.TrimStart('^').TrimEnd('$')})$";
            _methodMatchers.Add(new(false, new(methodMatchString, RegexOptions.Compiled | RegexOptions.IgnoreCase)));
        }
        else if (backendSpec.Parameters.TryGetValue("method!", out var negMethodMatchString))
        {
            negMethodMatchString = $"^(?:{negMethodMatchString.TrimStart('^').TrimEnd('$')})$";
            _methodMatchers.Add(new(true, new(negMethodMatchString, RegexOptions.Compiled | RegexOptions.IgnoreCase)));
        }
        if (backendSpec.Parameters.TryGetValue("path", out var pathMatchString))
        {
            pathMatchString = $"^(?:{pathMatchString.TrimStart('^').TrimEnd('$')})$";
            _pathMatchers.Add(new(false, new(pathMatchString, RegexOptions.Compiled)));
        }
        if (backendSpec.Parameters.TryGetValue("path!", out var negPathMatchString))
        {
            negPathMatchString = $"^(?:{negPathMatchString.TrimStart('^').TrimEnd('$')})$";
            _pathMatchers.Add(new(true, new(negPathMatchString, RegexOptions.Compiled)));
        }
        if (backendSpec.Parameters.TryGetValue("host", out var hostMatchString))
        {
            hostMatchString = $"^(?:{hostMatchString.TrimStart('^').TrimEnd('$')})$";
            _hostMatchers.Add(new(false, new(hostMatchString, RegexOptions.Compiled)));
        }
        if (backendSpec.Parameters.TryGetValue("host!", out var negHostMatchString))
        {
            negHostMatchString = $"^(?:{negHostMatchString.TrimStart('^').TrimEnd('$')})$";
            _hostMatchers.Add(new(true, new(negHostMatchString, RegexOptions.Compiled)));
        }
        foreach (var (name, values) in Backend.Spec.Parameters)
        {
            bool negate;
            if (name.EndsWith(']'))
            {
                negate = false;
            }
            else if (name.EndsWith("]!"))
            {
                negate = true;
            }
            else
            {
                continue;
            }
            if (name.StartsWith("property["))
            {
                var property = name[9..^(negate ? 2 : 1)];

                if (!_propertyMatchers.TryGetValue(property, out var matchers))
                {
                    matchers = [];
                    _propertyMatchers[property] = matchers;
                }
                matchers.AddRange(values
                    .Split(';')
                    .Select(value => new Matcher(negate, new(value, RegexOptions.Compiled))));
            }
            if (name.StartsWith("header["))
            {
                var header = name[7..^(negate ? 2 : 1)];

                if (!_headerMatchers.TryGetValue(header, out var matchers))
                {
                    matchers = [];
                    _headerMatchers[header] = matchers;
                }
                matchers.AddRange(values
                    .Split(';')
                    .Select(value => new Matcher(negate, new(value, RegexOptions.Compiled))));
            }
        }

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
                        using (Logger.BeginScope("{RemoteEndpoint}", new IPEndPoint(
                            context.Connection.RemoteIpAddress ?? IPAddress.Any,
                            context.Connection.RemotePort)))
                        using (Logger.BeginScope("{BackendName}:{BackendProtocol}",
                            backendSpec.Name,
                            backendSpec.ProtocolName))
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
        foreach (var (property, matchers) in _propertyMatchers)
        {
            if (matchers is { Count: 0 })
            {
                continue;
            }
            var propertyExists = clientPreview.Properties.TryGetValue(property, out var value);

            foreach (var matcher in matchers)
            {
                if ((propertyExists && matcher.Expression.IsMatch(value!)) == matcher.Negate)
                {
                    return false;
                }
            }
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

            if (_versionMatchers.Any(m => m.Expression.IsMatch(version!) == m.Negate))
            {
                return false;
            }
            if (_methodMatchers.Any(m => m.Expression.IsMatch(method!) == m.Negate))
            {
                return false;
            }
            if (_pathMatchers.Any(m => m.Expression.IsMatch(path!) == m.Negate))
            {
                return false;
            }

            if (_hostMatchers.Any())
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
                foreach (var matcher in _hostMatchers)
                {
                    if ((hostExists && matcher.Expression.IsMatch(host!)) == matcher.Negate)
                    {
                        return false;
                    }
                }
            }
            if (_headerMatchers.Any())
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

                    foreach (var matcher in matchers)
                    {
                        if ((headerExists && matcher.Expression.IsMatch(headerValue!)) == matcher.Negate)
                        {
                            return false;
                        }
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

    async ValueTask IAsyncDisposable.DisposeAsync()
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
