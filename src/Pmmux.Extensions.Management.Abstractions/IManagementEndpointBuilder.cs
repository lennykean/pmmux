using Microsoft.AspNetCore.Routing;

namespace Pmmux.Extensions.Management.Abstractions;

/// <summary>
/// Defines a group of management API endpoints that are mounted under a common path prefix.
/// </summary>
/// <remarks>
/// Endpoint groups organize related API endpoints and are automatically discovered and
/// registered by the management extension. Each group is mounted at <c>/api/[Name]</c>.
/// </remarks>
public interface IManagementEndpointGroup
{
    /// <summary>
    /// The endpoint group name used as the route path prefix.
    /// </summary>
    /// <remarks>
    /// The name should be lowercase and use hyphens for multi-word names (e.g., "health-checks").
    /// Endpoints mapped in <see cref="MapEndpoints"/> will be prefixed with <c>/api/[Name]</c>.
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Maps the group's HTTP endpoints to the route builder.
    /// </summary>
    /// <param name="builder">
    /// Route group builder scoped to this group's path prefix (<c>/api/[Name]</c>).
    /// </param>
    /// <remarks>
    /// Routes are relative to the group's path prefix. A route of <c>"/"</c> maps to <c>/api/[Name]</c>,
    /// while <c>"/{id}"</c> maps to <c>/api/[Name]/{id}</c>.
    /// </remarks>
    void MapEndpoints(IEndpointRouteBuilder builder);
}
