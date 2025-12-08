#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[BackendMonitor](index.md 'Pmmux\.Core\.BackendMonitor')

## BackendMonitor\.MonitorAsync\(IHealthCheckBackend, Protocol, CancellationToken, CancellationToken\) Method

Monitor the health of a backend and provide status updates as an async stream\.

```csharp
public System.Collections.Generic.IAsyncEnumerable<Pmmux.Abstractions.BackendStatusInfo> MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend backend, Mono.Nat.Protocol networkProtocol, System.Threading.CancellationToken cancellationToken, System.Threading.CancellationToken enumeratorCancellationToken);
```
#### Parameters

<a name='Pmmux.Core.BackendMonitor.MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend,Mono.Nat.Protocol,System.Threading.CancellationToken,System.Threading.CancellationToken).backend'></a>

`backend` [IHealthCheckBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IHealthCheckBackend/index.md 'Pmmux\.Abstractions\.IHealthCheckBackend')

The backend to monitor\.

<a name='Pmmux.Core.BackendMonitor.MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend,Mono.Nat.Protocol,System.Threading.CancellationToken,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol used for health checks\.

<a name='Pmmux.Core.BackendMonitor.MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend,Mono.Nat.Protocol,System.Threading.CancellationToken,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to stop monitoring this backend\.

<a name='Pmmux.Core.BackendMonitor.MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend,Mono.Nat.Protocol,System.Threading.CancellationToken,System.Threading.CancellationToken).enumeratorCancellationToken'></a>

`enumeratorCancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token for controlling enumeration of the stream\.

Implements [MonitorAsync\(IHealthCheckBackend, Protocol, CancellationToken, CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackendMonitor/MonitorAsync(IHealthCheckBackend,Protocol,CancellationToken,CancellationToken).md 'Pmmux\.Abstractions\.IBackendMonitor\.MonitorAsync\(Pmmux\.Abstractions\.IHealthCheckBackend,Mono\.Nat\.Protocol,System\.Threading\.CancellationToken,System\.Threading\.CancellationToken\)')

#### Returns
[System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[BackendStatusInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendStatusInfo/index.md 'Pmmux\.Abstractions\.BackendStatusInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')