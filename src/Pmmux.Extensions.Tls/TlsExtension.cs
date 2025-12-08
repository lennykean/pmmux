using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Security.Authentication;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Abstractions;
using Pmmux.Extensions.Tls.Abstractions;

using static Pmmux.Abstractions.CompactConfig;

namespace Pmmux.Extensions.Tls;

/// <summary>
/// TLS extension providing transport layer security for client connections.
/// </summary>
public sealed class TlsExtension : IExtension
{
    internal static Option<bool> EnableTlsOption { get; } = new("--tls-enable")
    {
        Description = "enable tls",
    };

    internal static Option<bool> EnforceTlsOption { get; } = new("--tls-enforce")
    {
        Description = "enforce tls for all connections",
    };

    internal static Option<SslProtocols> SupportedProtocolsOption { get; } = new("--tls-protocol")
    {
        Description = "tls protocol(s) to support",
        Arity = ArgumentArity.ZeroOrMore,
        DefaultValueFactory = _ => SslProtocols.Tls12 | SslProtocols.Tls13,
        CustomParser = static result =>
        {
            SslProtocols? protocols = null;
            foreach (var token in result.Tokens)
            {
                protocols = (protocols ?? default) | Enum.Parse<SslProtocols>(token.Value, ignoreCase: true);
            }
            return protocols ?? default;
        }
    };

    internal static Option<bool> GenerateCertificatesOption { get; } = new("--tls-generate-certificates")
    {
        Description = "auto-generate self-signed certificates for tls",
        DefaultValueFactory = _ => false
    };

    internal static Option<int> HelloTimeoutOption { get; } = new("--tls-hello-timeout")
    {
        Description = "tls hello timeout in milliseconds",
        DefaultValueFactory = _ => 5000
    };

    internal static Option<IEnumerable<string>> CertificatesOption { get; } = new("--tls-certificate")
    {
        Description =
            """
            certificate configuration(s)
            format: <name>:<property=value,...>

            properties:
            - path=<path>           path to certificate file (mutually exclusive with certificate)
            - certificate=<data>    base64-encoded certificate data (mutually exclusive with path)
            - type=<pfx|pem|der>    certificate format (optional, auto-detected if not specified)
            - password=<pass>       certificate password for encrypted certificates (optional)

            note: if type is not specified, will attempt to load as PEM/DER first, then PFX

            examples:
            # load a PFX certificate from file with password
            --tls-certificate my-cert:path=/path/to/cert.pfx,type=pfx,password=secret

            # load a PEM certificate (auto-detected)
            --tls-certificate example:path=/certs/example.com.pem

            # load multiple certificates
            --tls-certificate site1:path=/certs/site1.pfx,type=pfx --tls-certificate site2:path=/certs/site2.pem

            # load certificate from base64 data
            --tls-certificate embedded:certificate=...,type=pem
            """,
    };

    internal static Option<IEnumerable<string>> CertificateMappingsOption { get; } = new("--tls-certificate-map")
    {
        Description =
            """
            certificate domain mapping(s)
            format: <cert-name>:<hostname>

            examples:
            # map certificate to exact hostname
            --tls-certificate-map my-cert:example.com

            # map certificate to wildcard hostname
            --tls-certificate-map wildcard:*.example.com

            # map certificate to multiple hostnames
            --tls-certificate-map my-cert:example.com --tls-certificate-map my-cert:www.example.com

            # use multiple certificates with different hostnames
            --tls-certificate site1:path=/certs/site1.pfx,type=pfx \
            --tls-certificate site2:path=/certs/site2.pfx,type=pfx \
            --tls-certificate-map site1:site1.com \
            --tls-certificate-map site2:site2.com
            """,
    };

    void IExtension.RegisterCommandOptions(ICommandLineBuilder builder)
    {
        builder.Add(EnableTlsOption);
        builder.Add(EnforceTlsOption);
        builder.Add(SupportedProtocolsOption);
        builder.Add(GenerateCertificatesOption);
        builder.Add(HelloTimeoutOption);
        builder.Add(CertificatesOption);
        builder.Add(CertificateMappingsOption);
    }

    void IExtension.RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddSingleton(_ =>
        {
            var section = hostContext.Configuration.GetSection("pmmux");

            var protocolValue = section.GetValue<string?>(SupportedProtocolsOption.Name);
            var supportedProtocols = section.GetSection(SupportedProtocolsOption.Name)
                .GetChildren()
                .Select(c => Enum.TryParse<SslProtocols>(c.Value, ignoreCase: true, out var protocol)
                    ? protocol
                    : throw new ArgumentException($"invalid ssl protocol: {c.Value}"))
                .Aggregate(default(SslProtocols?), (current, protocol) => (current ?? default) | protocol);
            var certificates = section.GetSection(CertificatesOption.Name)
                .GetChildren()
                .Select(c => ParseCertificateConfig(c.Value ?? string.Empty))
                .ToList();
            var mappings = section.GetSection(CertificateMappingsOption.Name)
                .GetChildren()
                .Select(c => ParseCertificateMapConfig(c.Value ?? string.Empty))
                .ToList();

            var helloTimeout = section.GetValue<int?>(HelloTimeoutOption.Name) ?? 5000;

            return new TlsConfig
            {
                TlsEnable = section.GetValue<bool>(EnableTlsOption.Name),
                TlsEnforce = section.GetValue<bool>(EnforceTlsOption.Name),
                TlsProtocols = supportedProtocols,
                TlsGenerateCertificates = section.GetValue<bool>(GenerateCertificatesOption.Name),
                TlsHelloTimeout = TimeSpan.FromMilliseconds(helloTimeout),
                TlsCertificates = certificates,
                TlsCertificateMaps = mappings
            };
        });

        services.AddSingleton<ICertificateManager, CertificateManager>();
        services.AddSingleton<IClientConnectionNegotiator, TlsConnectionNegotiator>();
        services.AddSingleton<IManagementEndpointGroup, TlsEndpointGroup>();
        services.AddHostedService<CertificateLoader>();
    }

    internal static TlsCertificateMapConfig ParseCertificateMapConfig(string value)
    {
        var segments = CompactConfig.Parse(value).ToArray();

        if (segments is not [IdentifierSegment name, IdentifierSegment domain])
        {
            throw new ArgumentException($"invalid certificate map format {value}");
        }

        return new(name.Name, domain.Name);
    }

    internal static TlsCertificateConfig ParseCertificateConfig(string value)
    {
        var segments = CompactConfig.Parse(value).ToArray();

        if (segments is not [IdentifierSegment name, PropertiesSegment properties])
        {
            throw new ArgumentException($"invalid certificate configuration format {value}");
        }

        var config = new TlsCertificateConfig(name.Name);

        if (properties.Properties.TryGetValue("path", out var filePath))
        {
            config = config with { FilePath = filePath };
        }
        if (properties.Properties.TryGetValue("certificate", out var certData))
        {
            config = config with { CertificateData = certData };
        }
        if (properties.Properties.TryGetValue("type", out var type))
        {
            config = config with { Type = Enum.Parse<CertificateType>(type, ignoreCase: true) };
        }
        if (properties.Properties.TryGetValue("password", out var password))
        {
            config = config with { Password = password };
        }

        if (config.FilePath is not null && config.CertificateData is not null)
        {
            throw new ArgumentException("path and certificate cannot be specified together");
        }
        if (config.FilePath is null && config.CertificateData is null)
        {
            throw new ArgumentException("must specify either path or certificate");
        }

        return config;
    }

}
