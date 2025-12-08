#### [Pmmux\.Extensions\.Management\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Abstractions](../index.md 'Pmmux\.Extensions\.Management\.Abstractions').[IManagementEndpointGroup](index.md 'Pmmux\.Extensions\.Management\.Abstractions\.IManagementEndpointGroup')

## IManagementEndpointGroup\.Name Property

The endpoint group name used as the route path prefix\.

```csharp
string Name { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Remarks
The name should be lowercase and use hyphens for multi\-word names \(e\.g\., "health\-checks"\)\.
Endpoints mapped in [MapEndpoints\(IEndpointRouteBuilder\)](MapEndpoints(IEndpointRouteBuilder).md 'Pmmux\.Extensions\.Management\.Abstractions\.IManagementEndpointGroup\.MapEndpoints\(Microsoft\.AspNetCore\.Routing\.IEndpointRouteBuilder\)') will be prefixed with `/api/[Name]`\.