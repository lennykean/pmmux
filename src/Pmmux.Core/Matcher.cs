namespace Pmmux.Core;

/// <summary>
/// A matcher for a backend parameter value.
/// </summary>
/// <typeparam name="T">The type of the value to match.</typeparam>
/// <param name="Negate">Whether to negate the matcher.</param>
/// <param name="Value">The value to match.</param>
public record Matcher<T>(bool Negate, T Value);
