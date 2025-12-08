using System.Net;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Connections;

namespace Pmmux.Extensions.Http;

internal class PmmuxConnectionListener(Channel<ConnectionContext?> channel) : IConnectionListener
{
    internal class Factory(Channel<ConnectionContext?> channel) : IConnectionListenerFactory
    {
        public ValueTask<IConnectionListener> BindAsync(
            EndPoint bindEndpoint,
            CancellationToken cancellationToken = default)
        {
            return new ValueTask<IConnectionListener>(new PmmuxConnectionListener(channel));
        }
    }

    public EndPoint EndPoint => new IPEndPoint(IPAddress.Any, 0);

    public async ValueTask<ConnectionContext?> AcceptAsync(CancellationToken cancellationToken)
    {
        if (channel.Reader.Completion.IsCompleted)
        {
            return null;
        }

        try
        {
            return await channel.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (ChannelClosedException)
        {
            return null;
        }
    }

    public ValueTask UnbindAsync(CancellationToken cancellationToken)
    {
        channel.Writer.TryComplete();

        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
