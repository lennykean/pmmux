#### [Pmmux\.Extensions\.Acme\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Acme\.Abstractions](../index.md 'Pmmux\.Extensions\.Acme\.Abstractions').[IChallengeProcessor](index.md 'Pmmux\.Extensions\.Acme\.Abstractions\.IChallengeProcessor')

## IChallengeProcessor\.PrepareAsync\(IEnumerable\<AuthorizationInfo\>, IKey, string, IReadOnlyDictionary\<string,string\>, CancellationToken\) Method

Prepare challenge responses for the specified challenge\.

```csharp
System.Threading.Tasks.Task<System.IAsyncDisposable> PrepareAsync(System.Collections.Generic.IEnumerable<Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo> authorizations, Certes.IKey accountKey, string? provider, System.Collections.Generic.IReadOnlyDictionary<string,string> properties, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Pmmux.Extensions.Acme.Abstractions.IChallengeProcessor.PrepareAsync(System.Collections.Generic.IEnumerable_Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo_,Certes.IKey,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).authorizations'></a>

`authorizations` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[AuthorizationInfo](../AuthorizationInfo/index.md 'Pmmux\.Extensions\.Acme\.Abstractions\.AuthorizationInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The authorizations and their selected challenge contexts\.

<a name='Pmmux.Extensions.Acme.Abstractions.IChallengeProcessor.PrepareAsync(System.Collections.Generic.IEnumerable_Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo_,Certes.IKey,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).accountKey'></a>

`accountKey` [Certes\.IKey](https://learn.microsoft.com/en-us/dotnet/api/certes.ikey 'Certes\.IKey')

The ACME account key used to derive challenge tokens\.

<a name='Pmmux.Extensions.Acme.Abstractions.IChallengeProcessor.PrepareAsync(System.Collections.Generic.IEnumerable_Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo_,Certes.IKey,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).provider'></a>

`provider` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The provider name from the certificate entry, or `null` for default\.

<a name='Pmmux.Extensions.Acme.Abstractions.IChallengeProcessor.PrepareAsync(System.Collections.Generic.IEnumerable_Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo_,Certes.IKey,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).properties'></a>

`properties` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The per\-certificate properties from the certificate entry\.

<a name='Pmmux.Extensions.Acme.Abstractions.IChallengeProcessor.PrepareAsync(System.Collections.Generic.IEnumerable_Pmmux.Extensions.Acme.Abstractions.AuthorizationInfo_,Certes.IKey,string,System.Collections.Generic.IReadOnlyDictionary_string,string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A handle that cleans up challenge resources when disposed\.