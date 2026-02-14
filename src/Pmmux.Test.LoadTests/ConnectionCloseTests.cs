using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
public class ConnectionCloseTests
{
    public readonly record struct Wave(int Connections, int DelayAfterMs);
    private sealed record Counters
    {
        public int SocketErrors;
        public int IoErrors;
        public int Timeouts;
        public int Total => SocketErrors + IoErrors + Timeouts;
    }

    private static IEnumerable<TestCaseData> LoadScenarios()
    {
        yield return new TestCaseData(Enumerable.Repeat(new Wave(1, 0), 100).ToArray()).SetName("sequential");
        yield return new TestCaseData(Enumerable.Repeat(new Wave(10, 100), 5).ToArray()).SetName("burst");
        yield return new TestCaseData(new Wave[] { new(50, 0) }).SetName("tsunami");
        yield return new TestCaseData(new Wave[] { new(1, 0), new(2, 0), new(5, 0), new(10, 0), new(20, 0) })
            .SetName("ramp");
        yield return new TestCaseData(Enumerable.Range(0, 50).Select(i => new Wave(1, i % 5 == 0 ? 200 : 0)).ToArray())
            .SetName("intermittent");
    }

    [Test, TestCaseSource(nameof(LoadScenarios))]
    public async Task ConnectionClose_NoDroppedConnections(Wave[] waves)
    {
        var responsePayload = new byte[32 * 1024];
        Random.Shared.NextBytes(responsePayload);

        using var serverCts = new CancellationTokenSource();
        using var serverListener = new TcpListener(IPAddress.Loopback, 0);

        serverListener.Start();

        var serverPort = ((IPEndPoint)serverListener.LocalEndpoint).Port;
        var serverTask = RunRespondAndCloseServerAsync(serverListener, responsePayload, serverCts.Token);

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

        var sequence = 0;
        var counters = new Counters();

        foreach (var wave in waves)
        {
            var tasks = new List<Task>(wave.Connections);

            for (var c = 0; c < wave.Connections; c++)
            {
                var index = Interlocked.Increment(ref sequence);

                var client = new TcpClient();
                await client.ConnectAsync(IPAddress.Loopback, pmmuxPort);
                var accepted = await pmmuxListener.AcceptSocketAsync();
                var clientInfo = new ClientInfo(
                    accepted.LocalEndPoint as IPEndPoint,
                    accepted.RemoteEndPoint as IPEndPoint);
                await router.RouteConnectionAsync(accepted, clientInfo);

                tasks.Add(ExerciseConnectionAsync(client, index, responsePayload, counters));
            }

            await Task.WhenAll(tasks);

            if (wave.DelayAfterMs > 0)
            {
                await Task.Delay(wave.DelayAfterMs);
            }
        }

        var total = sequence;
        var errors = counters.Total;

        serverCts.Cancel();
        await ((IAsyncDisposable)router).DisposeAsync();
        await ((IAsyncDisposable)metricReporter).DisposeAsync();
        await ((IAsyncDisposable)backendMonitor).DisposeAsync();
        await eventBroker.DisposeAsync();
        pmmuxListener.Stop();
        serverListener.Stop();

        try
        {
            await serverTask;
        }
        catch (OperationCanceledException)
        {
        }

        Assert.That(errors, Is.EqualTo(0));
    }

    private static async Task ExerciseConnectionAsync(
        TcpClient client,
        int index,
        byte[] expectedResponse,
        Counters counters)
    {
        try
        {
            using (client)
            {
                var stream = client.GetStream();

                await stream.WriteAsync(new byte[] { 1 }).ConfigureAwait(false);

                var response = new byte[expectedResponse.Length];
                var totalRead = 0;

                using var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                while (totalRead < expectedResponse.Length)
                {
                    var n = await stream.ReadAsync(
                        response.AsMemory(totalRead),
                        readCts.Token).ConfigureAwait(false);
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

    private static async Task RunRespondAndCloseServerAsync(
        TcpListener listener,
        byte[] responsePayload,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
                tasks.Add(HandleRespondAndCloseAsync(client, responsePayload));
            }
        }
        catch (OperationCanceledException)
        {
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    private static async Task HandleRespondAndCloseAsync(TcpClient client, byte[] responsePayload)
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

                await stream.WriteAsync(responsePayload).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
            }
        }
        catch
        {
        }
    }
}
