using System;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.App;

public class InstallCommand : Command
{
    public Option<string?> ArgumentsOption { get; } = new("--arguments", "-a")
    {
        Description = "arguments to pass to the service",
    };

    public InstallCommand() : base("install", "install as a systemd service")
    {
        Add(ArgumentsOption);

        SetAction(ExecuteAsync);
    }

    private async Task ExecuteAsync(ParseResult result, CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsLinux())
        {
            throw new PlatformNotSupportedException();
        }

        var executable = Path.Combine(AppContext.BaseDirectory, "pmmux");
        var arguments = result.GetValue(ArgumentsOption);

        var unitFileContent = new StringBuilder();
        unitFileContent.AppendLine("[Unit]");
        unitFileContent.AppendLine("Description=Port map multiplexer");
        unitFileContent.AppendLine("After=network.target");
        unitFileContent.AppendLine();
        unitFileContent.AppendLine("[Service]");
        unitFileContent.AppendLine("Type=notify");
        unitFileContent.AppendLine($"ExecStart={executable} {arguments}");
        unitFileContent.AppendLine("Restart=on-failure");
        unitFileContent.AppendLine("RestartSec=5");
        unitFileContent.AppendLine("KillSignal=SIGTERM");
        unitFileContent.AppendLine();
        unitFileContent.AppendLine("[Install]");
        unitFileContent.AppendLine("WantedBy=multi-user.target");

        var unitFilePath = "/etc/systemd/system/pmmux.service";

        await File.WriteAllTextAsync(unitFilePath, unitFileContent.ToString(), cancellationToken)
            .ConfigureAwait(false);

        await Process.Start("systemctl", "daemon-reload")
            .WaitForExitAsync(cancellationToken)
            .ConfigureAwait(false);
        await Process.Start("systemctl", "start pmmux.service")
            .WaitForExitAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
