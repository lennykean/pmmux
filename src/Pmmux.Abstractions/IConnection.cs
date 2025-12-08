using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace Pmmux.Abstractions;

/// <summary>
/// Bidirectional data connection using <see cref="System.IO.Pipelines"/>.
/// </summary>
/// <remarks>
/// Abstracts both client and backend connections, providing an interface to read and write
/// data through high-performance pipelines. Connections carry metadata in the <see cref="Properties"/>
/// dictionary for protocol-specific information or routing context.
/// </remarks>
public interface IConnection : IAsyncDisposable
{
    /// <summary>
    /// Metadata associated with the connection.
    /// </summary>
    IReadOnlyDictionary<string, string> Properties { get; }

    /// <summary>
    /// Get pipeline reader for reading data from this connection.
    /// </summary>
    /// <returns>A <see cref="PipeReader"/> for consuming incoming data.</returns>
    PipeReader GetReader();

    /// <summary>
    /// Get pipeline writer for writing data to this connection.
    /// </summary>
    /// <returns>A <see cref="PipeWriter"/> for sending outgoing data.</returns>
    PipeWriter GetWriter();

    /// <summary>
    /// Close the connection gracefully, allowing pending data to be flushed.
    /// </summary>
    /// <returns>A task representing the asynchronous close operation.</returns>
    Task CloseAsync();
}
