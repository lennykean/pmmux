#### [Pmmux\.Extensions\.Acme\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Acme\.Abstractions](../index.md 'Pmmux\.Extensions\.Acme\.Abstractions').[IDnsProvider](index.md 'Pmmux\.Extensions\.Acme\.Abstractions\.IDnsProvider')

## IDnsProvider\.WaitForPropagationAsync\(string, string, string, TimeSpan, CancellationToken\) Method

Wait for the TXT record to propagate and become visible to DNS resolvers\.

```csharp
System.Threading.Tasks.Task<bool> WaitForPropagationAsync(string recordName, string txtValue, string? propagationToken, System.TimeSpan timeout, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.WaitForPropagationAsync(string,string,string,System.TimeSpan,System.Threading.CancellationToken).recordName'></a>

`recordName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The fully qualified DNS record name\.

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.WaitForPropagationAsync(string,string,string,System.TimeSpan,System.Threading.CancellationToken).txtValue'></a>

`txtValue` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The expected TXT record value\.

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.WaitForPropagationAsync(string,string,string,System.TimeSpan,System.Threading.CancellationToken).propagationToken'></a>

`propagationToken` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The provider\-specific propagation token returned by [CreateTxtRecordAsync\(string, string, IReadOnlyDictionary&lt;string,string&gt;, CancellationToken\)](CreateTxtRecordAsync(string,string,IReadOnlyDictionary_string,string_,CancellationToken).md 'Pmmux\.Extensions\.Acme\.Abstractions\.IDnsProvider\.CreateTxtRecordAsync\(string, string, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, System\.Threading\.CancellationToken\)')\.

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.WaitForPropagationAsync(string,string,string,System.TimeSpan,System.Threading.CancellationToken).timeout'></a>

`timeout` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The maximum time to wait for propagation\.

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.WaitForPropagationAsync(string,string,string,System.TimeSpan,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
`true` if the record propagated within the timeout; otherwise `false`\.