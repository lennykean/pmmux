using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Mono.Nat;

namespace Pmmux.Core;

internal static class NetworkUtility
{
    internal const int IPV4_MIN_MTU = 576;

    public static async Task<INatDevice> FindNatDeviceAsync(
        IPAddress[] gateways,
        NatProtocol? natProtocol,
        CancellationToken cancellationToken = default)
    {
        var natProtocols = natProtocol is null
            ? [NatProtocol.Upnp, NatProtocol.Pmp]
            : new[] { natProtocol.Value };
        var completionSource = new TaskCompletionSource<INatDevice>();

        cancellationToken.Register(() => completionSource.TrySetCanceled(cancellationToken));

        void DeviceFoundHandler(object sender, DeviceEventArgs e)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                completionSource.TrySetCanceled(cancellationToken);
                return;
            }

            completionSource.SetResult(e.Device);
        }

        try
        {
            NatUtility.DeviceFound += DeviceFoundHandler;

            if (gateways is [var gateway] && gateway == IPAddress.Any)
            {
                NatUtility.StartDiscovery(natProtocols);
            }
            else
            {
                foreach (var search in gateways.SelectMany(g => natProtocols.Select(p => (gateway: g, natProtocol: p))))
                {
                    NatUtility.Search(search.gateway, search.natProtocol);
                }
            }

            return await completionSource.Task;
        }
        finally
        {
            NatUtility.StopDiscovery();
            NatUtility.DeviceFound -= DeviceFoundHandler;
        }
    }

    public static IPAddress[] GetGateways(IPAddress gateway, string? networkInterface)
    {
        if (gateway != IPAddress.Any)
        {
            return [gateway];
        }

        var nicGateways =
            from nic in NetworkInterface.GetAllNetworkInterfaces()
            where nic.OperationalStatus == OperationalStatus.Up
            where networkInterface is null || nic.Name.Equals(networkInterface, StringComparison.OrdinalIgnoreCase)
            from gatewayAddress in nic.GetIPProperties().GatewayAddresses
            select gatewayAddress.Address;

        return [.. nicGateways];
    }

    public static IReadOnlyDictionary<string, string> GetProperties(Socket connection)
    {
        var properties = new Dictionary<string, string>
        {
            ["address-family"] = connection.AddressFamily.ToString().ToLowerInvariant(),
            ["receive-buffer"] = connection.ReceiveBufferSize.ToString(),
            ["send-buffer"] = connection.SendBufferSize.ToString(),
            ["receive-timeout"] = connection.ReceiveTimeout.ToString(),
            ["send-timeout"] = connection.SendTimeout.ToString(),
            ["type"] = connection.SocketType.ToString().ToLowerInvariant(),
            ["protocol-type"] = connection.ProtocolType.ToString().ToLowerInvariant()
        };
        if (connection.LocalEndPoint is IPEndPoint localIpEndPoint)
        {
            properties["local-ip"] = localIpEndPoint.Address.ToString();
            properties["local-port"] = localIpEndPoint.Port.ToString();
        }
        if (connection.RemoteEndPoint is IPEndPoint remoteIpEndPoint)
        {
            properties["remote-ip"] = remoteIpEndPoint.Address.ToString();
            properties["remote-port"] = remoteIpEndPoint.Port.ToString();
        }
        if (connection.ProtocolType == ProtocolType.Tcp)
        {
            properties["no-delay"] = connection.NoDelay.ToString();
            properties["linger-state"] = connection.LingerState.Enabled.ToString();
        }

        return properties;
    }

    public static bool TryGetMtu(Socket socket, out int mtu)
    {
        mtu = default;

        if (socket.LocalEndPoint is not IPEndPoint localEndpoint)
        {
            return false;
        }
        var listenerNic = (
            from nic in NetworkInterface.GetAllNetworkInterfaces()
            where nic.OperationalStatus == OperationalStatus.Up
            where nic.GetIPProperties().UnicastAddresses.Any(address => address.Address.Equals(localEndpoint.Address))
            select nic).FirstOrDefault();

        if (listenerNic is null)
        {
            return false;
        }
        var interfaceMtu = socket.AddressFamily switch
        {
            AddressFamily.InterNetwork => listenerNic.GetIPProperties()?.GetIPv4Properties()?.Mtu,
            AddressFamily.InterNetworkV6 => listenerNic.GetIPProperties()?.GetIPv6Properties()?.Mtu,
            _ => null
        };
        if (interfaceMtu is null or <= 0)
        {
            return false;
        }
        mtu = (int)interfaceMtu;

        return true;
    }
}
