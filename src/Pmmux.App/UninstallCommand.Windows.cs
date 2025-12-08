using System;
using System.CommandLine;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Pmmux.App;

internal class UninstallCommand : Command
{
    public UninstallCommand() : base("uninstall", "uninstall service")
    {
        SetAction(Execute);
    }

    private void Execute(ParseResult result)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }

        using var installer = new TransactedInstaller();
        using var serviceInstaller = new ServiceInstaller { ServiceName = "pmmux" };

        installer.Installers.Add(serviceInstaller);
        installer.Context = new InstallContext();

        installer.Uninstall(null);
    }
}
