#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IPortMultiplexer Interface

Main orchestration interface for the port multiplexer\.

```csharp
public interface IPortMultiplexer : System.IAsyncDisposable
```

Implements [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

| Methods | |
| :--- | :--- |
| [AddListener\(Protocol, int, IPAddress\)](AddListener(Protocol,int,IPAddress).md 'Pmmux\.Abstractions\.IPortMultiplexer\.AddListener\(Mono\.Nat\.Protocol, int, System\.Net\.IPAddress\)') | Add a new listener on the specified port\. |
| [GetListenersAsync\(CancellationToken\)](GetListenersAsync(CancellationToken).md 'Pmmux\.Abstractions\.IPortMultiplexer\.GetListenersAsync\(System\.Threading\.CancellationToken\)') | Get the currently bound listener endpoints\. |
| [RemoveListenerAsync\(Protocol, int, CancellationToken\)](RemoveListenerAsync(Protocol,int,CancellationToken).md 'Pmmux\.Abstractions\.IPortMultiplexer\.RemoveListenerAsync\(Mono\.Nat\.Protocol, int, System\.Threading\.CancellationToken\)') | Remove an existing listener on the specified port\. |
| [StartAsync\(CancellationToken\)](StartAsync(CancellationToken).md 'Pmmux\.Abstractions\.IPortMultiplexer\.StartAsync\(System\.Threading\.CancellationToken\)') | Start accepting client connections and messages on bound listeners\. |
