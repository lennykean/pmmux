#### [Pmmux\.Extensions\.Acme\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Acme\.Abstractions](../index.md 'Pmmux\.Extensions\.Acme\.Abstractions').[IDnsProvider](index.md 'Pmmux\.Extensions\.Acme\.Abstractions\.IDnsProvider')

## IDnsProvider\.CreateTxtRecordAsync\(string, string, IReadOnlyDictionary\<string,string\>, CancellationToken\) Method

Create a TXT record for the ACME challenge\.

```csharp
System.Threading.Tasks.Task<string?> CreateTxtRecordAsync(string recordName, string txtValue, System.Collections.Generic.IReadOnlyDictionary<string,string> properties, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.CreateTxtRecordAsync(string,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).recordName'></a>

`recordName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The fully qualified DNS record name\.

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.CreateTxtRecordAsync(string,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).txtValue'></a>

`txtValue` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The TXT record value\.

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.CreateTxtRecordAsync(string,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).properties'></a>

`properties` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The per\-certificate provider properties from the certificate entry\.

<a name='Pmmux.Extensions.Acme.Abstractions.IDnsProvider.CreateTxtRecordAsync(string,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An optional provider\-specific propagation token, or `null` if not applicable\.