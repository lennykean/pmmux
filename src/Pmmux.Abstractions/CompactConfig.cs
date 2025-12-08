using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pmmux.Abstractions;

/// <summary>
/// Utility for parsing compact configuration strings.
/// </summary>
/// <remarks>
/// <para>
/// Format: segments separated by colons, where each segment is either an identifier
/// or comma-delimited key=value properties. Values with special characters can be quoted.
/// </para>
/// </remarks>
/// <example>
/// Parsing a backend configuration string:
/// <code>
/// var input = "web:pass:ip=127.0.0.1,port=3000";
/// var segments = CompactConfig.Parse(input);
/// // Returns: IdentifierSegment("web"), IdentifierSegment("pass"), PropertiesSegment({ ip=127.0.0.1, port=3000 })
/// </code>
/// </example>
public abstract record CompactConfig
{
    /// <summary>
    /// Base type for segments.
    /// </summary>
    public record Segment;

    /// <summary>
    /// Identifier segment.
    /// </summary>
    /// <param name="Name">The identifier name.</param>
    public record IdentifierSegment(string Name) : Segment;

    /// <summary>
    /// Properties segment (key-value pairs) in the configuration.
    /// </summary>
    /// <param name="Properties">The properties dictionary.</param>
    public record PropertiesSegment(Dictionary<string, string> Properties) : Segment;

    /// <summary>
    /// Parse compact configuration string into a sequence of segments.
    /// </summary>
    /// <param name="compactConfig">The compact configuration string to parse.</param>
    /// <returns>Enumerable of <see cref="Segment"/> objects representing the parsed configuration.</returns>
    public static IEnumerable<Segment> Parse(string compactConfig)
    {
        foreach (var (segment, isProperties) in Tokenize(compactConfig, ':', subDelimiter: '='))
        {
            if (isProperties)
            {
                yield return new PropertiesSegment(new(ParseProperties(segment)));
            }
            else
            {
                yield return new IdentifierSegment(Unescape(Unquote(segment.Trim())));
            }
        }
    }

    private static IEnumerable<KeyValuePair<string, string>> ParseProperties(string segment)
    {
        var properties = Tokenize(segment, ',');

        foreach (var (property, _) in properties)
        {
            yield return Tokenize(property, '=').ToArray() switch
            {
                [var (key, _)] =>
                    new(Unescape(Unquote(key.Trim())), string.Empty),
                [var (key, _), var (value, _)] =>
                    new(Unescape(Unquote(key.Trim())), Unescape(Unquote(value.Trim()))),
                _ =>
                    throw new ArgumentException($"invalid property specifier: {property}", nameof(segment))
            };
        }
    }

    private static IEnumerable<(string Segment, bool HasSubDelimiter)> Tokenize(
        string input,
        char delimiter,
        char? subDelimiter = null)
    {
        var segment = new StringBuilder();

        var escaped = false;
        var inQuotes = false;
        var hasSubDelimiter = false;

        for (int i = 0; i < input.Length; i++)
        {
            if (escaped)
            {
                segment.Append(input[i]);
                escaped = false;
                continue;
            }
            switch (input[i])
            {
                case '\\' when inQuotes:
                    escaped = true;
                    segment.Append('\\');
                    break;
                case '"':
                    inQuotes = !inQuotes;
                    segment.Append('"');
                    break;
                default:
                    if (input[i] == delimiter && !inQuotes)
                    {
                        yield return (segment.ToString(), hasSubDelimiter);

                        hasSubDelimiter = false;
                        segment.Clear();
                    }
                    else
                    {
                        if (input[i] == subDelimiter)
                        {
                            hasSubDelimiter = true;
                        }
                        segment.Append(input[i]);
                    }
                    break;
            }
        }
        if (inQuotes)
        {
            throw new ArgumentException($"unterminated quote in config \"{segment}\"", nameof(input));
        }
        if (escaped)
        {
            throw new ArgumentException($"unterminated escape sequence in config \"{segment}\"", nameof(input));
        }
        if (segment.Length > 0)
        {
            yield return (segment.ToString(), hasSubDelimiter);
        }
    }

    private static string Unquote(string input)
    {
        if (input.StartsWith('"') && input.EndsWith('"') && input.Length >= 2)
        {
            return input[1..^1];
        }
        return input;
    }

    private static string Unescape(string input)
    {
        return input.Replace("\\\"", "\"");
    }
}
