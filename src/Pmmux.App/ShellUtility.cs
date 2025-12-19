using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Pmmux.App;

internal static class ShellUtility
{
    public static async Task ExecAsync(string fileName, string arguments, CancellationToken cancellationToken)
    {
        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
        });

        if (process is not null)
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}

