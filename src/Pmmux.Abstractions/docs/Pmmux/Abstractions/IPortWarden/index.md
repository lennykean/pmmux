#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IPortWarden Interface

Manages NAT port mappings \(UPnP/PMP\)\.

```csharp
public interface IPortWarden : System.IAsyncDisposable
```

Implements [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Port warden discovers NAT devices on the network, then creates and maintains port mappings that forward external
traffic to the multiplexer's listeners\.

| Properties | |
| :--- | :--- |
| [NatDevice](NatDevice.md 'Pmmux\.Abstractions\.IPortWarden\.NatDevice') | Discovered NAT device, if any\. |

| Methods | |
| :--- | :--- |
| [AddPortMapAsync\(Protocol, Nullable&lt;int&gt;, Nullable&lt;int&gt;, CancellationToken\)](AddPortMapAsync(Protocol,Nullable_int_,Nullable_int_,CancellationToken).md 'Pmmux\.Abstractions\.IPortWarden\.AddPortMapAsync\(Mono\.Nat\.Protocol, System\.Nullable\<int\>, System\.Nullable\<int\>, System\.Threading\.CancellationToken\)') | Add a new port mapping\. |
| [GetPortMaps\(\)](GetPortMaps().md 'Pmmux\.Abstractions\.IPortWarden\.GetPortMaps\(\)') | Get the current port mappings\. |
| [RemovePortMapAsync\(Protocol, int, int, CancellationToken\)](RemovePortMapAsync(Protocol,int,int,CancellationToken).md 'Pmmux\.Abstractions\.IPortWarden\.RemovePortMapAsync\(Mono\.Nat\.Protocol, int, int, System\.Threading\.CancellationToken\)') | Remove an existing port mapping\. |
| [StartAsync\(CancellationToken\)](StartAsync(CancellationToken).md 'Pmmux\.Abstractions\.IPortWarden\.StartAsync\(System\.Threading\.CancellationToken\)') | Discover NAT devices and create port mappings from the initial configuration\. |
