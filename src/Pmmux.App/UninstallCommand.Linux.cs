using System;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.App;

internal class UninstallCommand : Command
{
    public UninstallCommand() : base("uninstall", "remove systemd service")
    {
        SetAction(ExecuteAsync);
    }

    private async Task ExecuteAsync(ParseResult result, CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsLinux())
        {
            throw new PlatformNotSupportedException();
        }

        var unitFilePath = "/etc/systemd/system/pmmux.service";

        await Process.Start("systemctl", "stop pmmux.service")
            .WaitForExitAsync(cancellationToken)
            .ConfigureAwait(false);

        if (File.Exists(unitFilePath))
        {
            File.Delete(unitFilePath);
        }

        await Process.Start("systemctl", "daemon-reload")
            .WaitForExitAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
