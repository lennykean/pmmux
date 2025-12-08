using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;

namespace Pmmux.App;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {

#if WINDOWS
        if (Microsoft.Extensions.Hosting.WindowsServices.WindowsServiceHelpers.IsWindowsService())
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
        }
#endif

        var extensions = new List<IExtension>();
        var rootCommand = new MultiplexerCommand(extensions)
        {
            LoggingConfigLoader.VerbosityOption,
            LoggingConfigLoader.LogDirectoryOption,
            LoggingConfigLoader.LogFileMaxSizeOption,
            LoggingConfigLoader.LogFileMaxRetainOption,
            LoggingConfigLoader.LogMetricsOption,
            ExtensionLoader.ExtensionsOption
        };
        rootCommand.Options.Insert(0, ConfigurationLoader.ConfigurationFileOption);

        // first-pass of the command parser to load extensions
        rootCommand.TreatUnmatchedTokensAsErrors = false;

        var preParseResult = rootCommand.Parse(args);
        if (preParseResult.Errors is { Count: > 0 })
        {
            return await preParseResult.InvokeAsync();
        }

        var configurationBuilder = new ConfigurationBuilder();
        var configuration = ConfigurationLoader
            .Load(configurationBuilder, rootCommand.Options, preParseResult)
            .Build();

        var loggerFactory = LoggerFactory.Create(builder => LoggingConfigLoader.Load(configuration, builder));
        var logger = loggerFactory.CreateLogger("pmmux");

        try
        {
            var extensionLoader = new ExtensionLoader(loggerFactory.CreateLogger("extension-loader"));
            extensionLoader.Load(configuration, ref extensions);

            rootCommand.TreatUnmatchedTokensAsErrors = true;
            rootCommand.Add(new InstallCommand());
            rootCommand.Add(new UninstallCommand());

            foreach (var extension in extensions)
            {
                var commandlineBuilder = new CommandLineBuilder(rootCommand);
                extension.RegisterCommandOptions(commandlineBuilder);
            }

            // second-pass of the command parser to invoke the root command
            var parseResult = rootCommand.Parse(args);

            return await parseResult.InvokeAsync();
        }
        catch (Exception ex)
        {
            if (logger is not null)
            {
                logger.LogError(ex, "unexpected error");
            }
            else
            {
                Console.Error.WriteLine($"unexpected error: {ex.Message}");
            }
            return 1;
        }
    }
}
