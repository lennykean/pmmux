using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Pmmux.App;

internal class CommandlineParserConfigurationProvider(
    ParseResult parseResult,
    IEnumerable<Option> options,
    string rootKey) : IConfigurationProvider
{
    public class Source(
        ParseResult parseResult,
        IEnumerable<Option> options,
        string rootKey) : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CommandlineParserConfigurationProvider(parseResult, options, rootKey);
        }
    }

    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        foreach (var key in earlierKeys)
        {
            yield return key;
        }

        var path = parentPath?.Split(ConfigurationPath.KeyDelimiter, StringSplitOptions.RemoveEmptyEntries);
        if (path is not [var root, .. var children] || !root.Equals(rootKey, StringComparison.OrdinalIgnoreCase))
        {
            yield break;
        }

        if (children is [])
        {
            foreach (var option in options)
            {
                if (parseResult.GetResult(option) is not { } result)
                {
                    continue;
                }
                if (result is { Tokens.Count: > 0 } ||
                    (result is { IdentifierTokenCount: > 0 } && option.ValueType == typeof(bool)))
                {
                    yield return option.Name;
                }
            }
        }
        else if (children is [var optionName])
        {
            var option = options.SingleOrDefault(o => o.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (option is { Arity.MaximumNumberOfValues: > 1 } && parseResult.GetResult(option) is { } result)
            {
                for (var i = 0; i < result.Tokens.Count; i++)
                {
                    yield return i.ToString();
                }
            }
        }
    }

    public IChangeToken GetReloadToken()
    {
        return NullChangeToken.Singleton;
    }

    public void Load()
    {
    }

    public void Set(string key, string? value)
    {
        throw new NotSupportedException();
    }

    public bool TryGet(string key, out string? value)
    {
        value = null;

        var path = key?.Split(ConfigurationPath.KeyDelimiter, StringSplitOptions.RemoveEmptyEntries);
        if (path is not [var root, .. var children] || !root.Equals(rootKey, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (children is [var singularOptionName])
        {
            var option = options
                .SingleOrDefault(o => o.Name.Equals(singularOptionName, StringComparison.OrdinalIgnoreCase));

            if (option is not null && parseResult.GetResult(option) is { } result)
            {
                if (result is { Tokens: [var token] })
                {
                    value = token.Value;
                    return true;
                }
                else if (option.ValueType == typeof(bool) && result.IdentifierToken is { })
                {
                    value = bool.TrueString;
                    return true;
                }
            }
        }
        else if (children is [var arrayOptionName, var indexString] && int.TryParse(indexString, out var index))
        {
            var option = options
                .SingleOrDefault(o => o.Name.Equals(arrayOptionName, StringComparison.OrdinalIgnoreCase));

            if (option is not null && parseResult.GetResult(option) is { } result)
            {
                var token = result.Tokens.Skip(index).FirstOrDefault();
                if (token is not null)
                {
                    value = token.Value;
                    return true;
                }
            }
        }
        return false;
    }
}

