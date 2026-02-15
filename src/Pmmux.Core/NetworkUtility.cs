using System;
using System.Collections.Concurrent;
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

    private static readonly ConcurrentDictionary<IPAddress, int?> _mtuCache = new();

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

        void DeviceFoundHandler(object? sender, DeviceEventArgs e)
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
            if (connection?.LingerState?.Enabled is true)
            {
                properties["linger-state"] = bool.TrueString;
            }
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

        var cached = _mtuCache.GetOrAdd(localEndpoint.Address, static address =>
        {
            var nic = (
                from n in NetworkInterface.GetAllNetworkInterfaces()
                where n.OperationalStatus == OperationalStatus.Up
                where n.GetIPProperties().UnicastAddresses.Any(a => a.Address.Equals(address))
                select n).FirstOrDefault();

            if (nic is null)
            {
                return null;
            }

            var ipProps = nic.GetIPProperties();
            var ifMtu = address.AddressFamily switch
            {
                AddressFamily.InterNetwork => ipProps?.GetIPv4Properties()?.Mtu,
                AddressFamily.InterNetworkV6 => ipProps?.GetIPv6Properties()?.Mtu,
                _ => null
            };

            return ifMtu is > 0 ? ifMtu : null;
        });

        if (cached is not { } value)
        {
            return false;
        }

        mtu = value;
        return true;
    }
}
