using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;

namespace Pmmux.Extensions.Management;

internal class HealthCheckEndpointGroup(IBackendMonitor backendMonitor) : IManagementEndpointGroup
{
    public string Name => "health-checks";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", () =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return backendMonitor.GetHealthChecks();
            }
        });
        builder.MapPost("/", async ([FromBody] HealthCheckSpec spec) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return backendMonitor.TryAddHealthCheck(spec)
                    ? Results.Ok()
                    : Results.Conflict();
            }
        });
        builder.MapDelete("/", ([FromBody] HealthCheckSpec spec) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return backendMonitor.TryRemoveHealthCheck(spec)
                    ? Results.Ok()
                    : Results.NotFound();
            }
        });
    }
}
