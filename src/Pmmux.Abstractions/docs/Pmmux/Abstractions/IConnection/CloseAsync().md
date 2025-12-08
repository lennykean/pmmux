#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IConnection](index.md 'Pmmux\.Abstractions\.IConnection')

## IConnection\.CloseAsync\(\) Method

Close the connection gracefully, allowing pending data to be flushed\.

```csharp
System.Threading.Tasks.Task CloseAsync();
```

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the asynchronous close operation\.