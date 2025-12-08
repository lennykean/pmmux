#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[ClientInfo](index.md 'Pmmux\.Abstractions\.ClientInfo')

## ClientInfo\(IPEndPoint, IPEndPoint\) Constructor

Endpoint information for a remote client\.

```csharp
public ClientInfo(System.Net.IPEndPoint? LocalEndpoint, System.Net.IPEndPoint? RemoteEndpoint);
```
#### Parameters

<a name='Pmmux.Abstractions.ClientInfo.ClientInfo(System.Net.IPEndPoint,System.Net.IPEndPoint).LocalEndpoint'></a>

`LocalEndpoint` [System\.Net\.IPEndPoint](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipendpoint 'System\.Net\.IPEndPoint')

Local endpoint that received the connection\.

<a name='Pmmux.Abstractions.ClientInfo.ClientInfo(System.Net.IPEndPoint,System.Net.IPEndPoint).RemoteEndpoint'></a>

`RemoteEndpoint` [System\.Net\.IPEndPoint](https://learn.microsoft.com/en-us/dotnet/api/system.net.ipendpoint 'System\.Net\.IPEndPoint')

Remote endpoint \(client address\) that initiated the connection\.