using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Pmmux.App;

internal class CommandlineOptionDefaultConfigurationProvider(
    IEnumerable<Option> options,
    string rootKey,
    bool provideDefaultKeys,
    bool provideDefaultValues) : IConfigurationProvider
{
    public class Source(
        IEnumerable<Option> options,
        string rootKey,
        bool provideDefaultKeys = false,
        bool provideDefaultValues = false) : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CommandlineOptionDefaultConfigurationProvider(
                options,
                rootKey,
                provideDefaultKeys,
                provideDefaultValues);
        }
    }

    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        var hasEarlierKeys = false;

        foreach (var key in earlierKeys)
        {
            hasEarlierKeys = true;
            yield return key;
        }

        if (!provideDefaultKeys)
        {
            yield break;
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
                if (!option.HasDefaultValue)
                {
                    continue;
                }
                yield return option.Name;
            }
        }
        else if (children is [var optionName] && !hasEarlierKeys)
        {
            var option = options
                .SingleOrDefault(o => o.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));

            if (option is { HasDefaultValue: true } &&
                option.GetDefaultValue() is { } defaultValue &&
                defaultValue is not string &&
                defaultValue is IEnumerable defaultValues)
            {
                var i = 0;
                for (var enumerator = defaultValues.GetEnumerator(); enumerator.MoveNext();)
                {
                    yield return i++.ToString();
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

        if (!provideDefaultValues)
        {
            return false;
        }

        var path = key?.Split(ConfigurationPath.KeyDelimiter, StringSplitOptions.RemoveEmptyEntries);
        if (path is not [var root, .. var children] || !root.Equals(rootKey, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (children is [var singularOptionName])
        {
            var option = options
                .SingleOrDefault(o => o.Name.Equals(singularOptionName, StringComparison.OrdinalIgnoreCase));

            if (option is not { HasDefaultValue: true } || option.GetDefaultValue() is not { } defaultValue)
            {
                return false;
            }
            value = defaultValue.ToString();
            return true;
        }
        else if (children is [var arrayOptionName, var indexString] && int.TryParse(indexString, out var index))
        {
            var option = options
                .SingleOrDefault(o => o.Name.Equals(arrayOptionName, StringComparison.OrdinalIgnoreCase));

            if (option is not { HasDefaultValue: true } || option.GetDefaultValue() is not IEnumerable defaultValues)
            {
                return false;
            }

            var enumerator = defaultValues.GetEnumerator();
            for (var i = 0; i <= index; i++)
            {
                if (!enumerator.MoveNext())
                {
                    return false;
                }
            }

            value = enumerator.Current?.ToString();
            return true;
        }
        return false;
    }
}

