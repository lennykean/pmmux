using Pmmux.Extensions.Tls.Abstractions;

namespace Pmmux.Extensions.Tls;

/// <summary>Configuration for a TLS certificate.</summary>
/// <param name="Name">The unique name for the certificate.</param>
public record TlsCertificateConfig(string Name)
{
    /// <summary>The path to the certificate file.</summary>
    public string? FilePath { get; init; }
    /// <summary>The base64-encoded certificate data.</summary>
    public string? CertificateData { get; init; }
    /// <summary>The type of certificate.</summary>
    public CertificateType? Type { get; init; }
    /// <summary>The password for encrypted certificates.</summary>
    public string? Password { get; init; }
}
