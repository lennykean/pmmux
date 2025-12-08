using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Pmmux.Extensions.Http;

internal static class StreamParser
{
    private struct HeaderLocation
    {
        public bool Found;
        public long Position;
        public int Length;
    }

    public static bool? TryParseHttp(
        ReadOnlySequence<byte> data,
        [NotNullWhen(true)] out string? method,
        [NotNullWhen(true)] out string? resourceTarget,
        [NotNullWhen(true)] out string? version)
    {
        method = null;
        resourceTarget = null;
        version = null;

        // Extract the http method segment
        var separator = data.PositionOf((byte)' ');
        if (separator is null)
        {
            return null;
        }

        // Parse the http method
        var methodSegment = data.Slice(0, separator.Value);
        var methodBuilder = new StringBuilder();
        foreach (var chunk in methodSegment)
        {
            var memory = chunk.Span;

            for (var i = 0; i < memory.Length; i++)
            {
                if (!IsValidMethodChar(memory[i]))
                {
                    return false;
                }
                methodBuilder.Append((char)memory[i]);
            }
        }

        // Extract the resource target segment
        data = data.Slice(data.GetPosition(1, separator.Value));
        separator = data.PositionOf((byte)' ');
        if (separator is null)
        {
            return null;
        }

        // Parse the resource target
        var resourceTargetSegment = data.Slice(0, separator.Value);
        var resourceTargetBuilder = new StringBuilder();
        foreach (var chunk in resourceTargetSegment)
        {
            var memory = chunk.Span;

            for (var i = 0; i < memory.Length; i++)
            {
                if (!IsValidPathChar(memory[i]))
                {
                    return false;
                }
                resourceTargetBuilder.Append((char)memory[i]);
            }
        }

        // Extract the http version segment
        data = data.Slice(data.GetPosition(1, separator.Value));

        // Ensure the http version prefix is present
        var versionPrefix = "HTTP/"u8;
        var versionPrefixIndex = 0;
        foreach (var chunk in data)
        {
            var memory = chunk.Span;

            for (var i = 0; i < memory.Length && versionPrefixIndex < versionPrefix.Length; i++)
            {
                if (memory[i] != versionPrefix[versionPrefixIndex++])
                {
                    return false;
                }
            }
        }
        if (versionPrefixIndex < versionPrefix.Length)
        {
            return null;
        }

        // Ensure the CRLF marker is present
        var lf = data.PositionOf((byte)'\n');
        if (lf is null)
        {
            return null;
        }
        data = data.Slice(0, lf.Value);
        var cr = data.GetPosition(data.Length - 1);
        if (data.Slice(cr, 1).FirstSpan[0] != 0x0d)
        {
            return false;
        }

        // Extract the http version segment
        var versionSegment = data.Slice(versionPrefix.Length, cr);
        if (versionSegment.Length == 0)
        {
            return false;
        }

        // Parse the http version
        var versionBuilder = new StringBuilder("HTTP/");
        foreach (var chunk in versionSegment)
        {
            var memory = chunk.Span;
            for (var i = 0; i < memory.Length; i++)
            {
                if (memory[i] != 0x2e && (memory[i] < 0x30 || memory[i] > 0x39))
                {
                    return false;
                }
                versionBuilder.Append((char)memory[i]);
            }
        }
        method = methodBuilder.ToString();
        resourceTarget = resourceTargetBuilder.ToString();
        version = versionBuilder.ToString();

        return true;
    }

    public static bool IsValidMethodChar(byte value)
    {
        switch (value)
        {
            case 0x21: // !
            case >= 0x23 and <= 0x27: // # $ % & '
            case >= 0x2a and <= 0x2e: // * + , - .
            case >= 0x30 and <= 0x39: // 0-9
            case >= 0x41 and <= 0x5a: // A-Z
            case >= 0x5e and <= 0x7a: // ^ _ ` a-z
            case 0x7c: // |
            case 0x7e: // ~
                return true;
            default:
                return false;
        }
    }

    public static bool IsValidPathChar(byte value)
    {
        switch (value)
        {
            case 0x21: // !
            case >= 0x23 and <= 0x2F: // # $ % & ' ( ) * + , - . /
            case >= 0x30 and <= 0x39: // 0-9
            case >= 0x3A and <= 0x3B: // : ;
            case 0x3D: // =
            case >= 0x3F and <= 0x5D: // ? @ A-Z [ \ ] _ a-z ~
            case 0x5F: // _
            case >= 0x61 and <= 0x7A: // a-z
            case 0x7E: // ~
                return true;
            default:
                return false;
        }
    }

    public static bool? TryExtractHttpHeaders(
        ReadOnlySequence<byte> data,
        ReadOnlySpan<string> requiredHeaders,
        [MaybeNullWhen(false)] out Dictionary<string, string>? headers,
        bool includePartial = false)
    {
        switch (TryParseHttp(data, out _, out _, out _))
        {
            case null:
                headers = null;
                return null;
            case false:
                headers = null;
                return false;
        }

        var httpEnd = data.PositionOf((byte)'\n');
        if (httpEnd is null)
        {
            headers = null;
            return null;
        }
        var section = data.Slice(data.GetPosition(offset: 1, origin: httpEnd.Value));

        Span<HeaderLocation> headerLocations = stackalloc HeaderLocation[requiredHeaders.Length];
        var found = 0;

        while (true)
        {
            var colon = section.PositionOf((byte)':');
            var lf = section.PositionOf((byte)'\n');

            if (lf is null)
            {
                headers = null;
                return null;
            }
            if (section.GetOffset(lf.Value) <= 1)
            {
                if (!includePartial && found < requiredHeaders.Length)
                {
                    headers = null;
                    return false;
                }
                break;
            }
            if (colon is null || section.GetOffset(colon.Value) > section.GetOffset(lf.Value))
            {
                headers = null;
                return false;
            }

            var headerNameSegment = section.Slice(start: 0, end: colon.Value);
            var headerValueStart = section.GetPosition(offset: 1, origin: colon.Value);
            var headerValueSlice = section.Slice(start: headerValueStart, end: lf.Value);

            for (int i = 0; i < requiredHeaders.Length; i++)
            {
                if (!headerLocations[i].Found && CaseInsensitiveEquals(headerNameSegment, requiredHeaders[i]))
                {
                    headerLocations[i].Found = true;
                    headerLocations[i].Position = data.GetOffset(headerValueSlice.Start);
                    headerLocations[i].Length = (int)headerValueSlice.Length;
                    found++;
                }
            }
            if (found == requiredHeaders.Length)
            {
                break;
            }
            section = section.Slice(section.GetPosition(offset: 1, origin: lf.Value));
        }

        headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < requiredHeaders.Length; i++)
        {
            if (!headerLocations[i].Found)
            {
                continue;
            }
            var headerValue = data.Slice(
                headerLocations[i].Position,
                headerLocations[i].Length);

            headers[requiredHeaders[i]] = headerValue.IsSingleSegment
                ? Encoding.UTF8.GetString(headerValue.FirstSpan).Trim()
                : Encoding.UTF8.GetString(headerValue.ToArray()).Trim();
        }
        return headers.Count == requiredHeaders.Length;
    }

    private static bool CaseInsensitiveEquals(ReadOnlySequence<byte> sequence, string value)
    {
        while (sequence.Length > 0 && sequence.FirstSpan[0] is 0x20 or 0x09)
        {
            sequence = sequence.Slice(start: 1);
        }
        while (sequence.Length > 0 && sequence.Slice(sequence.Length - 1).FirstSpan[0] is 0x20 or 0x09 or 0x0d)
        {
            sequence = sequence.Slice(start: 0, length: (int)sequence.Length - 1);
        }

        if (sequence.Length != value.Length)
        {
            return false;
        }
        var iSequence = 0;

        foreach (var segment in sequence)
        {
            for (int i = 0; i < segment.Span.Length; i++, iSequence++)
            {
                var sequenceChar = segment.Span[i] is >= 0x41 and <= 0x5a ? segment.Span[i] + 32 : segment.Span[i];
                var valueChar = value[iSequence] is >= 'A' and <= 'Z' ? value[iSequence] + 32 : value[iSequence];
                if (sequenceChar != valueChar)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
