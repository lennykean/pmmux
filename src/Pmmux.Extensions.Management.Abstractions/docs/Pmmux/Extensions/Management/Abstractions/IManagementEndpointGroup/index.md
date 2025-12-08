#### [Pmmux\.Extensions\.Management\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Abstractions](../index.md 'Pmmux\.Extensions\.Management\.Abstractions')

## IManagementEndpointGroup Interface

Defines a group of management API endpoints that are mounted under a common path prefix\.

```csharp
public interface IManagementEndpointGroup
```

### Remarks
Endpoint groups organize related API endpoints and are automatically discovered and
registered by the management extension\. Each group is mounted at `/api/[Name]`\.

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Extensions\.Management\.Abstractions\.IManagementEndpointGroup\.Name') | The endpoint group name used as the route path prefix\. |

| Methods | |
| :--- | :--- |
| [MapEndpoints\(IEndpointRouteBuilder\)](MapEndpoints(IEndpointRouteBuilder).md 'Pmmux\.Extensions\.Management\.Abstractions\.IManagementEndpointGroup\.MapEndpoints\(Microsoft\.AspNetCore\.Routing\.IEndpointRouteBuilder\)') | Maps the group's HTTP endpoints to the route builder\. |
