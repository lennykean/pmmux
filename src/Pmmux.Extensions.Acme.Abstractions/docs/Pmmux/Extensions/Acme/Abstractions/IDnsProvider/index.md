#### [Pmmux\.Extensions\.Acme\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Acme\.Abstractions](../index.md 'Pmmux\.Extensions\.Acme\.Abstractions')

## IDnsProvider Interface

Provider for ACME DNS\-01 challenges\.

```csharp
public interface IDnsProvider
```

| Properties | |
| :--- | :--- |
| [Name](Name.md 'Pmmux\.Extensions\.Acme\.Abstractions\.IDnsProvider\.Name') | The provider name\. |

| Methods | |
| :--- | :--- |
| [CreateTxtRecordAsync\(string, string, IReadOnlyDictionary&lt;string,string&gt;, CancellationToken\)](CreateTxtRecordAsync(string,string,IReadOnlyDictionary_string,string_,CancellationToken).md 'Pmmux\.Extensions\.Acme\.Abstractions\.IDnsProvider\.CreateTxtRecordAsync\(string, string, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, System\.Threading\.CancellationToken\)') | Create a TXT record for the ACME challenge\. |
| [DeleteTxtRecordAsync\(string, string, IReadOnlyDictionary&lt;string,string&gt;, CancellationToken\)](DeleteTxtRecordAsync(string,string,IReadOnlyDictionary_string,string_,CancellationToken).md 'Pmmux\.Extensions\.Acme\.Abstractions\.IDnsProvider\.DeleteTxtRecordAsync\(string, string, System\.Collections\.Generic\.IReadOnlyDictionary\<string,string\>, System\.Threading\.CancellationToken\)') | Delete a TXT record\. |
| [WaitForPropagationAsync\(string, string, string, TimeSpan, CancellationToken\)](WaitForPropagationAsync(string,string,string,TimeSpan,CancellationToken).md 'Pmmux\.Extensions\.Acme\.Abstractions\.IDnsProvider\.WaitForPropagationAsync\(string, string, string, System\.TimeSpan, System\.Threading\.CancellationToken\)') | Wait for the TXT record to propagate and become visible to DNS resolvers\. |
