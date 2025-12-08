#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IClientWriterFactory](index.md 'Pmmux\.Abstractions\.IClientWriterFactory')

## IClientWriterFactory\.CreateWriter\(ClientInfo\) Method

Create a client writer for the specified client\.

```csharp
Pmmux.Abstractions.IClientWriter CreateWriter(Pmmux.Abstractions.ClientInfo clientInfo);
```
#### Parameters

<a name='Pmmux.Abstractions.IClientWriterFactory.CreateWriter(Pmmux.Abstractions.ClientInfo).clientInfo'></a>

`clientInfo` [ClientInfo](../ClientInfo/index.md 'Pmmux\.Abstractions\.ClientInfo')

The client to create a writer for\.

#### Returns
[IClientWriter](../IClientWriter/index.md 'Pmmux\.Abstractions\.IClientWriter')  
An instance of [IClientWriter](../IClientWriter/index.md 'Pmmux\.Abstractions\.IClientWriter') for the client\.