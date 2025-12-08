using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Writes data to a client via a listener.
/// </summary>
public interface IClientWriter
{
    /// <summary>
    /// Listener the writer is associated with.
    /// </summary>
    ListenerInfo ListenerInfo { get; }

    /// <summary>
    /// Write data to the client through the listener.
    /// </summary>
    /// <param name="data">The data to write.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task WriteAsync(byte[] data, CancellationToken cancellationToken);
}
