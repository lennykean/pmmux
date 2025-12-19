#### [Pmmux\.Extensions\.Management\.Abstractions](../../../../../index.md 'index')
### [Pmmux\.Extensions\.Management\.Abstractions](../index.md 'Pmmux\.Extensions\.Management\.Abstractions')

## ExecutionContextUtility Class

Utility methods for working with [System\.Threading\.ExecutionContext](https://learn.microsoft.com/en-us/dotnet/api/system.threading.executioncontext 'System\.Threading\.ExecutionContext')\.

```csharp
public static class ExecutionContextUtility
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; ExecutionContextUtility

| Methods | |
| :--- | :--- |
| [SuppressFlow\(Action, bool\)](SuppressFlow.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Action,bool) 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\(System\.Action, bool\)') | Suppresses the flow of the execution context for the duration of the specified action\. |
| [SuppressFlow\(Func&lt;Task&gt;, bool\)](SuppressFlow.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow(System.Func_System.Threading.Tasks.Task_,bool) 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\(System\.Func\<System\.Threading\.Tasks\.Task\>, bool\)') | Suppresses the flow of the execution context for the duration of the specified async action\. |
| [SuppressFlow&lt;T&gt;\(Func&lt;Task&lt;T&gt;&gt;, bool\)](SuppressFlow.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_System.Threading.Tasks.Task_T__,bool) 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\<T\>\(System\.Func\<System\.Threading\.Tasks\.Task\<T\>\>, bool\)') | Suppresses the flow of the execution context for the duration of the specified function\. |
| [SuppressFlow&lt;T&gt;\(Func&lt;T&gt;, bool\)](SuppressFlow.md#Pmmux.Extensions.Management.Abstractions.ExecutionContextUtility.SuppressFlow_T_(System.Func_T_,bool) 'Pmmux\.Extensions\.Management\.Abstractions\.ExecutionContextUtility\.SuppressFlow\<T\>\(System\.Func\<T\>, bool\)') | Suppresses the flow of the execution context for the duration of the specified function\. |
