using System.Linq;
using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Management.Models;

namespace Pmmux.Extensions.Management;

internal class PortmapEndpointGroup(IPortWarden portWarden) : IManagementEndpointGroup
{
    public string Name => "port-maps";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("", () => ExecutionContextUtility.SuppressFlow(() =>
        {
            return Results.Ok(portWarden.GetPortMaps().Select(m => m.ToDto()).ToList());
        }));

        builder.MapGet("/device", () => ExecutionContextUtility.SuppressFlow(() =>
        {
            if (portWarden.NatDevice is { } device)
            {
                return Results.Ok(device.ToDto());
            }
            return Results.NotFound();
        }));

        builder.MapPost("", (
            [FromBody] PortMapRequest request,
            CancellationToken cancellationToken) => ExecutionContextUtility.SuppressFlow(async () =>
        {
            if (await portWarden.AddPortMapAsync(
                request.NetworkProtocol,
                request.LocalPort,
                request.PublicPort,
                cancellationToken).ConfigureAwait(false) is not { } mapping)
            {
                return Results.InternalServerError();
            }
            return Results.Ok(mapping.ToDto());
        }));

        builder.MapDelete("", (
            [FromBody] PortMapRequest request,
            CancellationToken cancellationToken) => ExecutionContextUtility.SuppressFlow(async () =>
        {
            if (request.LocalPort is null || request.PublicPort is null)
            {
                return Results.BadRequest();
            }
            return Results.Ok(await portWarden.RemovePortMapAsync(
                request.NetworkProtocol,
                request.LocalPort.Value,
                request.PublicPort.Value,
                cancellationToken).ConfigureAwait(false));
        }));
    }
}
