using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;

namespace Pmmux.App;

internal class ExtensionLoader(ILogger? logger = null)
{
    public static Option<FileInfo[]> ExtensionsOption { get; } = new("--extension", "-x", "--extensions")
    {
        Description = "file path of extension assembly(s) to load",
        Arity = ArgumentArity.ZeroOrMore
    };

    public void Load(IConfiguration configuration, ref List<IExtension> extensions)
    {
        var root = configuration.GetSection(ConfigurationLoader.ROOT_KEY);
        if (root is null)
        {
            return;
        }

        var extensionPaths = root
            .GetSection(ExtensionsOption.Name)
            .GetChildren()
            .Select(section => new FileInfo(section.Value ?? string.Empty));

        logger?.LogTrace("loading {AssemblyCount} extension assembly(s)", extensionPaths.Count());

        foreach (var extensionPath in extensionPaths)
        {
            if (!extensionPath.Exists)
            {
                throw new FileNotFoundException("unable to load extension", extensionPath.FullName);
            }

            logger?.LogTrace("loading extension assembly from {AssemblyPath}", extensionPath.FullName);

            var assembly = Assembly.LoadFrom(extensionPath.FullName);
            var types = (
                from type in assembly.GetTypes()
                where typeof(IExtension).IsAssignableFrom(type)
                where type.IsPublic
                where !type.IsInterface
                where !type.IsAbstract
                where type.GetConstructor([]) is not null
                select type).ToList();

            if (types is { Count: 0 })
            {
                logger?.LogWarning("no extensions found in {AssemblyName}", assembly.FullName);
                continue;
            }

            logger?.LogTrace("found {ExtensionCount} extension(s) in {AssemblyName}", types.Count, assembly.FullName);

            foreach (var type in types)
            {
                logger?.LogTrace("loading extension {Extension}", type.FullName);

                var extension = CreateExtensionInstance(type);

                extensions.Add(extension);
            }
        }
    }

    private static IExtension CreateExtensionInstance(Type type)
    {
        if (Activator.CreateInstance(type) is not IExtension instance)
        {
            throw new InvalidOperationException($"unable to create instance of type {type.FullName}");
        }

        return instance;
    }
}
