using System.CommandLine;

using Pmmux.Abstractions;

namespace Pmmux.App;

internal class CommandLineBuilder(RootCommand rootCommand) : ICommandLineBuilder
{
    public ICommandLineBuilder Add(Option option)
    {
        rootCommand.Add(option);

        return this;
    }

    public ICommandLineBuilder AddSubcommand(Command command)
    {
        rootCommand.Add(command);

        return this;
    }
}
