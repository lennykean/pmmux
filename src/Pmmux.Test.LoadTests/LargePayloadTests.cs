using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NUnit.Framework;

using Pmmux.Abstractions;
using Pmmux.Core;
using Pmmux.Core.Configuration;

namespace Pmmux.Test.LoadTests;

[TestFixture]
public class LargePayloadTests
{
    private sealed record Counters
    {
        public long BytesReceived;
        public int SocketErrors;
        public int IoErrors;
        public int Timeouts;
        public int ErrorTotal => SocketErrors + IoErrors + Timeouts;
    }

    private readonly record struct Jitter(int MaxPauseMs, int MinChunkSize, int MaxChunkSize);

    private const int ChunkSize = 64 * 1024;

    private static IEnumerable<TestCaseData> LargeResponseScenarios()
    {
        yield return new TestCaseData(2L * 1024 * 1024 * 1024, 1).SetName("single_2gb");
        yield return new TestCaseData(1L * 1024 * 1024 * 1024, 5).SetName("concurrent_1gb_x5");
        yield return new TestCaseData(512L * 1024 * 1024, 10).SetName("concurrent_512mb_x10");
    }

    [Test, TestCaseSource(nameof(LargeResponseScenarios))]
    [Timeout(300_000)]
    public async Task LargeResponse_NoErrors(long responseSize, int concurrency)
    {
        using var serverCts = new CancellationTokenSource();
        using var serverListener = new TcpListener(IPAddress.Loopback, 0);
        serverListener.Start();

        var serverPort = ((IPEndPoint)serverListener.LocalEndpoint).Port;
        var serverTask = RunStreamingServerAsync(serverListener, responseSize, serverCts.Token);

        using var loggerFactory = LoggerFactory.Create(b => { });
        var metricReporter = new MetricReporter([], loggerFactory);
        var eventBroker = new EventBroker();

        var backendSpec = new BackendSpec("responder", "pass", new Dictionary<string, string>
        {
            ["target.ip"] = "127.0.0.1",
            ["target.port"] = serverPort.ToString()
        });

        var routerConfig = new RouterConfig { Backends = [backendSpec] };
        var backendMonitor = new BackendMonitor(routerConfig, loggerFactory, metricReporter, eventBroker);
        var negotiator = new SocketClientConnectionNegotiator(loggerFactory, routerConfig, metricReporter);
        var strategy = new FirstAvailableRoutingStrategy();
        var protocol = new PassthroughBackend.Protocol();

        var router = new Router(
            [negotiator],
            [strategy],
            [protocol],
            eventBroker,
            backendMonitor,
            metricReporter,
            routerConfig,
            loggerFactory);

        await router.InitializeAsync(null!, CancellationToken.None);

        using var pmmuxListener = new TcpListener(IPAddress.Loopback, 0);
        pmmuxListener.Start();
        var pmmuxPort = ((IPEndPoint)pmmuxListener.LocalEndpoint).Port;

        var counters = new Counters();
        var tasks = new List<Task>(concurrency);

        for (var i = 0; i < concurrency; i++)
        {
            var client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, pmmuxPort);
            var accepted = await pmmuxListener.AcceptSocketAsync();
            var clientInfo = new ClientInfo(
                accepted.LocalEndPoint as IPEndPoint,
                accepted.RemoteEndPoint as IPEndPoint);
            await router.RouteConnectionAsync(accepted, clientInfo);

            tasks.Add(ReadFullResponseAsync(client, responseSize, counters));
        }

        await Task.WhenAll(tasks);

        serverCts.Cancel();
        await DisposeInfrastructureAsync(router, metricReporter, backendMonitor, eventBroker);
        pmmuxListener.Stop();
        serverListener.Stop();
        await AwaitServerAsync(serverTask);

        Assert.That(counters.ErrorTotal, Is.EqualTo(0));
        Assert.That(Interlocked.Read(ref counters.BytesReceived), Is.EqualTo(responseSize * concurrency));
    }

    private static IEnumerable<TestCaseData> BidirectionalScenarios()
    {
        yield return new TestCaseData(1L * 1024 * 1024 * 1024, 1).SetName("bidi_1gb");
        yield return new TestCaseData(2L * 1024 * 1024 * 1024, 1).SetName("bidi_2gb");
        yield return new TestCaseData(512L * 1024 * 1024, 5).SetName("bidi_512mb_x5");
    }

    [Test, TestCaseSource(nameof(BidirectionalScenarios))]
    [Timeout(300_000)]
    public async Task Bidirectional_NoErrors(long payloadSize, int concurrency)
    {
        using var serverCts = new CancellationTokenSource();
        using var serverListener = new TcpListener(IPAddress.Loopback, 0);
        serverListener.Start();

        var serverPort = ((IPEndPoint)serverListener.LocalEndpoint).Port;
        var serverTask = RunBidirectionalServerAsync(serverListener, payloadSize, serverCts.Token);

        using var loggerFactory = LoggerFactory.Create(b => { });
        var metricReporter = new MetricReporter([], loggerFactory);
        var eventBroker = new EventBroker();

        var backendSpec = new BackendSpec("bidi", "pass", new Dictionary<string, string>
        {
            ["target.ip"] = "127.0.0.1",
            ["target.port"] = serverPort.ToString()
        });

        var routerConfig = new RouterConfig { Backends = [backendSpec] };
        var backendMonitor = new BackendMonitor(routerConfig, loggerFactory, metricReporter, eventBroker);
        var negotiator = new SocketClientConnectionNegotiator(loggerFactory, routerConfig, metricReporter);
        var strategy = new FirstAvailableRoutingStrategy();
        var protocol = new PassthroughBackend.Protocol();

        var router = new Router(
            [negotiator],
            [strategy],
            [protocol],
            eventBroker,
            backendMonitor,
            metricReporter,
            routerConfig,
            loggerFactory);

        await router.InitializeAsync(null!, CancellationToken.None);

        using var pmmuxListener = new TcpListener(IPAddress.Loopback, 0);
        pmmuxListener.Start();
        var pmmuxPort = ((IPEndPoint)pmmuxListener.LocalEndpoint).Port;

        var counters = new Counters();
        var tasks = new List<Task>(concurrency);

        for (var i = 0; i < concurrency; i++)
        {
            var client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, pmmuxPort);
            var accepted = await pmmuxListener.AcceptSocketAsync();
            var clientInfo = new ClientInfo(
                accepted.LocalEndPoint as IPEndPoint,
                accepted.RemoteEndPoint as IPEndPoint);
            await router.RouteConnectionAsync(accepted, clientInfo);

            tasks.Add(ExerciseBidirectionalAsync(client, payloadSize, counters));
        }

        await Task.WhenAll(tasks);

        serverCts.Cancel();
        await DisposeInfrastructureAsync(router, metricReporter, backendMonitor, eventBroker);
        pmmuxListener.Stop();
        serverListener.Stop();
        await AwaitServerAsync(serverTask);

        Assert.That(counters.ErrorTotal, Is.EqualTo(0));
        Assert.That(Interlocked.Read(ref counters.BytesReceived), Is.EqualTo(payloadSize * concurrency));
    }

    private static IEnumerable<TestCaseData> MidTransferCloseScenarios()
    {
        yield return new TestCaseData(2L * 1024 * 1024 * 1024, 1L * 1024 * 1024, 1).SetName("close_at_1mb_of_2gb");
        yield return new TestCaseData(2L * 1024 * 1024 * 1024, 64L * 1024, 1).SetName("close_at_64kb_of_2gb");
        yield return new TestCaseData(2L * 1024 * 1024 * 1024, 1L * 1024 * 1024, 10).SetName("close_at_1mb_of_2gb_x10");
    }

    [Test, TestCaseSource(nameof(MidTransferCloseScenarios))]
    [Timeout(300_000)]
    public async Task ClientCloseMidTransfer_NoErrors(long responseSize, long closeAfterBytes, int concurrency)
    {
        using var serverCts = new CancellationTokenSource();
        using var serverListener = new TcpListener(IPAddress.Loopback, 0);
        serverListener.Start();

        var serverPort = ((IPEndPoint)serverListener.LocalEndpoint).Port;
        var serverTask = RunStreamingServerAsync(serverListener, responseSize, serverCts.Token);

        using var loggerFactory = LoggerFactory.Create(b => { });
        var metricReporter = new MetricReporter([], loggerFactory);
        var eventBroker = new EventBroker();

        var backendSpec = new BackendSpec("responder", "pass", new Dictionary<string, string>
        {
            ["target.ip"] = "127.0.0.1",
            ["target.port"] = serverPort.ToString()
        });

        var routerConfig = new RouterConfig { Backends = [backendSpec] };
        var backendMonitor = new BackendMonitor(routerConfig, loggerFactory, metricReporter, eventBroker);
        var negotiator = new SocketClientConnectionNegotiator(loggerFactory, routerConfig, metricReporter);
        var strategy = new FirstAvailableRoutingStrategy();
        var protocol = new PassthroughBackend.Protocol();

        var router = new Router(
            [negotiator],
            [strategy],
            [protocol],
            eventBroker,
            backendMonitor,
            metricReporter,
            routerConfig,
            loggerFactory);

        await router.InitializeAsync(null!, CancellationToken.None);

        using var pmmuxListener = new TcpListener(IPAddress.Loopback, 0);
        pmmuxListener.Start();
        var pmmuxPort = ((IPEndPoint)pmmuxListener.LocalEndpoint).Port;

        var counters = new Counters();
        var tasks = new List<Task>(concurrency);

        for (var i = 0; i < concurrency; i++)
        {
            var client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, pmmuxPort);
            var accepted = await pmmuxListener.AcceptSocketAsync();
            var clientInfo = new ClientInfo(
                accepted.LocalEndPoint as IPEndPoint,
                accepted.RemoteEndPoint as IPEndPoint);
            await router.RouteConnectionAsync(accepted, clientInfo);

            tasks.Add(ReadPartialThenCloseAsync(client, closeAfterBytes, counters));
        }

        await Task.WhenAll(tasks);

        serverCts.Cancel();
        await DisposeInfrastructureAsync(router, metricReporter, backendMonitor, eventBroker);
        pmmuxListener.Stop();
        serverListener.Stop();
        await AwaitServerAsync(serverTask);

        Assert.That(counters.ErrorTotal, Is.EqualTo(0));
    }

    [Test]
    [Timeout(300_000)]
    public async Task SlowConsumer_NoErrors()
    {
        var responseSize = 2L * 1024 * 1024 * 1024;

        using var serverCts = new CancellationTokenSource();
        using var serverListener = new TcpListener(IPAddress.Loopback, 0);
        serverListener.Start();

        var serverPort = ((IPEndPoint)serverListener.LocalEndpoint).Port;
        var serverTask = RunStreamingServerAsync(serverListener, responseSize, serverCts.Token);

        using var loggerFactory = LoggerFactory.Create(b => { });
        var metricReporter = new MetricReporter([], loggerFactory);
        var eventBroker = new EventBroker();

        var backendSpec = new BackendSpec("responder", "pass", new Dictionary<string, string>
        {
            ["target.ip"] = "127.0.0.1",
            ["target.port"] = serverPort.ToString()
        });

        var routerConfig = new RouterConfig { Backends = [backendSpec] };
        var backendMonitor = new BackendMonitor(routerConfig, loggerFactory, metricReporter, eventBroker);
        var negotiator = new SocketClientConnectionNegotiator(loggerFactory, routerConfig, metricReporter);
        var strategy = new FirstAvailableRoutingStrategy();
        var protocol = new PassthroughBackend.Protocol();

        var router = new Router(
            [negotiator],
            [strategy],
            [protocol],
            eventBroker,
            backendMonitor,
            metricReporter,
            routerConfig,
            loggerFactory);

        await router.InitializeAsync(null!, CancellationToken.None);

        using var pmmuxListener = new TcpListener(IPAddress.Loopback, 0);
        pmmuxListener.Start();
        var pmmuxPort = ((IPEndPoint)pmmuxListener.LocalEndpoint).Port;

        var counters = new Counters();

        var client = new TcpClient();
        await client.ConnectAsync(IPAddress.Loopback, pmmuxPort);
        var accepted = await pmmuxListener.AcceptSocketAsync();
        var clientInfo = new ClientInfo(
            accepted.LocalEndPoint as IPEndPoint,
            accepted.RemoteEndPoint as IPEndPoint);
        await router.RouteConnectionAsync(accepted, clientInfo);

        await ReadSlowlyAsync(client, responseSize, readBufferSize: 512 * 1024, delayMs: 5, counters);

        serverCts.Cancel();
        await DisposeInfrastructureAsync(router, metricReporter, backendMonitor, eventBroker);
        pmmuxListener.Stop();
        serverListener.Stop();
        await AwaitServerAsync(serverTask);

        Assert.That(counters.ErrorTotal, Is.EqualTo(0));
        Assert.That(Interlocked.Read(ref counters.BytesReceived), Is.EqualTo(responseSize));
    }

    private static IEnumerable<TestCaseData> FlakyServerScenarios()
    {
        yield return new TestCaseData(100L * 1024 * 1024, 1).SetName("flaky_100mb");
        yield return new TestCaseData(100L * 1024 * 1024, 5).SetName("flaky_100mb_x5");
    }

    [Test, TestCaseSource(nameof(FlakyServerScenarios))]
    [Timeout(300_000)]
    public async Task FlakyServer_NoErrors(long responseSize, int concurrency)
    {
        var jitter = new Jitter(MaxPauseMs: 50, MinChunkSize: 1024, MaxChunkSize: ChunkSize);

        using var serverCts = new CancellationTokenSource();
        using var serverListener = new TcpListener(IPAddress.Loopback, 0);
        serverListener.Start();

        var serverPort = ((IPEndPoint)serverListener.LocalEndpoint).Port;
        var serverTask = RunStreamingServerAsync(serverListener, responseSize, serverCts.Token, jitter);

        using var loggerFactory = LoggerFactory.Create(b => { });
        var metricReporter = new MetricReporter([], loggerFactory);
        var eventBroker = new EventBroker();

        var backendSpec = new BackendSpec("responder", "pass", new Dictionary<string, string>
        {
            ["target.ip"] = "127.0.0.1",
            ["target.port"] = serverPort.ToString()
        });

        var routerConfig = new RouterConfig { Backends = [backendSpec] };
        var backendMonitor = new BackendMonitor(routerConfig, loggerFactory, metricReporter, eventBroker);
        var negotiator = new SocketClientConnectionNegotiator(loggerFactory, routerConfig, metricReporter);
        var strategy = new FirstAvailableRoutingStrategy();
        var protocol = new PassthroughBackend.Protocol();

        var router = new Router(
            [negotiator],
            [strategy],
            [protocol],
            eventBroker,
            backendMonitor,
            metricReporter,
            routerConfig,
            loggerFactory);

        await router.InitializeAsync(null!, CancellationToken.None);

        using var pmmuxListener = new TcpListener(IPAddress.Loopback, 0);
        pmmuxListener.Start();
        var pmmuxPort = ((IPEndPoint)pmmuxListener.LocalEndpoint).Port;

        var counters = new Counters();
        var tasks = new List<Task>(concurrency);

        for (var i = 0; i < concurrency; i++)
        {
            var client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, pmmuxPort);
            var accepted = await pmmuxListener.AcceptSocketAsync();
            var clientInfo = new ClientInfo(
                accepted.LocalEndPoint as IPEndPoint,
                accepted.RemoteEndPoint as IPEndPoint);
            await router.RouteConnectionAsync(accepted, clientInfo);

            tasks.Add(ReadFullResponseAsync(client, responseSize, counters));
        }

        await Task.WhenAll(tasks);

        serverCts.Cancel();
        await DisposeInfrastructureAsync(router, metricReporter, backendMonitor, eventBroker);
        pmmuxListener.Stop();
        serverListener.Stop();
        await AwaitServerAsync(serverTask);

        Assert.That(counters.ErrorTotal, Is.EqualTo(0));
        Assert.That(Interlocked.Read(ref counters.BytesReceived), Is.EqualTo(responseSize * concurrency));
    }

    // --- Client behaviors ---

    private static async Task ReadFullResponseAsync(TcpClient client, long expectedSize, Counters counters)
    {
        try
        {
            using (client)
            {
                var stream = client.GetStream();

                await stream.WriteAsync(new byte[] { 1 }).ConfigureAwait(false);

                var buffer = new byte[ChunkSize];
                long totalRead = 0;

                using var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(120));

                while (totalRead < expectedSize)
                {
                    var n = await stream.ReadAsync(buffer, readCts.Token).ConfigureAwait(false);
                    if (n == 0)
                    {
                        break;
                    }
                    totalRead += n;
                }

                Interlocked.Add(ref counters.BytesReceived, totalRead);
            }
        }
        catch (SocketException)
        {
            Interlocked.Increment(ref counters.SocketErrors);
        }
        catch (IOException)
        {
            Interlocked.Increment(ref counters.IoErrors);
        }
        catch (OperationCanceledException)
        {
            Interlocked.Increment(ref counters.Timeouts);
        }
    }

    private static async Task ExerciseBidirectionalAsync(TcpClient client, long payloadSize, Counters counters)
    {
        try
        {
            using (client)
            {
                var stream = client.GetStream();

                await stream.WriteAsync(new byte[] { 1 }).ConfigureAwait(false);

                var sendChunk = new byte[ChunkSize];
                Random.Shared.NextBytes(sendChunk);

                var sendTask = Task.Run(async () =>
                {
                    try
                    {
                        var remaining = payloadSize;
                        while (remaining > 0)
                        {
                            var toSend = (int)Math.Min(sendChunk.Length, remaining);
                            await stream.WriteAsync(sendChunk.AsMemory(0, toSend)).ConfigureAwait(false);
                            remaining -= toSend;
                        }
                    }
                    catch
                    {
                    }
                });

                var buffer = new byte[ChunkSize];
                long totalRead = 0;

                using var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(120));

                while (totalRead < payloadSize)
                {
                    var n = await stream.ReadAsync(buffer, readCts.Token).ConfigureAwait(false);
                    if (n == 0)
                    {
                        break;
                    }
                    totalRead += n;
                }

                Interlocked.Add(ref counters.BytesReceived, totalRead);

                await sendTask.ConfigureAwait(false);
            }
        }
        catch (SocketException)
        {
            Interlocked.Increment(ref counters.SocketErrors);
        }
        catch (IOException)
        {
            Interlocked.Increment(ref counters.IoErrors);
        }
        catch (OperationCanceledException)
        {
            Interlocked.Increment(ref counters.Timeouts);
        }
    }

    private static async Task ReadPartialThenCloseAsync(TcpClient client, long closeAfterBytes, Counters counters)
    {
        try
        {
            using (client)
            {
                var stream = client.GetStream();

                await stream.WriteAsync(new byte[] { 1 }).ConfigureAwait(false);

                var buffer = new byte[ChunkSize];
                long totalRead = 0;

                using var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                while (totalRead < closeAfterBytes)
                {
                    var n = await stream.ReadAsync(buffer, readCts.Token).ConfigureAwait(false);
                    if (n == 0)
                    {
                        break;
                    }
                    totalRead += n;
                }
            }
        }
        catch (SocketException)
        {
            Interlocked.Increment(ref counters.SocketErrors);
        }
        catch (IOException)
        {
            Interlocked.Increment(ref counters.IoErrors);
        }
        catch (OperationCanceledException)
        {
            Interlocked.Increment(ref counters.Timeouts);
        }
    }

    private static async Task ReadSlowlyAsync(
        TcpClient client,
        long expectedSize,
        int readBufferSize,
        int delayMs,
        Counters counters)
    {
        try
        {
            using (client)
            {
                var stream = client.GetStream();

                await stream.WriteAsync(new byte[] { 1 }).ConfigureAwait(false);

                var buffer = new byte[readBufferSize];
                long totalRead = 0;

                using var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(120));

                while (totalRead < expectedSize)
                {
                    var n = await stream.ReadAsync(buffer, readCts.Token).ConfigureAwait(false);
                    if (n == 0)
                    {
                        break;
                    }
                    totalRead += n;
                    await Task.Delay(delayMs).ConfigureAwait(false);
                }

                Interlocked.Add(ref counters.BytesReceived, totalRead);
            }
        }
        catch (SocketException)
        {
            Interlocked.Increment(ref counters.SocketErrors);
        }
        catch (IOException)
        {
            Interlocked.Increment(ref counters.IoErrors);
        }
        catch (OperationCanceledException)
        {
            Interlocked.Increment(ref counters.Timeouts);
        }
    }

    // --- Server behaviors ---

    private static async Task RunStreamingServerAsync(
        TcpListener listener,
        long responseSize,
        CancellationToken cancellationToken,
        Jitter? jitter = null)
    {
        var tasks = new List<Task>();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
                tasks.Add(HandleStreamingAsync(client, responseSize, jitter));
            }
        }
        catch (OperationCanceledException)
        {
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    private static async Task HandleStreamingAsync(TcpClient client, long responseSize, Jitter? jitter)
    {
        try
        {
            using (client)
            {
                var stream = client.GetStream();

                var trigger = new byte[1];
                var n = await stream.ReadAsync(trigger).ConfigureAwait(false);
                if (n == 0)
                {
                    return;
                }

                var chunk = new byte[ChunkSize];
                Random.Shared.NextBytes(chunk);
                var remaining = responseSize;

                while (remaining > 0)
                {
                    var writeSize = jitter is { } j
                        ? Random.Shared.Next(j.MinChunkSize, j.MaxChunkSize + 1)
                        : chunk.Length;
                    var toSend = (int)Math.Min(writeSize, remaining);
                    await stream.WriteAsync(chunk.AsMemory(0, toSend)).ConfigureAwait(false);
                    remaining -= toSend;

                    if (jitter is { } jp && Random.Shared.Next(4) == 0)
                    {
                        await Task.Delay(Random.Shared.Next(1, jp.MaxPauseMs + 1)).ConfigureAwait(false);
                    }
                }

                await stream.FlushAsync().ConfigureAwait(false);
            }
        }
        catch
        {
        }
    }

    private static async Task RunBidirectionalServerAsync(
        TcpListener listener,
        long responseSize,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
                tasks.Add(HandleBidirectionalAsync(client, responseSize));
            }
        }
        catch (OperationCanceledException)
        {
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    private static async Task HandleBidirectionalAsync(TcpClient client, long responseSize)
    {
        try
        {
            using (client)
            {
                var stream = client.GetStream();

                var trigger = new byte[1];
                var n = await stream.ReadAsync(trigger).ConfigureAwait(false);
                if (n == 0)
                {
                    return;
                }

                var drainTask = Task.Run(async () =>
                {
                    try
                    {
                        var buf = new byte[ChunkSize];
                        while (await stream.ReadAsync(buf).ConfigureAwait(false) > 0)
                        {
                        }
                    }
                    catch
                    {
                    }
                });

                var chunk = new byte[ChunkSize];
                Random.Shared.NextBytes(chunk);
                var remaining = responseSize;

                while (remaining > 0)
                {
                    var toSend = (int)Math.Min(chunk.Length, remaining);
                    await stream.WriteAsync(chunk.AsMemory(0, toSend)).ConfigureAwait(false);
                    remaining -= toSend;
                }

                await stream.FlushAsync().ConfigureAwait(false);
                await drainTask.ConfigureAwait(false);
            }
        }
        catch
        {
        }
    }

    // --- Cleanup ---

    private static async Task DisposeInfrastructureAsync(
        Router router,
        MetricReporter metricReporter,
        BackendMonitor backendMonitor,
        EventBroker eventBroker)
    {
        await ((IAsyncDisposable)router).DisposeAsync();
        await ((IAsyncDisposable)metricReporter).DisposeAsync();
        await ((IAsyncDisposable)backendMonitor).DisposeAsync();
        await eventBroker.DisposeAsync();
    }

    private static async Task AwaitServerAsync(Task serverTask)
    {
        try
        {
            await serverTask;
        }
        catch (OperationCanceledException)
        {
        }
    }
}
