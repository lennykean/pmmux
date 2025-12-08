using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;

using Alexinea.Extensions.Configuration.Toml;
using Alexinea.Extensions.Configuration.Yaml;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace Pmmux.App;

internal static class ConfigurationLoader
{
    public const string ROOT_KEY = "pmmux";

    public static Option<string> ConfigurationFileOption { get; } = new("--config-file", "-c")
    {
        Description = "load configuration from a file (toml, yaml or json)",
        DefaultValueFactory = _ => "pmmux.toml"
    };

    public static IConfigurationBuilder Load(
        IConfigurationBuilder builder,
        IEnumerable<Option> options,
        ParseResult parseResult)
    {
        var fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        var aliasMap = new Dictionary<string, string[]>(
            options
                .Select(o => new { o.Name, Aliases = o.Aliases.Append(o.Name) })
                .ToDictionary(
                    k => k.Name,
                    v => v.Aliases.Union(v.Aliases.Select(a => a.TrimStart('-').TrimStart('-'))).ToArray()),
            StringComparer.OrdinalIgnoreCase);

        var configFile = parseResult.GetValue(ConfigurationFileOption) ?? "pmmux.toml";

        builder.Sources.Clear();
        builder.Sources.Add(AliasingConfigurationProvider.Wrap(
            new CommandlineOptionDefaultConfigurationProvider.Source(
                options,
                ROOT_KEY,
                provideDefaultKeys: false,
                provideDefaultValues: true),
            aliasMap,
            rootKey: ROOT_KEY));

        switch (Path.GetExtension(configFile))
        {
            case ".json":
                builder.Sources.Add(AliasingConfigurationProvider.Wrap(
                    new JsonConfigurationSource() { Path = configFile, Optional = true, FileProvider = fileProvider },
                    aliasMap,
                    rootKey: ROOT_KEY));
                break;
            case ".yaml" or ".yml":
                builder.Sources.Add(AliasingConfigurationProvider.Wrap(
                    new YamlConfigurationSource() { Path = configFile, Optional = true, FileProvider = fileProvider },
                    aliasMap,
                    rootKey: ROOT_KEY));
                break;
            case ".toml":
                builder.Sources.Add(AliasingConfigurationProvider.Wrap(
                    new TomlConfigurationSource() { Path = configFile, Optional = true, FileProvider = fileProvider },
                    aliasMap,
                    rootKey: ROOT_KEY));
                break;
        }

        builder.Sources.Add(AliasingConfigurationProvider.Wrap(
            new EnvironmentVariablesConfigurationSource(),
            aliasMap,
            rootKey: ROOT_KEY));
        builder.Sources.Add(AliasingConfigurationProvider.Wrap(
            new CommandlineParserConfigurationProvider.Source(parseResult, options, ROOT_KEY),
            aliasMap,
            rootKey: ROOT_KEY));
        builder.Sources.Add(AliasingConfigurationProvider.Wrap(
            new CommandlineOptionDefaultConfigurationProvider.Source(
                options,
                ROOT_KEY,
                provideDefaultKeys: true,
                provideDefaultValues: false),
            aliasMap,
            rootKey: ROOT_KEY));

        return builder;
    }
}
