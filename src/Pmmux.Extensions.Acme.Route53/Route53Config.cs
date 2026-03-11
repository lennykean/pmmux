namespace Pmmux.Extensions.Acme.Route53;

internal record Route53Config
{
    public string? AccessKeyId { get; init; }
    public string? SecretAccessKey { get; init; }
    public string? CredentialProfile { get; init; }
    public string? CredentialFile { get; init; }
}
