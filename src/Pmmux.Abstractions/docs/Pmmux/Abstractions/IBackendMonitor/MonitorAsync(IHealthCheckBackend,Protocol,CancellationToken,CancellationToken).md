#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IBackendMonitor](index.md 'Pmmux\.Abstractions\.IBackendMonitor')

## IBackendMonitor\.MonitorAsync\(IHealthCheckBackend, Protocol, CancellationToken, CancellationToken\) Method

Monitor the health of a backend and provide status updates as an async stream\.

```csharp
System.Collections.Generic.IAsyncEnumerable<Pmmux.Abstractions.BackendStatusInfo> MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend backend, Mono.Nat.Protocol networkProtocol, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken), System.Threading.CancellationToken enumeratorCancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IBackendMonitor.MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend,Mono.Nat.Protocol,System.Threading.CancellationToken,System.Threading.CancellationToken).backend'></a>

`backend` [IHealthCheckBackend](../IHealthCheckBackend/index.md 'Pmmux\.Abstractions\.IHealthCheckBackend')

The backend to monitor\.

<a name='Pmmux.Abstractions.IBackendMonitor.MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend,Mono.Nat.Protocol,System.Threading.CancellationToken,System.Threading.CancellationToken).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol used for health checks\.

<a name='Pmmux.Abstractions.IBackendMonitor.MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend,Mono.Nat.Protocol,System.Threading.CancellationToken,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to stop monitoring this backend\.

<a name='Pmmux.Abstractions.IBackendMonitor.MonitorAsync(Pmmux.Abstractions.IHealthCheckBackend,Mono.Nat.Protocol,System.Threading.CancellationToken,System.Threading.CancellationToken).enumeratorCancellationToken'></a>

`enumeratorCancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token for controlling enumeration of the stream\.

#### Returns
[System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[BackendStatusInfo](../BackendStatusInfo/index.md 'Pmmux\.Abstractions\.BackendStatusInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')