using System.Text;

using Microsoft.Extensions.Logging;

using NetEscapades.Extensions.Logging.RollingFile.Formatters;

namespace Pmmux.App;

internal class LogFormatter : ILogFormatter
{
    public string Name => "pmmux";

    public void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, StringBuilder builder)
    {
        builder.Append(logEntry.Timestamp.ToUniversalTime().ToString("[yyyy-MM-ddTHH:mm:ss.fffZ]"));
        builder.Append(logEntry.LogLevel switch
        {
            LogLevel.Trace => " trce: ",
            LogLevel.Debug => " dbug: ",
            LogLevel.Information => " info: ",
            LogLevel.Warning => " warn: ",
            LogLevel.Error => " fail: ",
            LogLevel.Critical => " crit: ",
            _ => " : ",
        });
        builder.Append(logEntry.Category);

        if (scopeProvider != null)
        {
            scopeProvider.ForEachScope((scope, stringBuilder) => stringBuilder.Append(" => ").Append(scope), builder);
            builder.Append(':');
        }
        else
        {
            builder.Append(": ");
        }

        builder.AppendLine(logEntry.Formatter(logEntry.State, logEntry.Exception));

        if (logEntry.Exception != null)
        {
            builder.AppendLine(logEntry.Exception.ToString());
        }
    }
}
