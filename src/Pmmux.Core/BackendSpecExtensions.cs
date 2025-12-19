using System;
using System.Collections.Generic;

using Pmmux.Abstractions;

namespace Pmmux.Core;

/// <summary>
/// Extensions for <see cref="BackendSpec"/> parameter matchers.
/// </summary>
public static class BackendSpecExtensions
{
    /// <summary>
    /// Gets the matchers from a backend specification.
    /// </summary>
    /// <param name="spec">The backend specification.</param>
    /// <returns>A dictionary of matchers.</returns>
    public static IReadOnlyDictionary<string, Matcher<string>> GetMatchers(this BackendSpec spec)
    {
        var matchers = new Dictionary<string, Matcher<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var (name, value) in spec.Parameters)
        {
            if (name.EndsWith('!'))
            {
                matchers.Add(name[..^1], new(Negate: true, value));
            }
            else
            {
                matchers.Add(name, new(Negate: false, value));
            }
        }

        return matchers;
    }
}
