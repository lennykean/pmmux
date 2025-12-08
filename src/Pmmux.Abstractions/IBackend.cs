using System;

namespace Pmmux.Abstractions;

/// <summary>
/// Base interface for all backend implementations that handle traffic in the port multiplexer.
/// </summary>
/// <remarks>
/// A backend represents a destination that can receive and process client traffic. Backends are created
/// by <see cref="IBackendProtocol"/> implementations based on <see cref="BackendSpec"/> configurations.
/// Backends should implement either <see cref="IConnectionOrientedBackend"/> for TCP protocols
/// or <see cref="IConnectionlessBackend"/> for UDP protocols.
/// </remarks>
public interface IBackend : IAsyncDisposable
{
    /// <summary>
    /// The backend metadata and configuration information.
    /// </summary>
    BackendInfo Backend { get; }
}
