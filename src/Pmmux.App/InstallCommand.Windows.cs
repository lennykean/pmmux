using System;
using System.Collections;
using System.CommandLine;
using System.Configuration.Install;
using System.IO;
using System.ServiceProcess;

namespace Pmmux.App;

internal class InstallCommand : Command
{
    public Option<string?> ArgumentsOption { get; } = new("--arguments", "-a")
    {
        Description = "arguments to pass to the service",
    };

    public InstallCommand() : base("install", "install as a service")
    {
        Add(ArgumentsOption);

        SetAction(Execute);
    }

    private void Execute(ParseResult result)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }


        using var processInstaller = new ServiceProcessInstaller();
        using var serviceInstaller = new ServiceInstaller();
        using var installer = new TransactedInstaller();

        installer.Context = new InstallContext();

        serviceInstaller.ServiceName = "pmmux";
        serviceInstaller.DisplayName = "pmmux";
        serviceInstaller.Description = "Port map multiplexer";
        serviceInstaller.StartType = ServiceStartMode.Automatic;
        processInstaller.Account = ServiceAccount.LocalSystem;

        installer.Installers.Add(serviceInstaller);
        installer.Installers.Add(processInstaller);

        var executable = Path.Combine(AppContext.BaseDirectory, "pmmux.exe");

        installer.Context.Parameters["assemblypath"] = $"\"{executable}\" {result.GetValue(ArgumentsOption)}";

        installer.Install(new Hashtable());
    }
}
