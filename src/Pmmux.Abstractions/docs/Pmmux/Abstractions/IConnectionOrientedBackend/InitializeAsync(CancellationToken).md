#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IConnectionOrientedBackend](index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend')

## IConnectionOrientedBackend\.InitializeAsync\(CancellationToken\) Method

Initialize the backend at startup\.

```csharp
System.Threading.Tasks.Task InitializeAsync(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Pmmux.Abstractions.IConnectionOrientedBackend.InitializeAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task representing the initialization operation\.

### Remarks
Perform any one\-time setup such as establishing persistent connections to upstream servers,
warming caches, or validating configuration\.