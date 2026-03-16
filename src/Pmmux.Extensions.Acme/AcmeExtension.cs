using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;
using Pmmux.Extensions.Acme.Abstractions;
using Pmmux.Extensions.Acme.Models;

using static Pmmux.Abstractions.CompactConfig;

namespace Pmmux.Extensions.Acme;

/// <summary>
/// Extension for ACME TLS certificate automation.
/// </summary>
public sealed class AcmeExtension : IExtension
{
    internal static Option<bool> DisableOption { get; } = new("--acme-disable")
    {
        Description = "disable ACME certificate automation",
    };

    internal static Option<string> EmailOption { get; } = new("--acme-email")
    {
        Description = "email address for ACME account registration",
    };

    internal static Option<string> StoragePathOption { get; } = new("--acme-storage-path")
    {
        Description = "directory path for ACME account and certificate storage",
        DefaultValueFactory = _ => "./acme"
    };

    internal static Option<bool> StagingOption { get; } = new("--acme-staging")
    {
        Description = "use Let's Encrypt staging environment",
    };

    internal static Option<string?> ServerUrlOption { get; } = new("--acme-server-url")
    {
        Description = "override ACME server URL",
    };

    internal static Option<int> RenewalLeadOption { get; } = new("--acme-renewal-lead")
    {
        Description = "certificate renewal lead time in days - perform renewals ahead of expiration to prevent gaps",
        DefaultValueFactory = _ => 30
    };

    internal static Option<IEnumerable<string>> CertificatesOption { get; } = new("--acme-certificate")
    {
        Description =
            """
            certificate to manage via ACME
            format: <domain>[:<challenge-type>[:<properties>]]
            multiple SANs use semicolons: san=www.example.com;api.example.com

            examples:
            --acme-certificate example.com
            --acme-certificate "*.example.com:dns-01"
            --acme-certificate "example.com:dns-01:provider=route53,hostedzoneid=HOSTEDZONEID,san=www.example.com"
            """,
    };

    void IExtension.RegisterCommandOptions(ICommandLineBuilder builder)
    {
        builder.Add(DisableOption);
        builder.Add(EmailOption);
        builder.Add(StoragePathOption);
        builder.Add(StagingOption);
        builder.Add(ServerUrlOption);
        builder.Add(RenewalLeadOption);
        builder.Add(CertificatesOption);
    }

    void IExtension.RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        var section = hostContext.Configuration.GetSection("pmmux");

        var acmeDisable = section.GetValue<bool>(DisableOption.Name);

        var certificates = !acmeDisable
            ? section.GetSection(CertificatesOption.Name)
                .GetChildren()
                .Select(c => ParseCertificateEntry(c.Value ?? string.Empty))
                .ToList()
            : [];

        var acmeConfig = new AcmeConfig
        {
            AcmeDisable = acmeDisable,
            AcmeEmail = section.GetValue<string>(EmailOption.Name) ?? string.Empty,
            AcmeStoragePath = section.GetValue<string>(StoragePathOption.Name) ?? "./acme",
            AcmeStaging = section.GetValue<bool>(StagingOption.Name),
            AcmeServerUrl = section.GetValue<string?>(ServerUrlOption.Name),
            AcmeRenewalLead = Math.Max(1, section.GetValue<int?>(RenewalLeadOption.Name) ?? 30),
            AcmeCertificates = certificates
        };

        services.AddSingleton(acmeConfig);
        services.AddSingleton<AcmeStateStore>();
        services.AddSingleton<AcmeClient>();
        services.AddSingleton<IChallengeProcessor, DnsChallengeProcessor>();

        services.AddSingleton<AcmeService>();
        services.AddHostedService(sp => sp.GetRequiredService<AcmeService>());
    }

    internal static AcmeCertificateEntry ParseCertificateEntry(string value)
    {
        var segments = Parse(value).ToArray();

        string primaryDomain;
        var challenge = "dns-01";
        string? provider = null;
        var domains = new List<string>();
        var providerProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (segments is [IdentifierSegment { Name: var name }])
        {
            primaryDomain = name;
        }
        else if (segments is [IdentifierSegment { Name: var name2 }, IdentifierSegment { Name: var ch }])
        {
            primaryDomain = name2;
            challenge = ch;
        }
        else if (segments is
            [IdentifierSegment { Name: var name3 }, IdentifierSegment { Name: var ch2 }, PropertiesSegment props])
        {
            primaryDomain = name3;
            challenge = ch2;
            ExtractProperties(props, ref provider, domains, providerProperties);
        }
        else
        {
            throw new ArgumentException($"invalid ACME certificate entry: {value}");
        }

        domains.Insert(0, primaryDomain);
        domains = [.. domains.Distinct(StringComparer.OrdinalIgnoreCase)];

        foreach (var domain in domains)
        {
            ValidateDomainName(domain);
        }

        return new AcmeCertificateEntry
        {
            Domains = domains,
            Challenge = challenge,
            Provider = provider,
            ProviderProperties = providerProperties
        };
    }

    private static void ExtractProperties(
        PropertiesSegment properties,
        ref string? provider,
        List<string> domains,
        Dictionary<string, string> providerProperties)
    {
        foreach (var (key, value) in properties.Properties)
        {
            switch (key.ToLowerInvariant())
            {
                case "provider":
                    provider = value;
                    break;
                case "san":
                    foreach (var san in value.Split(';', StringSplitOptions.RemoveEmptyEntries))
                    {
                        var trimmed = san.Trim();
                        if (!string.IsNullOrEmpty(trimmed))
                        {
                            domains.Add(trimmed);
                        }
                    }
                    break;
                default:
                    providerProperties[key] = value;
                    break;
            }
        }
    }

    private static void ValidateDomainName(string domain)
    {
        if (string.IsNullOrWhiteSpace(domain))
        {
            throw new ArgumentException("domain name cannot be empty in ACME certificate entry");
        }

        var name = domain.StartsWith("*.", StringComparison.Ordinal) ? domain.Substring(2) : domain;

        if (name.Length == 0 || name.Length > 253)
        {
            throw new ArgumentException($"invalid domain name length: {domain}");
        }

        var labels = name.Split('.');
        if (labels.Length < 2)
        {
            throw new ArgumentException($"single-label domain names are not valid for ACME: {domain}");
        }

        foreach (var label in labels)
        {
            if (label.Length == 0 || label.Length > 63)
            {
                throw new ArgumentException($"invalid domain label length in: {domain}");
            }

            if (label[0] == '-' || label[label.Length - 1] == '-')
            {
                throw new ArgumentException($"domain label cannot start or end with hyphen in: {domain}");
            }

            foreach (var c in label)
            {
                if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '-'))
                {
                    throw new ArgumentException($"invalid character '{c}' in domain name: {domain}");
                }
            }
        }
    }
}
