using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Management.Dtos;

namespace Pmmux.Extensions.Management;

internal class PortmapEndpointGroup(IPortWarden portWarden) : IManagementEndpointGroup
{
    public string Name => "port-maps";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("", () =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return portWarden.GetPortMaps().Select(PortMapDto.FromPortMapInfo).ToList();
            }
        });

        builder.MapGet("/device", () =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return portWarden.NatDevice is { } device
                    ? Results.Ok(NatDeviceDto.FromNatDeviceInfo(device))
                    : Results.NotFound();
            }
        });

        builder.MapPost("", async (
            [FromBody] PortRequestDto request,
            CancellationToken cancellationToken) =>
        {
            Task<PortMapInfo?> task;

            using (ExecutionContext.SuppressFlow())
            {
                task = Task.Run(async () =>
                {
                    return await portWarden.AddPortMapAsync(
                        request.NetworkProtocol,
                        request.LocalPort,
                        request.PublicPort,
                        cancellationToken);
                });
            }
            return await task.ConfigureAwait(false) is { } mapping
                ? Results.Ok(PortMapDto.FromPortMapInfo(mapping))
                : Results.InternalServerError();
        });

        builder.MapDelete("", async (
            [FromBody] PortRequestDto request,
            CancellationToken cancellationToken) =>
        {
            if (request.LocalPort is null || request.PublicPort is null)
            {
                return Results.BadRequest();
            }
            return Results.Ok(await portWarden.RemovePortMapAsync(
                request.NetworkProtocol,
                request.LocalPort.Value,
                request.PublicPort.Value,
                cancellationToken));
        });
    }
}
