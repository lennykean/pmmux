#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## ClientInfo Class

Endpoint information for a remote client\.

```csharp
public sealed record ClientInfo : System.IEquatable<Pmmux.Abstractions.ClientInfo>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; ClientInfo

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ClientInfo](index.md 'Pmmux\.Abstractions\.ClientInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [ClientInfo\(IPEndPoint, IPEndPoint\)](ClientInfo(IPEndPoint,IPEndPoint).md 'Pmmux\.Abstractions\.ClientInfo\.ClientInfo\(System\.Net\.IPEndPoint, System\.Net\.IPEndPoint\)') | Endpoint information for a remote client\. |

| Properties | |
| :--- | :--- |
| [LocalEndpoint](LocalEndpoint.md 'Pmmux\.Abstractions\.ClientInfo\.LocalEndpoint') | Local endpoint that received the connection\. |
| [RemoteEndpoint](RemoteEndpoint.md 'Pmmux\.Abstractions\.ClientInfo\.RemoteEndpoint') | Remote endpoint \(client address\) that initiated the connection\. |
