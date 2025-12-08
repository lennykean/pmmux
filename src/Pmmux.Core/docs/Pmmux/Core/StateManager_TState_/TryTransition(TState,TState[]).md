#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core').[StateManager&lt;TState&gt;](index.md 'Pmmux\.Core\.StateManager\<TState\>')

## StateManager\<TState\>\.TryTransition\(TState, TState\[\]\) Method

Attempts to transition from one of the specified states to the target state\.

```csharp
public bool TryTransition(TState to, params TState[] from);
```
#### Parameters

<a name='Pmmux.Core.StateManager_TState_.TryTransition(TState,TState[]).to'></a>

`to` [TState](index.md#Pmmux.Core.StateManager_TState_.TState 'Pmmux\.Core\.StateManager\<TState\>\.TState')

The target state\.

<a name='Pmmux.Core.StateManager_TState_.TryTransition(TState,TState[]).from'></a>

`from` [TState](index.md#Pmmux.Core.StateManager_TState_.TState 'Pmmux\.Core\.StateManager\<TState\>\.TState')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The allowed source states\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True if transition succeeded; otherwise false\.