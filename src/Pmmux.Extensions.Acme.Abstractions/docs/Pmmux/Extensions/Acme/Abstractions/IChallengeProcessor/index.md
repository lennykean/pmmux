#### [Pmmux\.Extensions\.Acme\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Acme\.Abstractions](../index.md 'Pmmux\.Extensions\.Acme\.Abstractions')

## IChallengeProcessor Interface

A challenge processor that handles a specific ACME challenge type\.

```csharp
public interface IChallengeProcessor
```

| Properties | |
| :--- | :--- |
| [ChallengeType](ChallengeType.md 'Pmmux\.Extensions\.Acme\.Abstractions\.IChallengeProcessor\.ChallengeType') | The ACME challenge type that this processor handles\. |

| Methods | |
| :--- | :--- |
| [InitializeBatchAsync\(CancellationToken\)](InitializeBatchAsync(CancellationToken).md 'Pmmux\.Extensions\.Acme\.Abstractions\.IChallengeProcessor\.InitializeBatchAsync\(System\.Threading\.CancellationToken\)') | Initialize batch processing for this challenge type\. |
| [PrepareAsync\(IEnumerable&lt;AuthorizationInfo&gt;, IKey, string, IReadOnlyDictionary&lt;string,string&gt;, CancellationToken\)](PrepareAsync(IEnumerable_AuthorizationInfo_,IKey,string,IReadOnlyDictionary_string,string_,CancellationToken).md 'Pmmux\.Extensions\.Acme\.Abstractions\.IChallengeProcessor\.PrepareAsync\(System\.Collections\.Generic\.IEnumerable\<Pmmux\.Extensions\.Acme\.Abstractions\.AuthorizationInfo\>, Certes\.IKey, string, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, System\.Threading\.CancellationToken\)') | Prepare challenge responses for the specified challenge\. |
