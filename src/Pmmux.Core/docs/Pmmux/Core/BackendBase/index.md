#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## BackendBase Class

Base class for backend implementations providing common parameter support\.

```csharp
public abstract class BackendBase : Pmmux.Abstractions.IBackend, System.IAsyncDisposable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; BackendBase

Derived  
&#8627; [PassthroughBackend](../PassthroughBackend/index.md 'Pmmux\.Core\.PassthroughBackend')

Implements [IBackend](..\..\Pmmux.Abstractions\docs\Pmmux/Abstractions/IBackend/index.md 'Pmmux\.Abstractions\.IBackend'), [System\.IAsyncDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable 'System\.IAsyncDisposable')

### Remarks
Supports `local-ip`, `remote-ip`, `local-port`, `remote-port`,
`property[key]`, and `priority` parameters\.

| Constructors | |
| :--- | :--- |
| [BackendBase\(BackendSpec, Dictionary&lt;string,string&gt;, PriorityTier\)](BackendBase(BackendSpec,Dictionary_string,string_,PriorityTier).md 'Pmmux\.Core\.BackendBase\.BackendBase\(Pmmux\.Abstractions\.BackendSpec, System\.Collections\.Generic\.Dictionary\<string,string\>, Pmmux\.Abstractions\.PriorityTier\)') | |

| Properties | |
| :--- | :--- |
| [Backend](Backend.md 'Pmmux\.Core\.BackendBase\.Backend') | The backend metadata and configuration information\. |
