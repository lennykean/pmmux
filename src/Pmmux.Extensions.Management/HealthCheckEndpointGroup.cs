using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Management.Models;

namespace Pmmux.Extensions.Management;

internal class HealthCheckEndpointGroup(IBackendMonitor backendMonitor) : IManagementEndpointGroup
{
    public string Name => "health-checks";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", () => ExecutionContextUtility.SuppressFlow(() =>
        {
            return Results.Ok(backendMonitor.GetHealthChecks().Select(x => x.ToDto()));
        }));

        builder.MapPost("/", ([FromBody] HealthCheckSpecDto request) => ExecutionContextUtility.SuppressFlow(() =>
        {
            var spec = request.ToHealthCheckSpec();
            if (!backendMonitor.TryAddHealthCheck(spec))
            {
                return Results.Conflict();
            }
            return Results.Ok();
        }));

        builder.MapDelete("/", ([FromBody] HealthCheckSpecDto request) => ExecutionContextUtility.SuppressFlow(() =>
        {
            var spec = request.ToHealthCheckSpec();
            if (!backendMonitor.TryRemoveHealthCheck(spec))
            {
                return Results.NotFound();
            }
            return Results.Ok();
        }));
    }
}
