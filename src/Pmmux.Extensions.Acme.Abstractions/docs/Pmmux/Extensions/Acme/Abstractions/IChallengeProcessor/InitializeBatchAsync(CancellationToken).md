#### [Pmmux\.Extensions\.Acme\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Acme\.Abstractions](../index.md 'Pmmux\.Extensions\.Acme\.Abstractions').[IChallengeProcessor](index.md 'Pmmux\.Extensions\.Acme\.Abstractions\.IChallengeProcessor')

## IChallengeProcessor\.InitializeBatchAsync\(CancellationToken\) Method

Initialize batch processing for this challenge type\.

```csharp
System.Threading.Tasks.Task<System.IAsyncDisposable> InitializeBatchAsync(System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Pmmux.Extensions.Acme.Abstractions.IChallengeProcessor.InitializeBatchAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A handle that cleans up batch resources when disposed\.

### Remarks
Called once before processing a batch of certificates that use this challenge type\.