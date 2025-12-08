#### [Pmmux\.Core](../../../index.md 'index')
### [Pmmux\.Core](../index.md 'Pmmux\.Core')

## StateManager\<TState\> Class

Thread\-safe state machine for managing state transitions\.

```csharp
public sealed class StateManager<TState>
    where TState : struct, System.Enum, System.ValueType, System.ValueType
```
#### Type parameters

<a name='Pmmux.Core.StateManager_TState_.TState'></a>

`TState`

The enum type representing valid states\.

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; StateManager\<TState\>

| Constructors | |
| :--- | :--- |
| [StateManager\(TState\)](StateManager(TState).md 'Pmmux\.Core\.StateManager\<TState\>\.StateManager\(TState\)') | Thread\-safe state machine for managing state transitions\. |

| Methods | |
| :--- | :--- |
| [Is\(TState\)](Is(TState).md 'Pmmux\.Core\.StateManager\<TState\>\.Is\(TState\)') | Checks if the current state matches the specified state\. |
| [TryTransition\(TState, TState\[\]\)](TryTransition(TState,TState[]).md 'Pmmux\.Core\.StateManager\<TState\>\.TryTransition\(TState, TState\[\]\)') | Attempts to transition from one of the specified states to the target state\. |
