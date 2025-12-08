#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IClientWriter Interface

Writes data to a client via a listener\.

```csharp
public interface IClientWriter
```

| Properties | |
| :--- | :--- |
| [ListenerInfo](ListenerInfo.md 'Pmmux\.Abstractions\.IClientWriter\.ListenerInfo') | Listener the writer is associated with\. |

| Methods | |
| :--- | :--- |
| [WriteAsync\(byte\[\], CancellationToken\)](WriteAsync(byte[],CancellationToken).md 'Pmmux\.Abstractions\.IClientWriter\.WriteAsync\(byte\[\], System\.Threading\.CancellationToken\)') | Write data to the client through the listener\. |
