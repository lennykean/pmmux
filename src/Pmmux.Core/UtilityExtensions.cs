using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Mono.Nat;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal static class UtilityExtensions
{
    public static void SafeInvoke<TEventArgs>(
        this EventHandler<TEventArgs>? eventHandler,
        object? sender,
        TEventArgs e)
    {
        if (eventHandler is null)
        {
            return;
        }

        foreach (var subscriber in eventHandler.GetInvocationList().OfType<EventHandler<TEventArgs>>().ToArray())
        {
            try
            {
                subscriber(sender, e);
            }
            catch
            {
            }
        }
    }

    public static NatDeviceInfo DeviceInfo(this INatDevice natDevice)
    {
        return new(natDevice.NatProtocol, natDevice.DeviceEndpoint, natDevice.GetExternalIP(), DateTime.Now);
    }

    public static async Task<T> WithTimeout<T>(this Task<T> task, CancellationToken timeoutToken)
    {
        var timeoutTcs = new TaskCompletionSource<T>();

        timeoutToken.Register(() => timeoutTcs.TrySetException(new TimeoutException()));

        var winner = await Task.WhenAny(timeoutTcs.Task, task).ConfigureAwait(false);

        return await winner.ConfigureAwait(false);
    }

    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<T>();

        cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));

        var winner = await Task.WhenAny(task, tcs.Task).ConfigureAwait(false);

        return await winner.ConfigureAwait(false);
    }

    public static TimeSpan Clamp(this TimeSpan value, TimeSpan min, TimeSpan max)
    {
        if (max < min)
        {
            max = min;
        }
        return value < min ? min : value > max ? max : value;
    }

    public static bool IsShutdownSignal(this SocketException socketException)
    {
        return socketException.SocketErrorCode is
            SocketError.OperationAborted or
            SocketError.ConnectionReset or
            SocketError.Shutdown;
    }

    public static bool IsShutdownSignal(this IOException ioException)
    {
        return ioException.InnerException is SocketException sEx && sEx.IsShutdownSignal();
    }

    public static string ToDictionaryString(this IDictionary<string, string> dictionary)
    {
        return $"({string.Join(",", dictionary.Select(kv => $"[\"{kv.Key}\"=\"{kv.Value}\"]"))}";
    }
}
