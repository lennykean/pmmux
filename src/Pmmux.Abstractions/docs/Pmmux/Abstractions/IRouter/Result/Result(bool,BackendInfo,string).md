#### [Pmmux\.Abstractions](../../../../index.md 'index')
### [Pmmux\.Abstractions](../../index.md 'Pmmux\.Abstractions').[IRouter](../index.md 'Pmmux\.Abstractions\.IRouter').[Result](index.md 'Pmmux\.Abstractions\.IRouter\.Result')

## Result\(bool, BackendInfo, string\) Constructor

Result of a routing operation\.

```csharp
public Result(bool Success, Pmmux.Abstractions.BackendInfo? Backend, string? Reason);
```
#### Parameters

<a name='Pmmux.Abstractions.IRouter.Result.Result(bool,Pmmux.Abstractions.BackendInfo,string).Success'></a>

`Success` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` if routing succeeded, otherwise `false`\.

<a name='Pmmux.Abstractions.IRouter.Result.Result(bool,Pmmux.Abstractions.BackendInfo,string).Backend'></a>

`Backend` [BackendInfo](../../BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo')

The selected backend if successful\.

<a name='Pmmux.Abstractions.IRouter.Result.Result(bool,Pmmux.Abstractions.BackendInfo,string).Reason'></a>

`Reason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The failure reason if unsuccessful\.