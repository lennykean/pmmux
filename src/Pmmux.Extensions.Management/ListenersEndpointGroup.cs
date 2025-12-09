using System.Linq;
using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Management.Dtos;

namespace Pmmux.Extensions.Management;

internal class ListenersEndpointGroup(IPortMultiplexer portMultiplexer) : IManagementEndpointGroup
{
    public string Name => "listeners";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("", async () =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                var listeners = await portMultiplexer.GetListenersAsync().ConfigureAwait(false);
                return Results.Ok(listeners.Select(ListenerDto.FromListenerInfo).ToList());
            }
        });

        builder.MapPost("", ([FromBody] ListenerRequestDto request) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                var listener = portMultiplexer.AddListener(request.NetworkProtocol, request.Port);
                return listener is not null
                    ? Results.Ok(ListenerDto.FromListenerInfo(listener))
                    : Results.Conflict();
            }
        });

        builder.MapDelete("", async (
            [FromBody] ListenerRequestDto request,
            CancellationToken cancellationToken) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                var removed = await portMultiplexer.RemoveListenerAsync(
                    request.NetworkProtocol,
                    request.Port,
                    cancellationToken).ConfigureAwait(false);
                return removed ? Results.Ok() : Results.NotFound();
            }
        });
    }
}

