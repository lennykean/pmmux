using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Mono.Nat;

using Pmmux.Abstractions;

using static Pmmux.Core.Configuration.ListenerConfig;

namespace Pmmux.Core;

internal sealed class ClientWriter(ListenerInfo listenerInfo, Socket listener, ClientInfo clientInfo) : IClientWriter
{
    public sealed class Factory(ConcurrentDictionary<BindingConfig, PortMultiplexer.BoundListener> listeners)
        : IClientWriterFactory
    {
        public IClientWriter CreateWriter(ClientInfo clientInfo)
        {
            var protocolListeners = listeners
                .Where(kv => kv.Key.NetworkProtocol == Protocol.Udp)
                .Select(kv => kv.Value)
                .ToArray();

            if (protocolListeners is [var listener])
            {
                return new ClientWriter(listener.ListenerInfo, listener.Listener, clientInfo);
            }

            var endpointListener = protocolListeners
                .SingleOrDefault(l => l.ListenerInfo.LocalEndPoint == clientInfo.LocalEndpoint);

            if (endpointListener != default)
            {
                return new ClientWriter(endpointListener.ListenerInfo, endpointListener.Listener, clientInfo);
            }

            var portListener = protocolListeners
                .FirstOrDefault(l => l.ListenerInfo.LocalEndPoint.Port == clientInfo.LocalEndpoint?.Port);

            if (portListener != default)
            {
                return new ClientWriter(portListener.ListenerInfo, portListener.Listener, clientInfo);
            }

            throw new ArgumentException("listener could not be determined", nameof(clientInfo));
        }
    }

    public ListenerInfo ListenerInfo => listenerInfo;

    public async Task WriteAsync(byte[] data, CancellationToken cancellationToken)
    {
        await listener.SendToAsync(data, SocketFlags.None, clientInfo.RemoteEndpoint!).ConfigureAwait(false);
    }
}
