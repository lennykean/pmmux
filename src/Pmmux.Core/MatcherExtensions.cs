using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Pmmux.Core;

/// <summary>
/// Extensions for <see cref="Matcher{T}"/>.
/// </summary>
public static class MatcherExtensions
{
    /// <summary>
    /// Parses a string matcher into an array of string matchers.
    /// </summary>
    /// <param name="matcher">The matcher to parse.</param>
    /// <returns>A list of string matchers.</returns>
    public static IEnumerable<Matcher<string>> AsMultiValue(this Matcher<string> matcher)
    {
        return [..
            from value in matcher.Value.Split(';', StringSplitOptions.RemoveEmptyEntries)
            select new Matcher<string>(matcher.Negate, value)];
    }

    /// <summary>
    /// Gets the indexer-based matchers from a dictionary of matchers.
    /// </summary>
    /// <param name="matchers">The dictionary of matchers.</param>
    /// <param name="indexer">The indexer to use.</param>
    /// <returns>A dictionary of indexable matchers.</returns>
    public static IReadOnlyDictionary<string, Matcher<string>> AsIndexed(
        this IReadOnlyDictionary<string, Matcher<string>> matchers,
        string indexer)
    {
        var indexableMatchers = new Dictionary<string, Matcher<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var (name, matcher) in matchers)
        {
            if (name.StartsWith($"{indexer}[") && name.EndsWith(']'))
            {
                indexableMatchers.Add(name[(indexer.Length + 1)..^1], matcher);
            }
        }

        return indexableMatchers;
    }

    /// <summary>
    /// Gets the indexer-based multi-value matchers from a dictionary of matchers.
    /// </summary>
    /// <param name="matchers">The dictionary of matchers.</param>
    /// <param name="indexer">The indexer to use.</param>
    /// <returns>A dictionary of indexable matchers.</returns>
    public static IReadOnlyDictionary<string, IEnumerable<Matcher<string>>> AsMultiValueIndexed(
        this IReadOnlyDictionary<string, Matcher<string>> matchers,
        string indexer)
    {
        var indexableMatchers = new Dictionary<string, IEnumerable<Matcher<string>>>(StringComparer.OrdinalIgnoreCase);

        foreach (var (name, matcher) in matchers)
        {
            if (name.StartsWith($"{indexer}[") && name.EndsWith(']'))
            {
                indexableMatchers.Add(name[(indexer.Length + 1)..^1], matcher.AsMultiValue());
            }
        }

        return indexableMatchers;
    }

    /// <summary>
    /// Parses a string matcher into a regex matcher.
    /// </summary>
    /// <param name="matcher">The matcher to parse.</param>
    /// <returns>A regex matcher.</returns>
    public static Matcher<Regex> AsRegex(this Matcher<string> matcher)
    {
        return new(matcher.Negate, new($"^(?:{matcher.Value.TrimStart('^').TrimEnd('$')})$", RegexOptions.Compiled));
    }

    /// <summary>
    /// Parse a collection of string matchers into regex matchers.
    /// </summary>
    public static IEnumerable<Matcher<Regex>> AsRegex(this IEnumerable<Matcher<string>> matchers)
    {
        return matchers.Select(m => m.AsRegex());
    }

    /// <summary>
    /// Parse a dictionary of string matchers into regex matchers.
    /// </summary>
    public static IReadOnlyDictionary<string, Matcher<Regex>> AsRegex(
        this IReadOnlyDictionary<string,
        Matcher<string>> matchers)
    {
        return matchers.ToDictionary(
            kv => kv.Key,
            kv => kv.Value.AsRegex(),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Parses a string matcher into a CIDR matcher.
    /// </summary>
    /// <param name="matcher">The matcher to parse.</param>
    /// <returns>A CIDR matcher.</returns>
    public static Matcher<IPNetwork> AsCidr(this Matcher<string> matcher)
    {
        return new(matcher.Negate, IPNetwork.Parse(matcher.Value));
    }

    /// <summary>
    /// Parse a collection of string matchers into CIDR matchers.
    /// </summary>
    public static IEnumerable<Matcher<IPNetwork>> AsCidr(this IEnumerable<Matcher<string>> matchers)
    {
        return matchers.Select(m => m.AsCidr());
    }

    /// <summary>
    /// Parse a dictionary of string matchers into CIDR matchers.
    /// </summary>
    public static IReadOnlyDictionary<string, Matcher<IPNetwork>> AsCidr(
        this IReadOnlyDictionary<string, Matcher<string>> matchers)
    {
        return matchers.ToDictionary(
            kv => kv.Key,
            kv => kv.Value.AsCidr(),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Parses a string matcher into a numeric range matcher.
    /// </summary>
    /// <typeparam name="T">The type of the numbers in the range.</typeparam>
    /// <param name="matcher">The matcher to parse.</param>
    /// <returns>A number range matcher.</returns>
    public static Matcher<NumberRange<T>> AsNumeric<T>(this Matcher<string> matcher) where T : INumber<T>
    {
        var (start, end) = matcher.Value.Split('-') switch
        {
            [var startString, var endString] when
                T.TryParse(startString, NumberStyles.None, null, out var startValue) &&
                T.TryParse(endString, NumberStyles.None, null, out var endValue) => (startValue, endValue),
            [var valueString] when T.TryParse(valueString, NumberStyles.None, null, out var value) => (value, value),
            [..] => throw new ArgumentException($"invalid number range {matcher.Value}")
        };
        return new(matcher.Negate, new(start, end));
    }

    /// <summary>
    /// Parse a collection of string matchers into numeric range matchers.
    /// </summary>
    public static IEnumerable<Matcher<NumberRange<T>>> AsNumeric<T>(
        this IEnumerable<Matcher<string>> matchers) where T : INumber<T>
    {
        return matchers.Select(m => m.AsNumeric<T>());
    }

    /// <summary>
    /// Parse a dictionary of string matchers into numeric range matchers.
    /// </summary>
    public static IReadOnlyDictionary<string, Matcher<NumberRange<T>>> AsNumeric<T>(
        this IReadOnlyDictionary<string, Matcher<string>> matchers) where T : INumber<T>
    {
        return matchers.ToDictionary(
            kv => kv.Key,
            kv => kv.Value.AsNumeric<T>(),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Check if the matcher matches using the provided comparer.
    /// </summary>
    /// <typeparam name="T">The type of the matcher value.</typeparam>
    /// <param name="matchers">The matcher to check.</param>
    /// <param name="comparer">The comparison function.</param>
    /// <returns><c>true</c> if the matcher passes; otherwise, <c>false</c>.</returns>
    public static bool IsMatch<T>(this Matcher<T> matchers, Func<T, bool> comparer)
    {
        return comparer(matchers.Value) != matchers.Negate;
    }

    /// <summary>
    /// Check if any matcher in the collection matches using the provided comparer.
    /// </summary>
    /// <remarks>
    /// Matchers match if any value matches (logical OR).
    /// Negated matchers match only if none of the values match (logical AND).
    /// Empty matcher collection always returns <c>true</c> (no constraints to fail).
    /// </remarks>
    /// <typeparam name="T">The type of the matcher value.</typeparam>
    /// <param name="matchers">The matchers to check.</param>
    /// <param name="comparer">The comparison function.</param>
    /// <returns><c>true</c> if the matcher collection passes; otherwise, <c>false</c>.</returns>
    public static bool HasMatch<T>(this IEnumerable<Matcher<T>> matchers, Func<T, bool> comparer)
    {
        if (matchers.All(m => m.Negate))
        {
            return matchers.All(m => !comparer(m.Value));
        }
        return matchers.Any(m => m.IsMatch(comparer));
    }

    /// <summary>
    /// Check if the indexed matcher matches using the provided comparer.
    /// </summary>
    /// <typeparam name="T">The type of the matcher value.</typeparam>
    /// <param name="matchers">The dictionary of matchers.</param>
    /// <param name="indexer">The key to look up.</param>
    /// <param name="comparer">The comparison function.</param>
    /// <returns><c>true</c> if the key exists and the matcher passes; otherwise, <c>false</c>.</returns>
    public static bool HasMatch<T>(
        this IReadOnlyDictionary<string, Matcher<T>> matchers,
        string indexer,
        Func<T, bool> comparer)
    {
        if (matchers.TryGetValue(indexer, out var matcher))
        {
            return matcher.IsMatch(comparer);
        }
        return false;
    }

    /// <summary>
    /// Check if any matcher at the indexed key matches using the provided comparer.
    /// </summary>
    /// <typeparam name="T">The type of the matcher value.</typeparam>
    /// <param name="matchers">The dictionary of matcher collections.</param>
    /// <param name="indexer">The key to look up.</param>
    /// <param name="comparer">The comparison function.</param>
    /// <returns><c>true</c> if the key exists and any matcher passes; otherwise, <c>false</c>.</returns>
    public static bool HasMatch<T>(
        this IReadOnlyDictionary<string, IEnumerable<Matcher<T>>> matchers,
        string indexer,
        Func<T, bool> comparer)
    {
        if (matchers.TryGetValue(indexer, out var indexedMatchers))
        {
            return indexedMatchers.HasMatch(comparer);
        }
        return false;
    }
}
