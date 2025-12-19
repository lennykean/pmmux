using System;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.App;

internal sealed class UninstallCommand : Command
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

        await ShellUtility.ExecAsync("systemctl", "stop pmmux.service", cancellationToken).ConfigureAwait(false);
        await ShellUtility.ExecAsync("systemctl", "disable pmmux.service", cancellationToken).ConfigureAwait(false);

        if (File.Exists(unitFilePath))
        {
            File.Delete(unitFilePath);
        }

        await ShellUtility.ExecAsync("systemctl", "daemon-reload", cancellationToken).ConfigureAwait(false);
    }
}
