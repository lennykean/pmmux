using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Mono.Nat;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;

using IEndpointRouteBuilder = Microsoft.AspNetCore.Routing.IEndpointRouteBuilder;

namespace Pmmux.Extensions.Management;

internal class BackendsEndpointGroup(IRouter router) : IManagementEndpointGroup
{
    public string Name => "backends";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", ([FromQuery] Protocol networkProtocol) => ExecutionContextUtility.SuppressFlow(() =>
        {
            var backends = router.GetBackends(networkProtocol).Select(info => info.ToDto()).ToList();

            return Results.Ok(backends);
        }));

        builder.MapPost("{protocolName}/{name}", (
            string protocolName,
            string name,
            [FromQuery] Protocol networkProtocol,
            [FromBody] Dictionary<string, string> parameters,
            CancellationToken cancellationToken) => ExecutionContextUtility.SuppressFlow(async () =>
        {
            var spec = new BackendSpec(name, protocolName, parameters);

            var backendInfo = await router.AddBackendAsync(
                networkProtocol,
                spec,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            return Results.Ok(backendInfo.ToDto());
        }));

        builder.MapDelete("{protocolName}/{name}", (
            string protocolName,
            string name,
            [FromQuery] Protocol networkProtocol,
            CancellationToken cancellationToken) => ExecutionContextUtility.SuppressFlow(async () =>
        {
            var existingBackend = router
                .GetBackends(networkProtocol)
                .OrderBy(backend => backend.Status)
                .FirstOrDefault(backend =>
                    backend.Backend.Spec.Name == name &&
                    backend.Backend.Spec.ProtocolName == protocolName);

            if (existingBackend is null)
            {
                return Results.NotFound();
            }

            var removed = await router.RemoveBackendAsync(
                networkProtocol,
                existingBackend.Backend,
                cancellationToken: cancellationToken);

            return Results.Ok(removed);
        }));

        builder.MapPut("{protocolName}/{name}", (
            string protocolName,
            string name,
            [FromQuery] Protocol networkProtocol,
            [FromBody] Dictionary<string, string> parameters,
            CancellationToken cancellationToken) => ExecutionContextUtility.SuppressFlow(async () =>
        {
            var existingBackend = router
                .GetBackends(networkProtocol)
                .OrderBy(backend => backend.Status)
                .FirstOrDefault(backend =>
                    backend.Backend.Spec.Name == name &&
                    backend.Backend.Spec.ProtocolName == protocolName);

            if (existingBackend is null)
            {
                return Results.NotFound();
            }

            var spec = new BackendSpec(name, protocolName, parameters);
            var backend = await router.ReplaceBackendAsync(
                networkProtocol,
                existingBackend.Backend,
                spec,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            return Results.Ok(backend?.ToDto());
        }));
    }
}
