using System;
using System.CommandLine;

namespace Pmmux.App;

internal sealed class InstallCommand : Command
{
    public InstallCommand() : base("install", "install as a service")
    {
        SetAction(Execute);
    }

    private static void Execute(ParseResult result)
    {
        throw new PlatformNotSupportedException("service installation is not supported on macOS");
    }
}
