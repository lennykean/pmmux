using System;
using System.Threading.Tasks;

namespace Pmmux.Extensions.Acme.Abstractions;

internal sealed class NoopDisposable : IAsyncDisposable
{
    public static readonly NoopDisposable Instance = new();

    public ValueTask DisposeAsync()
    {
        return default;
    }
}
