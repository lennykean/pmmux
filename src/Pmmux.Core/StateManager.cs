using System;
using System.Threading;

namespace Pmmux.Core;

/// <summary>Thread-safe state machine for managing state transitions.</summary>
/// <typeparam name="TState">The enum type representing valid states.</typeparam>
/// <param name="initial">The initial state.</param>
public sealed class StateManager<TState>(TState initial) where TState : struct, Enum
{
    private volatile int _state = Convert.ToInt32(initial);

    /// <summary>Attempts to transition from one of the specified states to the target state.</summary>
    /// <param name="to">The target state.</param>
    /// <param name="from">The allowed source states.</param>
    /// <returns>True if transition succeeded; otherwise false.</returns>
    public bool TryTransition(TState to, params TState[] from)
    {
        var toInt = Convert.ToInt32(to);

        foreach (var fromState in from)
        {
            var fromInt = Convert.ToInt32(fromState);
            if (Interlocked.CompareExchange(ref _state, toInt, fromInt) == fromInt)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Checks if the current state matches the specified state.</summary>
    /// <param name="state">The state to check.</param>
    /// <returns>True if current state matches; otherwise false.</returns>
    public bool Is(TState state)
    {
        return _state == Convert.ToInt32(state);
    }
}
