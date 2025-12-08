# Management Extension Programming Guide

This guide covers extending the management API with custom endpoint groups.

## Custom Endpoint Groups

Extensions can register their own API endpoints by implementing `IManagementEndpointGroup` from the `Pmmux.Extensions.Management.Abstractions` package.

### Basic Implementation

```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Pmmux.Extensions.Management.Abstractions;

public class MyEndpointGroup : IManagementEndpointGroup
{
    public string Name => "my-feature";  // Mounted at /api/my-feature/

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetStatus);
        builder.MapGet("/{id}", GetById);
        builder.MapPost("/", CreateItem);
        builder.MapPut("/{id}", UpdateItem);
        builder.MapDelete("/{id}", DeleteItem);
    }

    private IResult GetStatus()
    {
        return Results.Ok(new { Status = "OK", Version = "1.0" });
    }

    private IResult GetById(string id)
    {
        return Results.Ok(new { Id = id });
    }

    private async Task<IResult> CreateItem(ItemRequest request)
    {
        // Process request
        return Results.Created($"/api/my-feature/{request.Id}", request);
    }

    private async Task<IResult> UpdateItem(string id, ItemRequest request)
    {
        // Update logic
        return Results.Ok(request);
    }

    private IResult DeleteItem(string id)
    {
        // Delete logic
        return Results.NoContent();
    }
}

public record ItemRequest(string Id, string Name);
```

### Dependency Injection

Endpoint groups have full access to dependency injection:

```csharp
public class MetricsEndpointGroup : IManagementEndpointGroup
{
    private readonly IMetricSink _metricSink;
    private readonly ILogger<MetricsEndpointGroup> _logger;

    public MetricsEndpointGroup(IMetricSink metricSink, ILogger<MetricsEndpointGroup> logger)
    {
        _metricSink = metricSink;
        _logger = logger;
    }

    public string Name => "metrics";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetMetrics);
    }

    private async Task<IResult> GetMetrics()
    {
        _logger.LogInformation("Metrics requested");
        // Return metric data
        return Results.Ok(new { RequestCount = 1000 });
    }
}
```

### Registering Endpoint Groups

Register the endpoint group in your extension's `RegisterServices` method:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;

public class MyExtension : IExtension
{
    public void RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddSingleton<IManagementEndpointGroup, MyEndpointGroup>();
        services.AddSingleton<IManagementEndpointGroup, MetricsEndpointGroup>();
    }

    public void RegisterCommandOptions(ICommandLineBuilder builder)
    {
        // Register any command line options if needed
    }
}
```

### Route Patterns

Routes are relative to the group's path prefix (`/api/{Name}`):

- `"/"` maps to `/api/my-feature`
- `"/{id}"` maps to `/api/my-feature/{id}`
- `"/items/{id}/details"` maps to `/api/my-feature/items/{id}/details`

### Example: State Management Endpoints

```csharp
using Pmmux.Abstractions;

public class StateEndpointGroup : IManagementEndpointGroup
{
    private readonly IStateManager _stateManager;

    public StateEndpointGroup(IStateManager stateManager)
    {
        _stateManager = stateManager;
    }

    public string Name => "state";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/backends", GetBackends);
        builder.MapGet("/backends/{name}", GetBackend);
    }

    private async Task<IResult> GetBackends()
    {
        var backends = await _stateManager.GetBackendsAsync();
        return Results.Ok(backends);
    }

    private async Task<IResult> GetBackend(string name)
    {
        var backend = await _stateManager.GetBackendAsync(name);
        if (backend == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(backend);
    }
}
```

## API Reference

See the API documentation for detailed interface definitions:

- [IManagementEndpointGroup](../Pmmux.Extensions.Management.Abstractions/docs/Pmmux/Extensions/Management/Abstractions/IManagementEndpointGroup/index.md) - Main interface for creating endpoint groups
