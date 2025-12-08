#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[Router](index.md 'Pmmux\.Core\.Router')

## Router\.GetBackends\(Protocol\) Method

Get all backends for the network protocol with their health status\.

```csharp
public System.Collections.Generic.IEnumerable<Pmmux.Abstractions.BackendStatusInfo> GetBackends(Mono.Nat.Protocol networkProtocol);
```
#### Parameters

<a name='Pmmux.Core.Router.GetBackends(Mono.Nat.Protocol).networkProtocol'></a>

`networkProtocol` [Mono\.Nat\.Protocol](https://learn.microsoft.com/en-us/dotnet/api/mono.nat.protocol 'Mono\.Nat\.Protocol')

The network protocol to filter backends by\.

Implements [GetBackends\(Protocol\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IRouter/GetBackends(Protocol).md 'Pmmux\.Abstractions\.IRouter\.GetBackends\(Mono\.Nat\.Protocol\)')

#### Returns
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[BackendStatusInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/BackendStatusInfo/index.md 'Pmmux\.Abstractions\.BackendStatusInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')  
Backend status information\.