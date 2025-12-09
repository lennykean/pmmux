using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Management.Dtos;

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
        builder.MapPost("/", ([FromBody] HealthCheckRequestDto request) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return backendMonitor.TryAddHealthCheck(request.ToHealthCheckSpec())
                    ? Results.Ok()
                    : Results.Conflict();
            }
        });
        builder.MapDelete("/", ([FromBody] HealthCheckRequestDto request) =>
        {
            using (ExecutionContext.SuppressFlow())
            {
                return backendMonitor.TryRemoveHealthCheck(request.ToHealthCheckSpec())
                    ? Results.Ok()
                    : Results.NotFound();
            }
        });
    }
}
