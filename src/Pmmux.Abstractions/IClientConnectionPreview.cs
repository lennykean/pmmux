using System;
using System.Collections.Generic;
using System.IO.Pipelines;

namespace Pmmux.Abstractions;

/// <summary>
/// Read-only preview of a client connection for making routing decisions without consuming data.
/// </summary>
/// <remarks>
/// Connection previews allow backends to inspect connection properties and peek at incoming data
/// for routing decisions. The preview uses a pipeline reader that supports examining buffered data
/// without consuming it. Once routing completes, the actual <see cref="IClientConnection"/> is used
/// for data transfer with all original data intact.
/// </remarks>
public interface IClientConnectionPreview : IAsyncDisposable
{
    /// <summary>
    /// Client associated with this connection.
    /// </summary>
    ClientInfo Client { get; }

    /// <summary>
    /// Metadata associated with the connection.
    /// </summary>
    /// <remarks>
    /// Properties are populated by <see cref="IClientConnectionNegotiator"/> implementations
    /// and may include <c>tls</c>, <c>tls.sni</c>, <c>host</c>, <c>path</c>, and other metadata.
    /// </remarks>
    IReadOnlyDictionary<string, string> Properties { get; }

    /// <summary>
    /// Pipeline reader for peeking at incoming data.
    /// </summary>
    /// <remarks>
    /// Use <see cref="PipeReader.ReadAsync"/> to examine buffered data, then call
    /// <see cref="PipeReader.AdvanceTo(System.SequencePosition, System.SequencePosition)"/>
    /// with the start position to avoid consuming the data. Configuration may limit buffered data.
    /// </remarks>
    PipeReader Ingress { get; }
}
