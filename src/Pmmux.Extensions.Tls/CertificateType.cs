namespace Pmmux.Extensions.Tls;

/// <summary>Certificate file format type.</summary>
public enum CertificateType
{
    /// <summary>PKCS#12 format (.pfx, .p12).</summary>
    Pfx,
    /// <summary>PEM format (.pem, .crt).</summary>
    Pem,
    /// <summary>DER format (.der, .cer).</summary>
    Der
}
