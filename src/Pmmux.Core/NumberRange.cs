using System.Numerics;

namespace Pmmux.Core;

/// <summary>
/// A range of numbers.
/// </summary>
/// <typeparam name="T">The type of the numbers in the range.</typeparam>
/// <param name="Start">The start of the range.</param>
/// <param name="End">The end of the range.</param>
/// <remarks>
/// The range is inclusive of the start and end values.
/// </remarks>
public record NumberRange<T>(T? Start, T? End) where T : INumber<T>
{
    /// <summary>
    /// Checks if the given value is within the range.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the value is within the range; otherwise, <c>false</c>.</returns>
    public bool Contains(T value)
    {
        return value switch
        {
            _ when Start is null && End is not null => value <= End,
            _ when Start is not null && End is null => value >= Start,
            _ when Start is not null && End is not null => value >= Start && value <= End,
            _ => true
        };
    }
};
