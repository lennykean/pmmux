using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Mono.Nat;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;

namespace Pmmux.Extensions.Management;

internal class PortmapEndpointGroup(IPortWarden portWarden) : IManagementEndpointGroup
{
    private record PortRequest(Protocol NetworkProtocol, int? LocalPort, int? PublicPort);

    public string Name => "port-maps";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("", () => portWarden.GetPortMaps());

        builder.MapPost("", async (
            [FromBody] PortRequest request,
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
                ? Results.Ok(mapping)
                : Results.InternalServerError();
        });

        builder.MapDelete("", async (
            [FromBody] PortRequest request,
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
