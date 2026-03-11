namespace Pmmux.Extensions.Acme.Models;

internal record AcmeAccountInfo
{
    public string? AccountKeyPem { get; init; }
    public string? Email { get; init; }
    public string? AccountUri { get; init; }
}
