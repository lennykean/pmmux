using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Mono.Nat;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Management.Dtos;

using IEndpointRouteBuilder = Microsoft.AspNetCore.Routing.IEndpointRouteBuilder;

namespace Pmmux.Extensions.Management;

internal class BackendsEndpointGroup(IRouter router) : IManagementEndpointGroup
{
    public string Name => "backends";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", ([FromQuery] Protocol networkProtocol) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return router.GetBackends(networkProtocol).Select(BackendInfoDto.FromBackendStatusInfo).ToList();
            }
        });
        builder.MapPost("{protocolName}/{name}", async (
            string protocolName,
            string name,
            [FromQuery] Protocol networkProtocol,
            [FromBody] Dictionary<string, string> parameters,
            CancellationToken cancellationToken) =>
        {
            Task<BackendInfo> task;

            using (ExecutionContext.SuppressFlow())
            {
                task = Task.Run(async () =>
                {
                    var spec = new BackendSpec(name, protocolName, parameters);

                    return await router.AddBackendAsync(
                        networkProtocol,
                        spec,
                        cancellationToken: cancellationToken).ConfigureAwait(false);
                });
            }
            return Results.Ok(BackendInfoDto.FromBackendInfo(await task.ConfigureAwait(false)));
        });
        builder.MapDelete("{protocolName}/{name}", async (
            string protocolName,
            string name,
            [FromQuery] Protocol networkProtocol,
            CancellationToken cancellationToken) =>
        {
            Task<bool> task;

            using (ExecutionContext.SuppressFlow())
            {
                task = Task.Run(async () =>
                {
                    var existingBackend = router
                        .GetBackends(networkProtocol)
                        .OrderBy(backend => backend.Status)
                        .FirstOrDefault(backend =>
                            backend.Backend.Spec.Name == name &&
                            backend.Backend.Spec.ProtocolName == protocolName);

                    if (existingBackend is null)
                    {
                        return false;
                    }

                    return await router.RemoveBackendAsync(
                        networkProtocol,
                        existingBackend.Backend,
                        cancellationToken: cancellationToken);
                });
            }
            return await task.ConfigureAwait(false)
                ? Results.Ok()
                : Results.NotFound();
        });
        builder.MapPut("{protocolName}/{name}", async (
            string protocolName,
            string name,
            [FromQuery] Protocol networkProtocol,
            [FromBody] Dictionary<string, string> parameters,
            CancellationToken cancellationToken) =>
        {
            Task<BackendInfo?> task;

            using (ExecutionContext.SuppressFlow())
            {
                task = Task.Run(async () =>
                {
                    var existingBackend = router
                        .GetBackends(networkProtocol)
                        .OrderBy(backend => backend.Status)
                        .FirstOrDefault(backend =>
                            backend.Backend.Spec.Name == name &&
                            backend.Backend.Spec.ProtocolName == protocolName);

                    if (existingBackend is null)
                    {
                        return null;
                    }

                    var spec = new BackendSpec(name, protocolName, parameters);

                    return await router.ReplaceBackendAsync(
                        networkProtocol,
                        existingBackend.Backend,
                        spec,
                        cancellationToken: cancellationToken).ConfigureAwait(false);
                });
            }
            return await task.ConfigureAwait(false) is { } backendInfo
                ? Results.Ok(BackendInfoDto.FromBackendInfo(backendInfo))
                : Results.NotFound();
        });
    }
}
