#### [Pmmux\.Extensions\.Management\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Abstractions](../index.md 'Pmmux\.Extensions\.Management\.Abstractions').[IManagementEndpointGroup](index.md 'Pmmux\.Extensions\.Management\.Abstractions\.IManagementEndpointGroup')

## IManagementEndpointGroup\.MapEndpoints\(IEndpointRouteBuilder\) Method

Maps the group's HTTP endpoints to the route builder\.

```csharp
void MapEndpoints(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder builder);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Abstractions.IManagementEndpointGroup.MapEndpoints(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder).builder'></a>

`builder` [Microsoft\.AspNetCore\.Routing\.IEndpointRouteBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.routing.iendpointroutebuilder 'Microsoft\.AspNetCore\.Routing\.IEndpointRouteBuilder')

Route group builder scoped to this group's path prefix \(`/api/[Name]`\)\.

### Remarks
Routes are relative to the group's path prefix\. A route of `"/"` maps to `/api/[Name]`,
while `"/{id}"` maps to `/api/[Name]/{id}`\.