using System;
using System.CommandLine;

namespace Pmmux.App;

internal sealed class UninstallCommand : Command
{
    public UninstallCommand() : base("uninstall", "remove service")
    {
        SetAction(Execute);
    }

    private static void Execute(ParseResult result)
    {
        throw new PlatformNotSupportedException("service uninstallation is not supported on macOS");
    }
}
