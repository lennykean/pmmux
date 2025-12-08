#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IConnection Interface

Bidirectional data connection using [System\.IO\.Pipelines](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipelines 'System\.IO\.Pipelines')\.

```csharp
public interface IConnection : System.IAsyncDisposable
```

Derived  
&#8627; [IClientConnection](../IClientConnection/index.md 'Pmmux\.Abstractions\.IClientConnection')

Implements [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Abstracts both client and backend connections, providing an interface to read and write
data through high\-performance pipelines\. Connections carry metadata in the [Properties](Properties.md 'Pmmux\.Abstractions\.IConnection\.Properties')
dictionary for protocol\-specific information or routing context\.

| Properties | |
| :--- | :--- |
| [Properties](Properties.md 'Pmmux\.Abstractions\.IConnection\.Properties') | Metadata associated with the connection\. |

| Methods | |
| :--- | :--- |
| [CloseAsync\(\)](CloseAsync().md 'Pmmux\.Abstractions\.IConnection\.CloseAsync\(\)') | Close the connection gracefully, allowing pending data to be flushed\. |
| [GetReader\(\)](GetReader().md 'Pmmux\.Abstractions\.IConnection\.GetReader\(\)') | Get pipeline reader for reading data from this connection\. |
| [GetWriter\(\)](GetWriter().md 'Pmmux\.Abstractions\.IConnection\.GetWriter\(\)') | Get pipeline writer for writing data to this connection\. |
