using System;
using System.CommandLine;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.App;

internal sealed class InstallCommand : Command
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
        var execStart = string.IsNullOrEmpty(arguments) ? executable : $"{executable} {arguments}";

        var unitFileContent = new StringBuilder();
        unitFileContent.AppendLine("[Unit]");
        unitFileContent.AppendLine("Description=Port map multiplexer");
        unitFileContent.AppendLine("After=network.target");
        unitFileContent.AppendLine();
        unitFileContent.AppendLine("[Service]");
        unitFileContent.AppendLine("Type=notify");
        unitFileContent.AppendLine($"ExecStart={execStart}");
        unitFileContent.AppendLine("Restart=on-failure");
        unitFileContent.AppendLine("RestartSec=5");
        unitFileContent.AppendLine("KillSignal=SIGTERM");
        unitFileContent.AppendLine();
        unitFileContent.AppendLine("[Install]");
        unitFileContent.AppendLine("WantedBy=multi-user.target");

        var unitFilePath = "/etc/systemd/system/pmmux.service";

        await File.WriteAllTextAsync(unitFilePath, unitFileContent.ToString(), cancellationToken)
            .ConfigureAwait(false);

        await ShellUtility.ExecAsync("systemctl", "daemon-reload", cancellationToken).ConfigureAwait(false);
        await ShellUtility.ExecAsync("systemctl", "enable pmmux.service", cancellationToken).ConfigureAwait(false);
        await ShellUtility.ExecAsync("systemctl", "start pmmux.service", cancellationToken).ConfigureAwait(false);
    }
}
