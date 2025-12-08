using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Pmmux.Abstractions;

namespace Pmmux.Core;

/// <summary>
/// Backend that forwards traffic to a configured upstream endpoint.
/// </summary>
public class PassthroughBackend : IConnectionOrientedBackend, IConnectionlessBackend, IHealthCheckBackend
{
    /// <summary>
    /// Protocol factory for <see cref="PassthroughBackend"/>.
    /// </summary>
    public sealed class Protocol : IBackendProtocol
    {
        /// <inheritdoc />
        public string Name => "pass";

        /// <inheritdoc />
        public Task<IBackend> CreateBackendAsync(BackendSpec spec, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IBackend>(new PassthroughBackend(spec));
        }
    }

    private class Connection : IConnection
    {
        private readonly TcpClient _backendConnection;
        private readonly PipeReader _reader;
        private readonly PipeWriter _writer;

        public Connection(TcpClient backendConnection)
        {
            _backendConnection = backendConnection;

            var backendStream = backendConnection.GetStream();

            _reader = PipeReader.Create(backendStream);
            _writer = PipeWriter.Create(backendStream);
        }

        public IReadOnlyDictionary<string, string> Properties => new Dictionary<string, string>();

        public PipeReader GetReader() => _reader;
        public PipeWriter GetWriter() => _writer;

        public async Task CloseAsync()
        {
            await _writer.CompleteAsync().ConfigureAwait(false);
            await _reader.CompleteAsync().ConfigureAwait(false);

            try
            {
                if (_backendConnection.Connected)
                {
                    _backendConnection.Client.Shutdown(SocketShutdown.Both);
                    _backendConnection.Client.Close();
                }
            }
            catch
            {
            }
        }

        public async ValueTask DisposeAsync()
        {
            await CloseAsync().ConfigureAwait(false);
            _backendConnection.Dispose();
        }
    }

    /// <param name="backendSpec">The backend specification containing connection parameters.</param>
    public PassthroughBackend(BackendSpec backendSpec)
    {
        if (!backendSpec.Parameters.TryGetValue("ip", out _ip))
        {
            throw new InvalidOperationException($"ip is required for {backendSpec.Name} backend");
        }
        if (!backendSpec.Parameters.TryGetValue("port", out var port) || !int.TryParse(port, out _port))
        {
            throw new InvalidOperationException($"port is required for {backendSpec.Name} backend");
        }

        var priority = PriorityTier.Normal;

        if (backendSpec.Parameters.TryGetValue("priority", out var p))
        {
            if (!Enum.TryParse(p, ignoreCase: true, out priority))
            {
                var values = string.Join(',', Enum.GetValues(typeof(PriorityTier))).ToLowerInvariant();
                throw new InvalidOperationException($"invalid priority {p}, must be one of {values}");
            }
        }

        Backend = new(backendSpec, new Dictionary<string, string>()
        {
            ["ip"] = _ip,
            ["port"] = _port.ToString()
        }, priority);
    }

    private readonly string _ip;
    private readonly int _port;

    private volatile int _isDisposed = 0;
    private int _isTcpInitialized = 0;
    private int _isUdpInitialized = 0;

    private UdpClient? _udpClient;

    /// <inheritdoc />
    public BackendInfo Backend { get; }

    /// <inheritdoc />
    public async Task<HealthCheckResult> HealthCheckAsync(
        Mono.Nat.Protocol networkProtocol,
        IReadOnlyDictionary<string, string> parameters,
        CancellationToken cancellationToken = default)
    {
        if (networkProtocol != Mono.Nat.Protocol.Tcp)
        {
            return new(true, null);
        }
        try
        {
            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(IPAddress.Parse(_ip), _port).ConfigureAwait(false);
            return new(true, null);
        }
        catch (Exception ex)
        {
            return new(false, ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<IConnection> CreateBackendConnectionAsync(
        IClientConnection client,
        CancellationToken cancellationToken = default)
    {
        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(IPAddress.Parse(_ip!), _port).ConfigureAwait(false);
        return new Connection(tcpClient);
    }

    /// <inheritdoc />
    public async Task HandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageMetadata,
        byte[] message,
        CancellationToken cancellationToken = default)
    {
        _udpClient ??= new UdpClient();
        await _udpClient.SendAsync(message, message.Length, _ip, _port).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_isDisposed != 0)
        {
            throw new ObjectDisposedException(nameof(PassthroughBackend));
        }
        if (Interlocked.CompareExchange(ref _isTcpInitialized, value: 1, comparand: 0) != 0)
        {
            return Task.CompletedTask;
        }


        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task InitializeAsync(
        IClientWriterFactory clientWriterFactory,
        CancellationToken cancellationToken = default)
    {
        if (_isDisposed != 0)
        {
            throw new ObjectDisposedException(nameof(PassthroughBackend));
        }
        if (Interlocked.CompareExchange(ref _isUdpInitialized, value: 1, comparand: 0) != 0)
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task<bool> CanHandleConnectionAsync(
        IClientConnectionPreview preview,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public virtual Task<bool> CanHandleMessageAsync(
        ClientInfo client,
        Dictionary<string, string> messageMetadata,
        ReadOnlyMemory<byte> message,
        CancellationToken cancellationToken = default)
    {

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public virtual ValueTask DisposeAsync()
    {
        if (Interlocked.CompareExchange(ref _isDisposed, value: 1, comparand: 0) != 0)
        {
            return default;
        }

        _udpClient?.Dispose();

        return default;
    }
}
