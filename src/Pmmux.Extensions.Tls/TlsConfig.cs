using System;
using System.Collections.Generic;
using System.Security.Authentication;

namespace Pmmux.Extensions.Tls;

internal record TlsConfig
{
    public bool TlsEnable { get; init; } = true;
    public bool TlsEnforce { get; init; } = false;
    public SslProtocols? TlsProtocols { get; init; }
    public bool TlsGenerateCertificates { get; init; } = false;
    public TimeSpan TlsHelloTimeout { get; init; } = TimeSpan.FromMilliseconds(5000);
    public List<TlsCertificateConfig> TlsCertificates { get; init; } = [];
    public List<TlsCertificateMapConfig> TlsCertificateMaps { get; init; } = [];
}
