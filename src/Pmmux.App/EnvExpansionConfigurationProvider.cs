using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Pmmux.App;

internal sealed partial class EnvExpansionConfigurationProvider(IConfigurationProvider inner) : IConfigurationProvider
{
    public class Source(IConfigurationSource innerSource) : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EnvExpansionConfigurationProvider(innerSource.Build(builder));
        }
    }

    [GeneratedRegex("{{env:(.+?)}}")]
    private static partial Regex ReferencePattern { get; }

    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        return inner.GetChildKeys(earlierKeys, parentPath);
    }

    public IChangeToken GetReloadToken()
    {
        return inner.GetReloadToken();
    }

    public void Load()
    {
        inner.Load();
    }

    public void Set(string key, string? value)
    {
        inner.Set(key, value);
    }

    public bool TryGet(string key, out string? value)
    {
        if (!inner.TryGet(key, out value))
        {
            return false;
        }
        if (value is not null)
        {
            value = ReferencePattern.Replace(value, match =>
            {
                var envVarName = match.Groups[1].Value;
                return Environment.GetEnvironmentVariable(envVarName) ?? string.Empty;
            });
        }
        return true;
    }

    public static IConfigurationSource Wrap(IConfigurationSource innerSource)
    {
        return new Source(innerSource);
    }
}
