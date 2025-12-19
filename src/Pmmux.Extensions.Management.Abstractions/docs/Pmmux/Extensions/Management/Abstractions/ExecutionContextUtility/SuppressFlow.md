#### [Pmmux\.Extensions\.Management\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Abstractions](../index.md 'Pmmux\.Extensions\.Management\.Abstractions').[ExecutionContextUtility](index.md 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility')

## ExecutionContextUtility\.SuppressFlow Method

| Overloads | |
| :--- | :--- |
| [SuppressFlow\(Action, bool\)](SuppressFlow.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Action,bool) 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\(System\.Action, bool\)') | Suppresses the flow of the execution context for the duration of the specified action\. |
| [SuppressFlow\(Func&lt;Task&gt;, bool\)](SuppressFlow.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Func_System.Threading.Tasks.Task_,bool) 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\(System\.Func\<System\.Threading\.Tasks\.Task\>, bool\)') | Suppresses the flow of the execution context for the duration of the specified async action\. |
| [SuppressFlow&lt;T&gt;\(Func&lt;Task&lt;T&gt;&gt;, bool\)](SuppressFlow.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_System.Threading.Tasks.Task_T__,bool) 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\<T\>\(System\.Func\<System\.Threading\.Tasks\.Task\<T\>\>, bool\)') | Suppresses the flow of the execution context for the duration of the specified function\. |
| [SuppressFlow&lt;T&gt;\(Func&lt;T&gt;, bool\)](SuppressFlow.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_T_,bool) 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\<T\>\(System\.Func\<T\>, bool\)') | Suppresses the flow of the execution context for the duration of the specified function\. |

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Action,bool)'></a>

## ExecutionContextUtility\.SuppressFlow\(Action, bool\) Method

Suppresses the flow of the execution context for the duration of the specified action\.

```csharp
public static System.Threading.Tasks.Task SuppressFlow(System.Action action, bool clearActivity=true);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Action,bool).action'></a>

`action` [System\.Action](https://learn.microsoft.com/en-us/dotnet/api/system.action 'System\.Action')

The action to execute\.

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Action,bool).clearActivity'></a>

`clearActivity` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether to clear the activity context\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Func_System.Threading.Tasks.Task_,bool)'></a>

## ExecutionContextUtility\.SuppressFlow\(Func\<Task\>, bool\) Method

Suppresses the flow of the execution context for the duration of the specified async action\.

```csharp
public static System.Threading.Tasks.Task SuppressFlow(System.Func<System.Threading.Tasks.Task> asyncAction, bool clearActivity=true);
```
#### Parameters

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Func_System.Threading.Tasks.Task_,bool).asyncAction'></a>

`asyncAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

The async action to execute\.

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Func_System.Threading.Tasks.Task_,bool).clearActivity'></a>

`clearActivity` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether to clear the activity context\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_System.Threading.Tasks.Task_T__,bool)'></a>

## ExecutionContextUtility\.SuppressFlow\<T\>\(Func\<Task\<T\>\>, bool\) Method

Suppresses the flow of the execution context for the duration of the specified function\.

```csharp
public static System.Threading.Tasks.Task<T> SuppressFlow<T>(System.Func<System.Threading.Tasks.Task<T>> func, bool clearActivity=true);
```
#### Type parameters

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_System.Threading.Tasks.Task_T__,bool).T'></a>

`T`

The type of the result of the function\.
#### Parameters

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_System.Threading.Tasks.Task_T__,bool).func'></a>

`func` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[T](index.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_System.Threading.Tasks.Task_T__,bool).T 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\<T\>\(System\.Func\<System\.Threading\.Tasks\.Task\<T\>\>, bool\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

The function to execute\.

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_System.Threading.Tasks.Task_T__,bool).clearActivity'></a>

`clearActivity` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether to clear the activity context\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[T](index.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_System.Threading.Tasks.Task_T__,bool).T 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\<T\>\(System\.Func\<System\.Threading\.Tasks\.Task\<T\>\>, bool\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The result of the function\.

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_T_,bool)'></a>

## ExecutionContextUtility\.SuppressFlow\<T\>\(Func\<T\>, bool\) Method

Suppresses the flow of the execution context for the duration of the specified function\.

```csharp
public static System.Threading.Tasks.Task<T> SuppressFlow<T>(System.Func<T> func, bool clearActivity=true);
```
#### Type parameters

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_T_,bool).T'></a>

`T`

The type of the result of the function\.
#### Parameters

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_T_,bool).func'></a>

`func` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[T](index.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_T_,bool).T 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\<T\>\(System\.Func\<T\>, bool\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

The function to execute\.

<a name='Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_T_,bool).clearActivity'></a>

`clearActivity` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether to clear the activity context\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[T](index.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_T_,bool).T 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\<T\>\(System\.Func\<T\>, bool\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The result of the function\.