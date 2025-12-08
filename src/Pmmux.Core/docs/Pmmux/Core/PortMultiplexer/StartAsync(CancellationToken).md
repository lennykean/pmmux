#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PortMultiplexer](index.md 'Pmmux\.Core\.PortMultiplexer')

## PortMultiplexer\.StartAsync\(CancellationToken\) Method

Start accepting client connections and messages on bound listeners\.

```csharp
public System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Core.PortMultiplexer.StartAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [StartAsync\(CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortMultiplexer/StartAsync(CancellationToken).md 'Pmmux\.Abstractions\.IPortMultiplexer\.StartAsync\(System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task that represents the asynchronous start operation\.