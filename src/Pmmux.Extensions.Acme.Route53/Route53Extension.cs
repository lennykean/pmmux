using System;
using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pmmux.Abstractions;
using Pmmux.Extensions.Acme.Abstractions;

namespace Pmmux.Extensions.Acme.Route53;

/// <summary>
/// Route 53 DNS provider extension for ACME DNS-01 challenges.
/// </summary>
public sealed class Route53Extension : IExtension
{
    internal static Option<string?> AccessKeyIdOption { get; } = new("--acme-route53-access-key-id")
    {
        Description = "AWS access key ID for Route 53 DNS provider",
    };

    internal static Option<string?> SecretAccessKeyOption { get; } = new("--acme-route53-secret-access-key")
    {
        Description = "AWS secret access key for Route 53 DNS provider",
    };

    internal static Option<string?> CredentialProfileOption { get; } = new("--acme-route53-credential-profile")
    {
        Description = "AWS credential profile name for Route 53 DNS provider",
    };

    internal static Option<string?> CredentialFileOption { get; } = new("--acme-route53-credential-file")
    {
        Description = "path to AWS shared credentials file for Route 53 DNS provider",
    };

    void IExtension.RegisterCommandOptions(ICommandLineBuilder builder)
    {
        builder.Add(AccessKeyIdOption);
        builder.Add(SecretAccessKeyOption);
        builder.Add(CredentialProfileOption);
        builder.Add(CredentialFileOption);
    }

    void IExtension.RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
    {
        var section = hostContext.Configuration.GetSection("pmmux");

        var route53Config = new Route53Config
        {
            AccessKeyId = section.GetValue<string?>(AccessKeyIdOption.Name),
            SecretAccessKey = section.GetValue<string?>(SecretAccessKeyOption.Name),
            CredentialProfile = section.GetValue<string?>(CredentialProfileOption.Name),
            CredentialFile = section.GetValue<string?>(CredentialFileOption.Name),
        };

        if (route53Config.CredentialProfile is not null &&
            (route53Config.AccessKeyId is not null || route53Config.SecretAccessKey is not null))
        {
            throw new ArgumentException("AWS credential profile and AWS IAM access keys cannot be specified together");
        }

        services.AddSingleton(route53Config);
        services.AddSingleton<IDnsProvider, Route53DnsProvider>();
    }
}
