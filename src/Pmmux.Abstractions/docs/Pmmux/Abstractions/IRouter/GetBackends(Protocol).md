#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IRouter](index.md 'Pmmux\.Abstractions\.IRouter')

## IRouter\.GetBackends\(Protocol\) Method

Get all backends for the network protocol with their health status\.

```csharp
System.Collections.Generic.IEnumerable<Pmmux.Abstractions.BackendStatusInfo> GetBackends(Mono.Nat.Protocol networkProtocol);
```
#### Parameters

<a name='Pmmux.Abstractions.IRouter.GetBackends(Mono.Nat.Protocol).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol to filter backends by\.

#### Returns
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[BackendStatusInfo](../BackendStatusInfo/index.md 'Pmmux\.Abstractions\.BackendStatusInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')  
Backend status information\.