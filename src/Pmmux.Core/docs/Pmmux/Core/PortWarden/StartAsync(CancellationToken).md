#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[PortWarden](index.md 'Pmmux\.Core\.PortWarden')

## PortWarden\.StartAsync\(CancellationToken\) Method

Discover NAT devices and create port mappings from the initial configuration\.

```csharp
public System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Pmmux.Core.PortWarden.StartAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

Implements [StartAsync\(CancellationToken\)](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IPortWarden/StartAsync(CancellationToken).md 'Pmmux\.Abstractions\.IPortWarden\.StartAsync\(System\.Threading\.CancellationToken\)')

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the asynchronous startup operation\.