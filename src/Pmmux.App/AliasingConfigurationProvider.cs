using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Pmmux.App;

internal class AliasingConfigurationProvider(
    IConfigurationProvider innerProvider,
    IDictionary<string, string[]> aliasMap,
    string rootKey) : IConfigurationProvider
{
    public class Source(
        IConfigurationSource innerSource,
        IDictionary<string, string[]> aliasMap,
        string rootKey) : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var innerProvider = innerSource.Build(builder);
            return new AliasingConfigurationProvider(innerProvider, aliasMap, rootKey);
        }
    }

    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        var path = parentPath?.Split(ConfigurationPath.KeyDelimiter, StringSplitOptions.RemoveEmptyEntries);
        if (path is not [var root, .. var children] || !root.Equals(rootKey, StringComparison.OrdinalIgnoreCase))
        {
            return innerProvider.GetChildKeys(earlierKeys, parentPath);
        }

        var earlierKeysMaterialized = earlierKeys.ToArray();

        var childKeys = innerProvider.GetChildKeys(earlierKeysMaterialized, parentPath).ToArray();
        if (childKeys.Length > earlierKeysMaterialized.Length ||
            children is not [var keyName, .. var rest] ||
            !aliasMap.TryGetValue(keyName, out var aliases))
        {
            return childKeys;
        }
        foreach (var alias in aliases)
        {
            var aliasPath = string.Join<string>(ConfigurationPath.KeyDelimiter, [root, alias, .. rest]);

            childKeys = innerProvider.GetChildKeys(earlierKeysMaterialized, aliasPath).ToArray();

            if (childKeys.Length > earlierKeysMaterialized.Length)
            {
                break;
            }
        }
        return childKeys;
    }

    public IChangeToken GetReloadToken()
    {
        return innerProvider.GetReloadToken();
    }

    public void Load()
    {
        innerProvider.Load();
    }

    public void Set(string key, string? value)
    {
        innerProvider.Set(key, value);
    }

    public bool TryGet(string key, out string? value)
    {
        var path = key.Split(ConfigurationPath.KeyDelimiter, StringSplitOptions.RemoveEmptyEntries);
        if (path is not [var root, .. var children] || !root.Equals(rootKey, StringComparison.OrdinalIgnoreCase))
        {
            return innerProvider.TryGet(key, out value);
        }

        if (innerProvider.TryGet(key, out value))
        {
            return true;
        }

        if (children is not [var keyName, .. var rest] ||
            !aliasMap.TryGetValue(keyName, out var aliases))
        {
            value = null;
            return false;
        }

        foreach (var alias in aliases)
        {
            var aliasPath = string.Join<string>(ConfigurationPath.KeyDelimiter, [root, alias, .. rest]);

            if (innerProvider.TryGet(aliasPath, out value))
            {
                return true;
            }
        }

        value = null;
        return false;
    }

    public static IConfigurationSource Wrap(
        IConfigurationSource innerSource,
        IDictionary<string, string[]> aliasMap,
        string rootKey)
    {
        return new Source(innerSource, aliasMap, rootKey);
    }
}
