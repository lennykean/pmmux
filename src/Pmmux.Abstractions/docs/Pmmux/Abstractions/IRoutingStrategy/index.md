#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IRoutingStrategy Interface

Strategy for selecting a backend from multiple candidates during routing\.

```csharp
public interface IRoutingStrategy
```

### Remarks
The router uses the configured routing strategy to select a single backend when multiple
backends match a client request\. Backend matching is performed lazily via async enumeration,
allowing strategies to short\-circuit once a suitable backend is found\.

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Abstractions\.IRoutingStrategy\.Name') | Name of the routing strategy\. |

| Methods | |
| :--- | :--- |
| [SelectBackendAsync\(ClientInfo, IReadOnlyDictionary&lt;string,string&gt;, IAsyncEnumerable&lt;BackendStatusInfo&gt;, CancellationToken\)](SelectBackendAsync(ClientInfo,IReadOnlyDictionary_string,string_,IAsyncEnumerable_BackendStatusInfo_,CancellationToken).md 'Pmmux\.Abstractions\.IRoutingStrategy\.SelectBackendAsync\(Pmmux\.Abstractions\.ClientInfo, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, System\.Collections\.Generic\.IAsyncEnumerable\<Pmmux\.Abstractions\.BackendStatusInfo\>, System\.Threading\.CancellationToken\)') | Select a backend from matched candidates to handle the client request\. |
