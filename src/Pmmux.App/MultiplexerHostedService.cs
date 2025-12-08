using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;

namespace Pmmux.App;

internal class MultiplexerHostedService(IPortMultiplexer multiplexer) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await multiplexer.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
