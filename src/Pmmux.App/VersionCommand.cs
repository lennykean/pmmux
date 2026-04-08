using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;

using Pmmux.Abstractions;

namespace Pmmux.App;

internal sealed class VersionCommand : Command
{
    private readonly IEnumerable<IExtension> _extensions;

    public VersionCommand(IEnumerable<IExtension> extensions) : base("--version", "display version information")
    {
        _extensions = extensions;

        SetAction(Execute);
    }

    private void Execute(ParseResult result)
    {
        var assembly = typeof(Program).Assembly;
        var version = GetVersion(assembly);

        Console.WriteLine($"pmmux {version}");

        var extensionsDir = Path.Combine(AppContext.BaseDirectory, "extensions");

        // Assembly.Location is valid here: extensions are loaded from disk, not embedded in single-file
#pragma warning disable IL3000
        var loadedAssemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var loadedExtensions = (
            from ext in _extensions
            let asm = ext.GetType().Assembly
            let loc = asm.Location
            where loadedAssemblies.Add(loc)
            let extVersion = GetVersion(asm)
            select new { Version = extVersion, Location = loc }).ToList();
#pragma warning restore IL3000

        var installedExtensions = !Directory.Exists(extensionsDir) ? [] : (
            from dll in Directory.GetFiles(extensionsDir, "*.dll")
            where !loadedAssemblies.Contains(Path.GetFullPath(dll))
            let asm = TryLoadAssembly(dll)
            where asm is not null
            where HasExtensionType(asm)
            let extVersion = GetVersion(asm)
            select new { Version = extVersion, Location = dll }).ToList();

        if (loadedExtensions.Count == 0 && installedExtensions.Count == 0)
        {
            return;
        }

        Console.WriteLine("extensions:");
        foreach (var ext in loadedExtensions.Concat(installedExtensions).OrderBy(e => e.Location))
        {
            Console.WriteLine($"  {ext.Location} {ext.Version}");
        }
    }

    private static string GetVersion(Assembly assembly)
    {
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
            ?? assembly.GetName().Version?.ToString()
            ?? "unknown";

        var metadataIndex = version.IndexOf('+');
        if (metadataIndex >= 0)
        {
            version = version[..metadataIndex];
        }

        return version;
    }

    private static Assembly? TryLoadAssembly(string path)
    {
        try
        {
            return Assembly.LoadFrom(path);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool HasExtensionType(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes().Any(t =>
                typeof(IExtension).IsAssignableFrom(t) && t.IsPublic && !t.IsInterface && !t.IsAbstract);
        }
        catch (ReflectionTypeLoadException)
        {
            return false;
        }
    }
}
