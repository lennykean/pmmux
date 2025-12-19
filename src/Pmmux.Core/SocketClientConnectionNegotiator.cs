using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;
using Pmmux.Core.Configuration;

using Result = Pmmux.Abstractions.IClientConnectionNegotiator.Result;

namespace Pmmux.Core;

internal sealed class SocketClientConnectionNegotiator(
    ILoggerFactory loggerFactory,
    RouterConfig config,
    IMetricReporter metricReporter) : IClientConnectionNegotiator
{
    public string Name => "socket";

    public Task<Result> NegotiateAsync(
        ClientConnectionContext context,
        Func<Task<Result>> next,
        CancellationToken cancellationToken = default)
    {
        foreach (var (key, value) in NetworkUtility.GetProperties(context.ClientConnection))
        {
            context.Properties[$"socket.{key}"] = value;
        }

        if (NetworkUtility.TryGetMtu(context.ClientConnection, out var mtu))
        {
            context.Properties["socket.mtu"] = mtu.ToString();
        }
        else
        {
            mtu = NetworkUtility.IPV4_MIN_MTU;
        }

        return Task.FromResult(Result.Accept(new SocketClientConnection(
            context.Client,
            context.Properties,
            context.ClientConnection,
            context.ClientConnectionStream,
            loggerFactory,
            mtu,
            config.PreviewSizeLimit,
            metricReporter)));
    }
}
