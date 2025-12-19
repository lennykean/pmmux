using System;
using System.Linq;
using System.Net;
using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Management.Models;

namespace Pmmux.Extensions.Management;

internal class ListenersEndpointGroup(IPortMultiplexer portMultiplexer) : IManagementEndpointGroup
{
    public string Name => "listeners";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("", () => ExecutionContextUtility.SuppressFlow(async () =>
        {
            var listeners = await portMultiplexer.GetListenersAsync().ConfigureAwait(false);

            return Results.Ok(listeners.Select(l => l.ToDto()).ToArray());
        }));

        builder.MapPost("", ([FromBody] ListenerRequest request) => ExecutionContextUtility.SuppressFlow(() =>
        {
            var protocol = request.NetworkProtocol;
            var port = request.Port;
            var bindAddress = IPAddress.Any;

            if (request.BindAddress is not null &&
                !IPAddress.TryParse(request.BindAddress, out bindAddress))
            {
                return Results.BadRequest($"Invalid bind address: \"{request.BindAddress}\"");
            }

            try
            {
                if (portMultiplexer.AddListener(protocol, port, bindAddress) is not { } listener)
                {
                    return Results.BadRequest("Failed to create listener");
                }
                return Results.Ok(listener.ToDto());
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        }));

        builder.MapDelete("", (
            [FromBody] ListenerRequest request,
            CancellationToken cancellationToken) => ExecutionContextUtility.SuppressFlow(async () =>
        {
            try
            {
                if (await portMultiplexer.RemoveListenerAsync(
                    request.NetworkProtocol,
                    request.Port,
                    cancellationToken).ConfigureAwait(false))
                {
                    return Results.Ok();
                }
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(ex.Message);
            }
        }));
    }
}
