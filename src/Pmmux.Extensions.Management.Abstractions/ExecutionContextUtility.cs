using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.Extensions.Management.Abstractions;

/// <summary>
/// Utility methods for working with <see cref="ExecutionContext"/>.
/// </summary>
public static class ExecutionContextUtility
{
    /// <summary>
    /// Suppresses the flow of the execution context for the duration of the specified function.
    /// </summary>
    /// <typeparam name="T">The type of the result of the function.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="clearActivity">Indicates whether to clear the activity context.</param>
    /// <returns>The result of the function.</returns>
    public static async Task<T> SuppressFlow<T>(Func<Task<T>> func, bool clearActivity = true)
    {
        Task<T> task;

        using (ExecutionContext.SuppressFlow())
        {
            if (clearActivity)
            {
                Activity.Current = null;
            }
            task = func();
        }
        return await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Suppresses the flow of the execution context for the duration of the specified async action.
    /// </summary>
    /// <param name="asyncAction">The async action to execute.</param>
    /// <param name="clearActivity">Indicates whether to clear the activity context.</param>
    public static async Task SuppressFlow(Func<Task> asyncAction, bool clearActivity = true)
    {
        Task task;

        using (ExecutionContext.SuppressFlow())
        {
            if (clearActivity)
            {
                Activity.Current = null;
            }
            task = asyncAction();
        }
        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Suppresses the flow of the execution context for the duration of the specified function.
    /// </summary>
    /// <typeparam name="T">The type of the result of the function.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="clearActivity">Indicates whether to clear the activity context.</param>
    /// <returns>The result of the function.</returns>
    public static Task<T> SuppressFlow<T>(Func<T> func, bool clearActivity = true)
    {
        return SuppressFlow(() => Task.Run(func), clearActivity);
    }

    /// <summary>
    /// Suppresses the flow of the execution context for the duration of the specified action.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="clearActivity">Indicates whether to clear the activity context.</param>
    public static Task SuppressFlow(Action action, bool clearActivity = true)
    {
        return SuppressFlow(() => Task.Run(action), clearActivity);
    }
}
