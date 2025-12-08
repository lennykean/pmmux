using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;

namespace Pmmux.Core;

internal class BackendConnectionClientBroker(
    IConnection backendConnection,
    IClientConnection clientConnection,
    ILoggerFactory loggerFactory) : IAsyncDisposable
{
    private enum State
    {
        Initial = 0,
        Starting = 1,
        Started = 2,
        Disposed = 3
    }

    private readonly TaskCompletionSource<bool> _disposedTsc = new();
    private readonly CancellationTokenSource _workerCts = new();
    private readonly ILogger _logger = loggerFactory.CreateLogger("client-connection-broker");
    private readonly StateManager<State> _state = new(State.Initial);

    private Task _ingressTask = Task.CompletedTask;
    private Task _egressTask = Task.CompletedTask;

    public event EventHandler? ClientConnectionClosed;
    public event EventHandler? BackendConnectionClosed;

    public void Start()
    {
        if (_state.Is(State.Disposed))
        {
            throw new ObjectDisposedException(nameof(BackendConnectionClientBroker));
        }
        if (!_state.TryTransition(to: State.Starting, from: State.Initial))
        {
            throw new InvalidOperationException();
        }

        try
        {
            _ingressTask = IngressAsync();
            _logger.LogTrace("ingress pump started");

            _egressTask = EgressAsync();
            _logger.LogTrace("egress pump started");

            _state.TryTransition(to: State.Started, from: State.Starting);
        }
        catch (Exception ex)
        {
            _state.TryTransition(to: State.Initial, from: State.Starting);

            _logger.LogDebug(ex, "error starting");

            throw;
        }
    }

    public async Task WaitAsync(CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<bool>();

        using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken)))
        {
            await Task.WhenAny(tcs.Task, _disposedTsc.Task).ConfigureAwait(false);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_state.Is(State.Disposed) ||
            !_state.TryTransition(to: State.Disposed, from: [State.Started, State.Starting, State.Initial]))
        {
            await _disposedTsc.Task.ConfigureAwait(false);
            return;
        }

        try
        {
            _workerCts.Cancel();

            await Task.WhenAll(_ingressTask, _egressTask).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "error disposing");

            _disposedTsc.SetException(ex);

            throw;
        }
        finally
        {
            _workerCts.Dispose();
            ClientConnectionClosed = null;
            BackendConnectionClosed = null;

            _logger.LogTrace("client connection broker disposed");

            _disposedTsc.SetResult(true);
        }
    }

    private async Task IngressAsync()
    {
        try
        {
            var reader = clientConnection.GetReader();
            var writer = backendConnection.GetWriter();

            await reader.CopyToAsync(writer, _workerCts.Token).ConfigureAwait(false);

            _logger.LogTrace("ingress completed");
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "ingress error");
        }
        finally
        {
            try
            {
                await clientConnection.CloseAsync().ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex, "error closing client connection");
            }

            ClientConnectionClosed?.Invoke(this, EventArgs.Empty);
        }
    }

    private async Task EgressAsync()
    {
        try
        {
            var reader = backendConnection.GetReader();
            var writer = clientConnection.GetWriter();

            await reader.CopyToAsync(writer, _workerCts.Token).ConfigureAwait(false);

            _logger.LogTrace("egress completed");
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "egress error");
        }
        finally
        {
            try
            {
                await backendConnection.CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex, "error closing backend connection");
            }

            BackendConnectionClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}
