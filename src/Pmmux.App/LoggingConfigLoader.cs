using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NetEscapades.Extensions.Logging.RollingFile.Formatters;

using Pmmux.Abstractions;
using Pmmux.Core;

namespace Pmmux.App;

internal class LoggingConfigLoader
{
    public static Option<LogLevel> VerbosityOption { get; } = new("--verbosity", "-v")
    {
        Description = "logging verbosity",
        DefaultValueFactory = _ => LogLevel.Information,
        Recursive = true
    };

    public static Option<string> LogDirectoryOption { get; } = new("--log-directory", "-d")
    {
        Description = "log directory",
        Recursive = true
    };

    public static Option<int?> LogFileMaxSizeOption { get; } = new("--log-file-max-size")
    {
        Description = "maximum log file size in bytes",
        Recursive = true
    };

    public static Option<int?> LogFileMaxRetainOption { get; } = new("--log-file-max-retain")
    {
        Description = "maximum number of log files to retain",
        Recursive = true
    };

    public static Option<bool> LogMetricsOption { get; } = new("--log-metrics")
    {
        Description = "report metrics in the logs",
        Recursive = true
    };

    public static ILoggingBuilder Load(IConfiguration configuration, ILoggingBuilder builder)
    {
        var section = configuration.GetSection("pmmux");
        var verbosity = section.GetValue<LogLevel>(VerbosityOption.Name);
        var logDirectory = section.GetValue<string?>(LogDirectoryOption.Name);
        var logFileMaxSize = section.GetValue<int?>(LogFileMaxSizeOption.Name);
        var logFileMaxRetain = section.GetValue<int?>(LogFileMaxRetainOption.Name);
        var logMetrics = section.GetValue<bool>(LogMetricsOption.Name);

        builder
            .AddFilter("Microsoft.Hosting", LogLevel.Warning)
            .AddFilter("Microsoft.Extensions.Hosting", LogLevel.Warning)
            .SetMinimumLevel(verbosity)
#if LINUX
            .AddSystemdConsole(logger =>
            {
#else
            .AddSimpleConsole(logger =>
            {
                logger.SingleLine = true;
#endif
                logger.TimestampFormat = " [yyyy-MM-ddTHH:mm:ss.fffZ] ";
                logger.UseUtcTimestamp = true;
                logger.IncludeScopes = true;
            });

        if (logMetrics)
        {
            builder.Services.AddSingleton<IMetricSink, LoggerMetricSink>();
        }
        if (logDirectory is not null)
        {
            builder.Services.AddSingleton<ILogFormatter, LogFormatter>();
            builder.AddFile(logger =>
            {
                logger.LogDirectory = logDirectory;
                logger.IncludeScopes = true;
                logger.FormatterName = "pmmux";
                logger.FileName = "pmmux.";
                logger.Extension = "log";
                logger.FileSizeLimit = logFileMaxSize;
                logger.RetainedFileCountLimit = logFileMaxRetain;
            });
        }
        return builder;
    }
}
