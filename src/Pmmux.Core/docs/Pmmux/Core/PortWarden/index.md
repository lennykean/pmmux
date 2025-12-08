#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## PortWarden Class

Default implementation of [IPortWarden](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortWarden/index.md 'Pmmux\.Abstractions\.IPortWarden')\.

```csharp
public sealed class PortWarden : Pmmux.Abstractions.IPortWarden, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PortWarden

Implements [IPortWarden](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortWarden/index.md 'Pmmux\.Abstractions\.IPortWarden'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Constructors | |
| :--- | :--- |
| [PortWarden\(IEventSender, IMetricReporter, PortWardenConfig, ILoggerFactory\)](PortWarden(IEventSender,IMetricReporter,PortWardenConfig,ILoggerFactory).md 'Pmmux\.Core\.PortWarden\.PortWarden\(Pmmux\.Abstractions\.IEventSender, Pmmux\.Abstractions\.IMetricReporter, Pmmux\.Core\.Configuration\.PortWardenConfig, Microsoft\.Extensions\.Logging\.ILoggerFactory\)') | Default implementation of [IPortWarden](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortWarden/index.md 'Pmmux\.Abstractions\.IPortWarden')\. |

| Properties | |
| :--- | :--- |
| [NatDevice](NatDevice.md 'Pmmux\.Core\.PortWarden\.NatDevice') | Discovered NAT device, if any\. |

| Methods | |
| :--- | :--- |
| [AddPortMapAsync\(Protocol, Nullable&lt;int&gt;, Nullable&lt;int&gt;, CancellationToken\)](AddPortMapAsync(Protocol,Nullable_int_,Nullable_int_,CancellationToken).md 'Pmmux\.Core\.PortWarden\.AddPortMapAsync\(Mono\.Nat\.Protocol, System\.Nullable\<int\>, System\.Nullable\<int\>, System\.Threading\.CancellationToken\)') | Add a new port mapping\. |
| [GetPortMaps\(\)](GetPortMaps().md 'Pmmux\.Core\.PortWarden\.GetPortMaps\(\)') | Get the current port mappings\. |
| [RemovePortMapAsync\(Protocol, int, int, CancellationToken\)](RemovePortMapAsync(Protocol,int,int,CancellationToken).md 'Pmmux\.Core\.PortWarden\.RemovePortMapAsync\(Mono\.Nat\.Protocol, int, int, System\.Threading\.CancellationToken\)') | Remove an existing port mapping\. |
| [StartAsync\(CancellationToken\)](StartAsync(CancellationToken).md 'Pmmux\.Core\.PortWarden\.StartAsync\(System\.Threading\.CancellationToken\)') | Discover NAT devices and create port mappings from the initial configuration\. |
