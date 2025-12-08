#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions').[IMetricReporter](index.md 'Pmmux\.Abstractions\.IMetricReporter')

## IMetricReporter\.MeasureDurationAsync\<T\>\(string, string, Dictionary\<string,string\>, Func\<Task\<T\>\>\) Method

Execute an asynchronous operation and measure its duration\.

```csharp
System.Threading.Tasks.Task<T> MeasureDurationAsync<T>(string name, string category, System.Collections.Generic.Dictionary<string,string?> metadata, System.Func<System.Threading.Tasks.Task<T>> operation);
```
#### Type parameters

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDurationAsync_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_System.Threading.Tasks.Task_T__).T'></a>

`T`

Return type of the operation\.
#### Parameters

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDurationAsync_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_System.Threading.Tasks.Task_T__).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric name\.

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDurationAsync_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_System.Threading.Tasks.Task_T__).category'></a>

`category` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The metric category for grouping\.

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDurationAsync_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_System.Threading.Tasks.Task_T__).metadata'></a>

`metadata` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

Additional dimensions for the metric\.

<a name='Pmmux.Abstractions.IMetricReporter.MeasureDurationAsync_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_System.Threading.Tasks.Task_T__).operation'></a>

`operation` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[T](MeasureDurationAsync_T_(string,string,Dictionary_string,string_,Func_Task_T__).md#Pmmux.Abstractions.IMetricReporter.MeasureDurationAsync_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_System.Threading.Tasks.Task_T__).T 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDurationAsync\<T\>\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.Func\<System\.Threading\.Tasks\.Task\<T\>\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

The asynchronous operation to measure\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[T](MeasureDurationAsync_T_(string,string,Dictionary_string,string_,Func_Task_T__).md#Pmmux.Abstractions.IMetricReporter.MeasureDurationAsync_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_System.Threading.Tasks.Task_T__).T 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDurationAsync\<T\>\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.Func\<System\.Threading\.Tasks\.Task\<T\>\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the operation\.