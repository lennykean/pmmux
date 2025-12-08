#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IClientConnectionNegotiator](index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator')

## IClientConnectionNegotiator\.NegotiateAsync\(ClientConnectionContext, Func\<Task\<Result\>\>, CancellationToken\) Method

Negotiate a client connection, performing any required transport\-level processing\.

```csharp
System.Threading.Tasks.Task<Pmmux.Abstractions.IClientConnectionNegotiator.Result> NegotiateAsync(Pmmux.Abstractions.ClientConnectionContext context, System.Func<System.Threading.Tasks.Task<Pmmux.Abstractions.IClientConnectionNegotiator.Result>> next, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IClientConnectionNegotiator.NegotiateAsync(Pmmux.Abstractions.ClientConnectionContext,System.Func_System.Threading.Tasks.Task_Pmmux.Abstractions.IClientConnectionNegotiator.Result__,System.Threading.CancellationToken).context'></a>

`context` [ClientConnectionContext](../ClientConnectionContext/index.md 'Pmmux\.Abstractions\.ClientConnectionContext')

The context containing client information, properties, socket, and stream\.

<a name='Pmmux.Abstractions.IClientConnectionNegotiator.NegotiateAsync(Pmmux.Abstractions.ClientConnectionContext,System.Func_System.Threading.Tasks.Task_Pmmux.Abstractions.IClientConnectionNegotiator.Result__,System.Threading.CancellationToken).next'></a>

`next` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Result](Result/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

Function to invoke the next negotiator in the chain\. Returns the result from the next negotiator\.

<a name='Pmmux.Abstractions.IClientConnectionNegotiator.NegotiateAsync(Pmmux.Abstractions.ClientConnectionContext,System.Func_System.Threading.Tasks.Task_Pmmux.Abstractions.IClientConnectionNegotiator.Result__,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Result](Result/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A [Result](Result/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result') with the negotiated [IClientConnection](../IClientConnection/index.md 'Pmmux\.Abstractions\.IClientConnection') if accepted,
or a rejection reason if rejected\.

### Remarks
Negotiators can accept the connection or pass it to the next negotiator in the chain\.
Negotiators may modify [Properties](../ClientConnectionContext/Properties.md 'Pmmux\.Abstractions\.ClientConnectionContext\.Properties') or
wrap and replace [ClientConnectionStream](../ClientConnectionContext/ClientConnectionStream.md 'Pmmux\.Abstractions\.ClientConnectionContext\.ClientConnectionStream') before returning\.