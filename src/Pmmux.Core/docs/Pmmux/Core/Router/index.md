#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## Router Class

Default implementation of [IRouter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/index.md 'Pmmux\.Abstractions\.IRouter')\.

```csharp
public sealed class Router : Pmmux.Abstractions.IRouter, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; Router

Implements [IRouter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/index.md 'Pmmux\.Abstractions\.IRouter'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Constructors | |
| :--- | :--- |
| [Router\(IEnumerable&lt;IClientConnectionNegotiator&gt;, IEnumerable&lt;IRoutingStrategy&gt;, IEnumerable&lt;IBackendProtocol&gt;, IEventSender, IBackendMonitor, IMetricReporter, RouterConfig, ILoggerFactory\)](Router(IEnumerable_IClientConnectionNegotiator_,IEnumerable_IRoutingStrategy_,IEnumerable_IBackendProtocol_,IEventSender,IBackendMonitor,IMetricReporter,RouterConfig,ILoggerFactory).md 'Pmmux\.Core\.Router\.Router\(System\.Collections\.Generic\.IEnumerable\<Pmmux\.Abstractions\.IClientConnectionNegotiator\>, System\.Collections\.Generic\.IEnumerable\<Pmmux\.Abstractions\.IRoutingStrategy\>, System\.Collections\.Generic\.IEnumerable\<Pmmux\.Abstractions\.IBackendProtocol\>, Pmmux\.Abstractions\.IEventSender, Pmmux\.Abstractions\.IBackendMonitor, Pmmux\.Abstractions\.IMetricReporter, Pmmux\.Core\.Configuration\.RouterConfig, Microsoft\.Extensions\.Logging\.ILoggerFactory\)') | Default implementation of [IRouter](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/index.md 'Pmmux\.Abstractions\.IRouter')\. |

| Methods | |
| :--- | :--- |
| [AddBackendAsync\(Protocol, BackendSpec, CancellationToken\)](AddBackendAsync(Protocol,BackendSpec,CancellationToken).md 'Pmmux\.Core\.Router\.AddBackendAsync\(Mono\.Nat\.Protocol, Pmmux\.Abstractions\.BackendSpec, System\.Threading\.CancellationToken\)') | Add a backend dynamically at runtime\. |
| [GetBackends\(Protocol\)](GetBackends(Protocol).md 'Pmmux\.Core\.Router\.GetBackends\(Mono\.Nat\.Protocol\)') | Get all backends for the network protocol with their health status\. |
| [InitializeAsync\(IClientWriterFactory, CancellationToken\)](InitializeAsync(IClientWriterFactory,CancellationToken).md 'Pmmux\.Core\.Router\.InitializeAsync\(Pmmux\.Abstractions\.IClientWriterFactory, System\.Threading\.CancellationToken\)') | Initialize the router with listener information at startup\. |
| [RemoveBackendAsync\(Protocol, BackendInfo, bool, CancellationToken\)](RemoveBackendAsync(Protocol,BackendInfo,bool,CancellationToken).md 'Pmmux\.Core\.Router\.RemoveBackendAsync\(Mono\.Nat\.Protocol, Pmmux\.Abstractions\.BackendInfo, bool, System\.Threading\.CancellationToken\)') | Remove a backend dynamically at runtime\. |
| [ReplaceBackendAsync\(Protocol, BackendInfo, BackendSpec, CancellationToken\)](ReplaceBackendAsync(Protocol,BackendInfo,BackendSpec,CancellationToken).md 'Pmmux\.Core\.Router\.ReplaceBackendAsync\(Mono\.Nat\.Protocol, Pmmux\.Abstractions\.BackendInfo, Pmmux\.Abstractions\.BackendSpec, System\.Threading\.CancellationToken\)') | Replace an existing backend with a new backend atomically\. |
| [RouteConnectionAsync\(Socket, ClientInfo, CancellationToken\)](RouteConnectionAsync(Socket,ClientInfo,CancellationToken).md 'Pmmux\.Core\.Router\.RouteConnectionAsync\(System\.Net\.Sockets\.Socket, Pmmux\.Abstractions\.ClientInfo, System\.Threading\.CancellationToken\)') | Route a client connection to a backend\. |
| [RouteMessageAsync\(byte\[\], ClientInfo, CancellationToken\)](RouteMessageAsync(byte[],ClientInfo,CancellationToken).md 'Pmmux\.Core\.Router\.RouteMessageAsync\(byte\[\], Pmmux\.Abstractions\.ClientInfo, System\.Threading\.CancellationToken\)') | Route a client message to a backend\. |
