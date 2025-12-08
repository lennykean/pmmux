#### [Pmmux\.Extensions\.Tls\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Tls\.Abstractions](../index.md 'Pmmux\.Extensions\.Tls\.Abstractions').[ICertificateManager](index.md 'Pmmux\.Extensions\.Tls\.Abstractions\.ICertificateManager')

## ICertificateManager\.GetMappings\(\) Method

Gets all hostname\-to\-certificate mappings\.

```csharp
System.Collections.Generic.IEnumerable<(string Hostname,string CertificateName)> GetMappings();
```

#### Returns
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')