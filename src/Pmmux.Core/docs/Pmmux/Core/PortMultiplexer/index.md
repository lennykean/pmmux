#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## PortMultiplexer Class

Default implementation of [IPortMultiplexer](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortMultiplexer/index.md 'Pmmux\.Abstractions\.IPortMultiplexer')\.

```csharp
public sealed class PortMultiplexer : Pmmux.Abstractions.IPortMultiplexer, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PortMultiplexer

Implements [IPortMultiplexer](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortMultiplexer/index.md 'Pmmux\.Abstractions\.IPortMultiplexer'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Constructors | |
| :--- | :--- |
| [PortMultiplexer\(IPortWarden, IRouter, ListenerConfig, ILoggerFactory\)](PortMultiplexer(IPortWarden,IRouter,ListenerConfig,ILoggerFactory).md 'Pmmux\.Core\.PortMultiplexer\.PortMultiplexer\(Pmmux\.Abstractions\.IPortWarden, Pmmux\.Abstractions\.IRouter, Pmmux\.Core\.Configuration\.ListenerConfig, Microsoft\.Extensions\.Logging\.ILoggerFactory\)') | Default implementation of [IPortMultiplexer](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortMultiplexer/index.md 'Pmmux\.Abstractions\.IPortMultiplexer')\. |

| Methods | |
| :--- | :--- |
| [AddListener\(Protocol, int\)](AddListener(Protocol,int).md 'Pmmux\.Core\.PortMultiplexer\.AddListener\(Mono\.Nat\.Protocol, int\)') | Add a new listener on the specified port\. |
| [GetListenersAsync\(CancellationToken\)](GetListenersAsync(CancellationToken).md 'Pmmux\.Core\.PortMultiplexer\.GetListenersAsync\(System\.Threading\.CancellationToken\)') | Get the currently bound listener endpoints\. |
| [RemoveListenerAsync\(Protocol, int, CancellationToken\)](RemoveListenerAsync(Protocol,int,CancellationToken).md 'Pmmux\.Core\.PortMultiplexer\.RemoveListenerAsync\(Mono\.Nat\.Protocol, int, System\.Threading\.CancellationToken\)') | Remove an existing listener on the specified port\. |
| [StartAsync\(CancellationToken\)](StartAsync(CancellationToken).md 'Pmmux\.Core\.PortMultiplexer\.StartAsync\(System\.Threading\.CancellationToken\)') | Start accepting client connections and messages on bound listeners\. |
