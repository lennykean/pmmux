#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IMetricReporter](index.md 'Pmmux\.Abstractions\.IMetricReporter')

## IMetricReporter\.MeasureDuration Method

| Overloads | |
| :--- | :--- |
| [MeasureDuration\(string, string, Dictionary&lt;string,string&gt;\)](MeasureDuration.md#Pmmux.Abstractions.IMetricReporter.MeasureDuration(string,string,System.Collections.Generic.Dictionary_string,string_) 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDuration\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>\)') | Create a disposable object that measures and reports duration from creation to disposal\. |
| [MeasureDuration&lt;T&gt;\(string, string, Dictionary&lt;string,string&gt;, Func&lt;T&gt;\)](MeasureDuration.md#Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_) 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDuration\<T\>\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.Func\<T\>\)') | Execute an operation and measure its duration\. |

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration(string,string,System.Collections.Generic.Dictionary_string,string_)'></a>

## IMetricReporter\.MeasureDuration\(string, string, Dictionary\<string,string\>\) Method

Create a disposable object that measures and reports duration from creation to disposal\.

```csharp
System.IDisposable MeasureDuration(string name, string category, System.Collections.Generic.Dictionary<string,string?> metadata);
```
#### Parameters

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration(string,string,System.Collections.Generic.Dictionary_string,string_).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric name\.

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration(string,string,System.Collections.Generic.Dictionary_string,string_).category'></a>

`category` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric category for grouping\.

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration(string,string,System.Collections.Generic.Dictionary_string,string_).metadata'></a>

`metadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Additional dimensions for the metric\.

#### Returns
[System\.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable 'System\.IDisposable')  
[System\.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable 'System\.IDisposable') that starts timing immediately and reports the duration when disposed\.

### Example

```csharp
using (metricReporter.MeasureDuration("operation_duration", "backend"))
{
    // Code block to measure
}
```

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_)'></a>

## IMetricReporter\.MeasureDuration\<T\>\(string, string, Dictionary\<string,string\>, Func\<T\>\) Method

Execute an operation and measure its duration\.

```csharp
T MeasureDuration<T>(string name, string category, System.Collections.Generic.Dictionary<string,string?> metadata, System.Func<T> operation);
```
#### Type parameters

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_).T'></a>

`T`

Return type of the operation\.
#### Parameters

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric name\.

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_).category'></a>

`category` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric category for grouping\.

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_).metadata'></a>

`metadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Additional dimensions for the metric\.

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_).operation'></a>

`operation` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[T](index.md#Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_).T 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDuration\<T\>\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.Func\<T\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

The operation to measure\.

#### Returns
[T](index.md#Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_).T 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDuration\<T\>\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.Func\<T\>\)\.T')  
Result of the operation\.