using Microsoft.Extensions.Logging;

namespace Pmmux.Extensions.Management;

internal class ProxyLoggerProvider(ILoggerFactory loggerFactory) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return loggerFactory.CreateLogger(categoryName);
    }

    public void Dispose()
    {
    }
}
