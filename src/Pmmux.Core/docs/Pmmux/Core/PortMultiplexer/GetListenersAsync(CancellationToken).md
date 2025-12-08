#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PortMultiplexer](index.md 'Pmmux\.Core\.PortMultiplexer')

## PortMultiplexer\.GetListenersAsync\(CancellationToken\) Method

Get the currently bound listener endpoints\.

```csharp
public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Pmmux.Abstractions.ListenerInfo>> GetListenersAsync(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.PortMultiplexer.GetListenersAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [GetListenersAsync\(CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortMultiplexer/GetListenersAsync(CancellationToken).md 'Pmmux\.Abstractions\.IPortMultiplexer\.GetListenersAsync\(System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[ListenerInfo](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
List of bound listener endpoints\.