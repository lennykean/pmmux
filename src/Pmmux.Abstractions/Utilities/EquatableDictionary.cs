using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pmmux.Abstractions.Utilities;

/// <summary>
/// A read-only dictionary that provides value-based equality comparison.
/// </summary>
public sealed class EquatableDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
    : ReadOnlyDictionary<TKey, TValue>(dictionary) where TKey : IComparable<TKey>
{
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not IDictionary<TKey, TValue> other)
        {
            return false;
        }

        if (Count != other.Count)
        {
            return false;
        }

        foreach (var key in Keys)
        {
            if (!other.TryGetValue(key, out var otherValue) ||
                !EqualityComparer<TValue>.Default.Equals(this[key], otherValue))
            {
                return false;
            }
        }
        return true;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach (var key in Keys.OrderBy(k => k))
        {
            hashCode.Add(key);
            hashCode.Add(this[key]);
        }
        return hashCode.ToHashCode();
    }
}
