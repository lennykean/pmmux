# Extensibility Guide

pmmux uses a plugin architecture for extending functionality without modifying the core. Extensions can add new backend protocols, routing strategies, connection negotiators, metric sinks, and more.

## Loading Extensions

When pmmux starts, extension DLLs are loaded based on the `--extensions` CLI option or the `extensions` configuration setting. Extensions must have a **public** class implementing [`IExtension`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IExtension/index.md), which pmmux discovers and invokes during startup.

Extensions are loaded early, so their CLI options are available for parsing. Registered services are resolved when needed during runtime.

```sh
pmmux -x Pmmux.Extensions.Tls.dll -x Pmmux.Extensions.Http.dll ...
```

```toml
[pmmux]
extensions = ["Pmmux.Extensions.Tls.dll", "Pmmux.Extensions.Http.dll"]
```

## Extension Entry Point

The `IExtension` interface defines two methods for registering functionality with pmmux. [`RegisterServices`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IExtension/RegisterServices(IServiceCollection,HostBuilderContext).md) adds components to the DI container to provide backend protocols, routing strategies, and connection negotiators. [`RegisterCommandOptions`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IExtension/RegisterCommandOptions(ICommandLineBuilder).md) adds CLI options using `System.CommandLine`, which are parsed into configuration and accessible via `IConfiguration`.

```csharp
public class MyExtension : IExtension
{
    public void RegisterServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddSingleton<IBackendProtocol, EchoBackendProtocol>();
        services.AddSingleton<IClientConnectionNegotiator, IpDenylistNegotiator>();
    }

    public void RegisterCommandOptions(ICommandLineBuilder builder)
    {
        builder.Add(new Option<string[]>("--deny-ip", "IP addresses to block")
        {
            AllowMultipleArgumentsPerToken = true
        });
    }
}
```

## Backend Protocols

Backend protocols define application-specific backend types and handle protocol detection and traffic routing.

### Connection Routing

When a client connects, pmmux needs to decide which backend should handle it:

1. The router iterates through configured backends in configuration order
2. Each backend's [`CanHandleConnectionAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionOrientedBackend/CanHandleConnectionAsync(IClientConnectionPreview,CancellationToken).md) is called with a lazy-loaded read-only connection preview
3. The preview lets backends inspect connection properties and peek at the incoming stream
4. The backend selected by the routing strategy handles the connection (lower priority backends may not be evaluated depending on the strategy)
5. The router calls [`CreateBackendConnectionAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionOrientedBackend/CreateBackendConnectionAsync(IClientConnection,CancellationToken).md) to establish the backend connection
6. Data flows bidirectionally between client and backend via [`System.IO.Pipelines`](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipelines)

### Creating a Backend Protocol

Backend protocols implement at least two interfaces. [`IBackendProtocol`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IBackendProtocol/index.md) is a factory that creates backend instances from configuration. The [`Name`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IBackendProtocol/Name.md) property determines how the protocol is referenced in the CLI (e.g., `-b "my-backend:echo:prefix=Hello"` specifies a backend named `my-backend` with protocol `echo` and parameters `prefix=Hello`). [`CreateBackendAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IBackendProtocol/CreateBackendAsync(BackendSpec,CancellationToken).md) uses the specification to create the backend instance.

[`IConnectionOrientedBackend`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionOrientedBackend/index.md) (for TCP) or [`IConnectionlessBackend`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionlessBackend/index.md) (for UDP) is the implementation of the backend itself. The [`Backend`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IBackend/Backend.md) property returns identity info used for routing, metrics, and health tracking. [`CanHandleConnectionAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionOrientedBackend/CanHandleConnectionAsync(IClientConnectionPreview,CancellationToken).md) decides if this backend should handle a connection, and if determined to be the best candidate, the router will send the connection to [`CreateBackendConnectionAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionOrientedBackend/CreateBackendConnectionAsync(IClientConnection,CancellationToken).md) to create the backend connection and start the data relay. [`InitializeAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionOrientedBackend/InitializeAsync(CancellationToken).md) and `DisposeAsync` are used for lifecycle management.

Backends that support health checks also implement [`IHealthCheckBackend`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IHealthCheckBackend/index.md).

### Connection Preview vs Connection

During routing, backends receive [`IClientConnectionPreview`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionPreview/index.md), a read-only view for inspecting [`Properties`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionPreview/Properties.md) (metadata from negotiators like TLS SNI) and peeking at the buffered data stream with [`Ingress`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionPreview/Ingress.md).

The preview buffers data so it can be replayed once routing selection is complete. Once a backend is selected, it receives the full data stream as an [`IClientConnection`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnection/index.md) with read/write access.

### The IConnection Interface

[`IConnection`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnection/index.md) is used for both client and backend connections. It exposes [`GetReader()`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnection/GetReader().md) returning a [`PipeReader`](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipelines.pipereader) for reading data from the connection, [`GetWriter()`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnection/GetWriter().md) returning a [`PipeWriter`](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipelines.pipewriter) for writing data to the connection, and [`Properties`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnection/Properties.md) for metadata. [`IClientConnection`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnection/index.md) extends it with client-specific functionality like preview support.

The multiplexer relays data bidirectionally: client ingress -> backend writer, backend reader -> client egress.

### Example: Echo Backend

A minimal backend that echoes received data back to the client.

```csharp
public class EchoBackendProtocol : IBackendProtocol
{
    public string Name => "echo";

    public async Task<IBackend> CreateBackendAsync(BackendSpec spec, CancellationToken ct)
    {
        return new EchoBackend(spec);
    }
}

public class EchoBackend(BackendSpec spec) : IConnectionOrientedBackend
{
    public BackendInfo Backend => new(spec, new Dictionary<string, string>(), PriorityTier.Normal);

    public async Task<bool> CanHandleConnectionAsync(IClientConnectionPreview client, CancellationToken ct)
    {
        return true;
    }

    public async Task<IConnection> CreateBackendConnectionAsync(IClientConnection client, CancellationToken ct)
    {
        return new EchoConnection(client);
    }

    public async Task InitializeAsync(CancellationToken ct) { }
    public async ValueTask DisposeAsync() { }
}

public class EchoConnection(IClientConnection client) : IConnection
{
    private readonly Pipe _outbound = new();

    public IReadOnlyDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

    public PipeReader GetReader()
    {
        _ = EchoAsync();
        return _outbound.Reader;
    }

    public PipeWriter GetWriter()
    {
        return PipeWriter.Create(Stream.Null);
    }

    private async Task EchoAsync()
    {
        var reader = client.GetReader();
        while (true)
        {
            var result = await reader.ReadAsync();
            if (result.IsCompleted)
            {
                break;
            }

            await _outbound.Writer.WriteAsync(result.Buffer.ToArray());
            reader.AdvanceTo(result.Buffer.End);
        }
        await _outbound.Writer.CompleteAsync();
    }

    public async Task CloseAsync() { }
    public async ValueTask DisposeAsync() { }
}
```

```sh
# Usage
pmmux -x MyExtension.dll -b "myecho:echo" -p 8080:8080:tcp
```

## Routing Strategies

Routing strategies decide which backend receives a connection when multiple backends can handle it.

### Strategy Evaluation

The router passes matched backends as [`IAsyncEnumerable<BackendStatusInfo>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1). Each [`BackendStatusInfo`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/BackendStatusInfo/index.md) includes backend identity and health status. Strategies can use any criteria - priority tier, health, random selection, or custom logic. Some strategies (like `first-available`) short-circuit after the first match, while others (like `random`) enumerate all candidates.

### Creating a Routing Strategy

Routing strategies implement [`IRoutingStrategy`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IRoutingStrategy/index.md). The [`Name`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IRoutingStrategy/Name.md) property determines how the strategy is referenced in config (e.g., `-r random`). The [`SelectBackendAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IRoutingStrategy/SelectBackendAsync(ClientInfo,IReadOnlyDictionary_string,string_,IAsyncEnumerable_BackendStatusInfo_,CancellationToken).md) method receives matched backends and returns the selected one.

### Example: Random Strategy

This strategy collects healthy backends and randomly selects one, distributing load across the pool.

```csharp
public class RandomRoutingStrategy : IRoutingStrategy
{
    public string Name => "random";

    public async Task<BackendInfo?> SelectBackendAsync(ClientInfo client, IReadOnlyDictionary<string, string> clientConnectionProperties, IAsyncEnumerable<BackendStatusInfo> matchedBackends, CancellationToken ct)
    {
        var candidates = new List<BackendInfo>();

        await foreach (var backend in matchedBackends.WithCancellation(ct))
        {
            if (backend.Status is BackendStatus.Healthy or BackendStatus.Unknown)
            {
                candidates.Add(backend.Backend);
            }
        }

        if (candidates.Count == 0)
        {
            return null;
        }
        return candidates[Random.Shared.Next(candidates.Count)];
    }
}
```

```sh
# Usage
pmmux -x MyExtension.dll -r random -b web1:pass:ip=10.0.0.1,port=80 -b web2:pass:ip=10.0.0.2,port=80
```

## Connection Negotiators

Connection negotiators intercept raw socket connections before routing for transport-level processing like TLS termination, protocol detection, or connection filtering.

### The Negotiation Pipeline

When a client connects, pmmux passes the raw socket through a chain of registered negotiators. Each negotiator can:
- Accept the connection by returning [`Result.Accept`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionNegotiator/Result/Accept(IClientConnection).md) with transformations applied (e.g., decrypted TLS stream) and metadata in [`Properties`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/ClientConnectionContext/Properties.md)
- Reject the connection by returning [`Result.Reject`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionNegotiator/Result/Reject(string).md) to terminate the chain
- Pass to the next negotiator by invoking and returning the `next()` function

Negotiators can add properties to the connection context that are visible to backends and subsequent negotiators. For example, TLS negotiators add `tls.sni` for backends to match against.

### Creating a Connection Negotiator

Connection negotiators implement [`IClientConnectionNegotiator`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionNegotiator/index.md). The [`NegotiateAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionNegotiator/NegotiateAsync(ClientConnectionContext,Func_Task_Result__,CancellationToken).md) method receives a [`ClientConnectionContext`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/ClientConnectionContext/index.md) containing the socket, stream, and properties, plus a `next()` function to invoke the next negotiator in the chain. Negotiators that accept a connection return `Result.Accept` with an [`IClientConnection`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnection/index.md) implementation, which can wrap the stream for transformations like TLS decryption.

### Example: IP Denylist

This negotiator filters connections by IP address. It reads blocked IPs from configuration, checks incoming connections, and either rejects them or passes to the next negotiator.

```csharp
public class IpDenylistNegotiator(HashSet<IPAddress> blockedIps) : IClientConnectionNegotiator
{
    public string Name => "ip-denylist";

    public async Task<Result> NegotiateAsync(ClientConnectionContext context, Func<Task<Result>> next, CancellationToken ct)
    {
        if (blockedIps.Contains(context.Client.RemoteEndpoint.Address))
        {
            return Result.Reject("IP blocked");
        }

        return await next();
    }
}
```

```sh
# Usage
pmmux -x MyExtension.dll --deny-ip 192.168.1.100 --deny-ip 10.0.0.50 -p 8080:8080:tcp
```

## Metric Sinks

Metric sinks receive telemetry data from pmmux for export to monitoring systems.

### Metric Collection

[`IMetricReporter`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IMetricReporter/index.md) collects metrics throughout pmmux and dispatches them to all registered [`IMetricSink`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IMetricSink/index.md) implementations.

Metric types:
- **Counters**: Cumulative values (total connections, bytes transferred)
- **Gauges**: Point-in-time values (active connections, queue depth)
- **Durations**: Timing measurements (health check latency, selection time)

Events (backend added, connection failed) are reported as counters with a value of 1.

### Creating a Metric Sink

Metric sinks implement [`IMetricSink`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IMetricSink/index.md). The [`ReceiveMetricAsync`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IMetricSink/ReceiveMetricAsync(Metric,DateTimeOffset).md) method receives individual metrics as reported. Sinks can batch, filter, or forward them immediately.

### Example: JSON File Sink

This sink writes metrics to a JSON file:

```csharp
public class JsonFileMetricSink(string path) : IMetricSink
{
    public async Task ReceiveMetricAsync(Metric metric, DateTimeOffset captured)
    {
        await using var stream = new FileStream(path, FileMode.Append);
        await using var writer = new StreamWriter(stream);

        var json = JsonSerializer.Serialize(metric);
        await writer.WriteLineAsync(json);
    }
}
```

See [OTLP extension](../src/Pmmux.Extensions.Otlp/README.md) for a full OpenTelemetry implementation.

## Common Services

Extensions have access to services through dependency injection that provide runtime state and event notifications.

### Event Notifications

[`IEventNotifier`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IEventNotifier/index.md) provides event notifications for runtime state changes. Available events:

- `BackendAdded`, `BackendRemoved` - backend lifecycle
- `PortMapAdded`, `PortMapRemoved`, `PortMapChanged` - NAT port mapping changes
- `HealthCheckAdded`, `HealthCheckRemoved` - health check configuration changes

```csharp
public class MyExtension : IExtension
{
    public void RegisterServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddSingleton<MyService>();
    }
}

public class MyService : IDisposable
{
    private readonly IEventNotifier _eventNotifier;

    public MyService(IEventNotifier eventNotifier)
    {
        _eventNotifier = eventNotifier;
        _eventNotifier.BackendAdded += OnBackendAdded;
        _eventNotifier.BackendRemoved += OnBackendRemoved;
    }

    private void OnBackendAdded(object sender, BackendSpec backend)
    {
        // React to new backend
    }

    private void OnBackendRemoved(object sender, BackendSpec backend)
    {
        // Clean up resources
    }

    public void Dispose()
    {
        _eventNotifier.BackendAdded -= OnBackendAdded;
        _eventNotifier.BackendRemoved -= OnBackendRemoved;
    }
}
```

### Metrics Reporting

[`IMetricReporter`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IMetricReporter/index.md) allows extensions to report custom metrics that are dispatched to all registered metric sinks. This can be used to track extension-specific operations and performance.

```csharp
public class MyBackend : IConnectionOrientedBackend
{
    private readonly IMetricReporter _metricReporter;

    public async Task<IConnection> CreateBackendConnectionAsync(IClientConnection client, CancellationToken ct)
    {
        var metadata = new Dictionary<string, string?>
        {
            ["client"] = client.Client.RemoteEndpoint?.ToString()
        };
        _metricReporter.ReportEvent("my-backend.connection", "my-backend", metadata);
        // Create connection
    }
}
```

## Common Extension Points

Interface | Purpose
-|-
[`IExtension`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IExtension/index.md) | Extension entry point
[`IBackendProtocol`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IBackendProtocol/index.md) | Backend factory
[`IConnectionOrientedBackend`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionOrientedBackend/index.md) | TCP backend implementation
[`IConnectionlessBackend`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnectionlessBackend/index.md) | UDP backend implementation
[`IHealthCheckBackend`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IHealthCheckBackend/index.md) | Health check support
[`IRoutingStrategy`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IRoutingStrategy/index.md) | Backend selection
[`IClientConnectionNegotiator`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionNegotiator/index.md) | Transport-level processing
[`IMetricSink`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IMetricSink/index.md) | Telemetry export

## Key Types

Type | Purpose
-|-
[`BackendSpec`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/BackendSpec/index.md) | Backend configuration
[`BackendInfo`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/BackendInfo/index.md) | Backend identity
[`BackendStatusInfo`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/BackendStatusInfo/index.md) | Backend with health
[`BackendStatus`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/BackendStatus/index.md) | Health states
[`PriorityTier`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/PriorityTier/index.md) | Backend priority
[`ClientInfo`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/ClientInfo/index.md) | Client endpoint info
[`IClientConnectionPreview`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnectionPreview/index.md) | Read-only connection view
[`IClientConnection`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IClientConnection/index.md) | Client connection with preview support
[`IConnection`](../src/Pmmux.Abstractions/docs/Pmmux/Abstractions/IConnection/index.md) | Bidirectional data connection

## Built-in Extensions

Built-in extensions that ship with pmmux:

Extension | Provides | Docs
-|-|-
HTTP | `http-proxy`, `http-response` backends | [README](../src/Pmmux.Extensions.Http/README.md)
TLS | TLS termination, SNI routing | [README](../src/Pmmux.Extensions.Tls/README.md)
Management | REST API for runtime control | [README](../src/Pmmux.Extensions.Management/README.md)
OTLP | OpenTelemetry metrics | [README](../src/Pmmux.Extensions.Otlp/README.md)
BitTorrent | BitTorrent detection | [README](../src/Pmmux.Extensions.BitTorrent/README.md)

## See Also

- [Configuration Guide](configuration.md)
- [Architecture Guide](architecture.md)
- [API Reference](api-reference.md)
