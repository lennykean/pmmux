#### [Pmmux\.Core](../../../../index.md 'index')
### [Pmmux\.Core\.Configuration](../index.md 'Pmmux\.Core\.Configuration')

## RouterConfig Class

Configuration for the router including routing strategy and backends\.

```csharp
public record RouterConfig : System.IEquatable<Pmmux.Core.Configuration.RouterConfig>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; RouterConfig

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[RouterConfig](index.md 'Pmmux\.Core\.Configuration\.RouterConfig')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Properties | |
| :--- | :--- |
| [Backends](Backends.md 'Pmmux\.Core\.Configuration\.RouterConfig\.Backends') | List of backend specifications\. |
| [HealthChecks](HealthChecks.md 'Pmmux\.Core\.Configuration\.RouterConfig\.HealthChecks') | List of health check specifications\. |
| [PreviewSizeLimit](PreviewSizeLimit.md 'Pmmux\.Core\.Configuration\.RouterConfig\.PreviewSizeLimit') | Maximum size of the connection preview buffer\. |
| [RoutingStrategy](RoutingStrategy.md 'Pmmux\.Core\.Configuration\.RouterConfig\.RoutingStrategy') | Name of the routing strategy to use\. |
| [SelectionTimeout](SelectionTimeout.md 'Pmmux\.Core\.Configuration\.RouterConfig\.SelectionTimeout') | Timeout for backend selection\. |
