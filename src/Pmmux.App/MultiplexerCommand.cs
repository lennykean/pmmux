using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mono.Nat;

using Pmmux.Abstractions;
using Pmmux.Core;
using Pmmux.Core.Configuration;

using static Pmmux.Abstractions.CompactConfig;

namespace Pmmux.App;

internal class MultiplexerCommand : RootCommand
{
    private readonly IEnumerable<IExtension> _extensions;

    public MultiplexerCommand(IEnumerable<IExtension> extensions) : base("pmmux - port map multiplexer")
    {
        _extensions = extensions;

        Add(BackendsOption);
        Add(PortBindingsOption);
        Add(PortMapProtocolOption);
        Add(RoutingStrategyOption);
        Add(HealthChecksOption);
        Add(GatewayIpOption);
        Add(NetworkInterfaceOption);
        Add(PortMapLifetimeOption);
        Add(PortMapRenewalLeadOption);
        Add(SelectionTimeoutOption);
        Add(BindTimeoutOption);
        Add(PreviewSizeLimitOption);
        Add(QueueLengthOption);
        Add(MuxOnlyOption);
        Add(PortmapOnlyOption);

        Validators.Add(result =>
        {
            if (result.GetValue(MuxOnlyOption) && result.GetValue(PortmapOnlyOption))
            {
                result.AddError($"{MuxOnlyOption.Name} and {PortmapOnlyOption.Name} cannot be used together");
            }
        });

        SetAction(ExecuteAsync);
    }

    public static Option<string[]> BackendsOption { get; } = new("--backend", "-b", "--backends")
    {
        Description =
            """
            backend specification(s)
            format: <backend-name>:<backend-protocol>:<parameter=value,...>

            examples:
            # passthrough backend named web that forwards to local port 3000
            -b web:pass:ip=127.0.0.1,port=3000

            # multiple passthrough backends for load balancing with a noop fallthrough
            -b api1:pass:ip=127.0.0.1,port=5001 -b api2:pass:ip=127.0.0.1,port=5002 -b blackhole:noop

            # a custom-protocol backend named service with custom parameters
            -b service:custom-protocol:host="service1.example.com",api-key="example"
            """,
    };

    public static Option<string[]> PortBindingsOption { get; } = new("--port-binding", "--port-bindings", "-p")
    {
        Description =
            """
            port binding specification(s)
            format: <public-port>:<local-port>:<network-protocol?>

            ! for public port disables NAT port mapping for that binding
            ? for ports indicates automatic assignment, if supported by the NAT device
            omitting the network protocol enables both TCP and UDP

            examples:
            # bind public port 80 to local port 8080 using TCP (with NAT mapping)
            -p 80:8080:tcp

            # bind local port 8080 using TCP (no NAT mapping)
            -p !:8080:tcp

            # bind an automatically assigned public port to local port 22 using TCP
            -p ?:22:tcp

            # bind an automatically assigned public port to an automatically assigned local port using both TCP and UDP
            -p ?:?

            """
            ,
        Arity = ArgumentArity.OneOrMore
    };

    public static Option<NatProtocol?> PortMapProtocolOption { get; } = new("--port-map-protocol", "-m")
    {
        Description = "query for NAT devices supporting a specific protocol",
    };

    public static Option<string?> RoutingStrategyOption { get; } = new("--routing-strategy", "-r")
    {
        Description = "routing strategy",
        DefaultValueFactory = _ => "first-available"
    };

    public static Option<HealthCheckSpec[]> HealthChecksOption { get; } = new("--health-check", "-z", "--health-checks")
    {
        Description =
            """
            health check specification(s)
            format: <backend-protocol?>:<backend-name?>:<parameter=value,...>

            common parameters:
            - initial-delay=<ms>       delay before first check (default: 0)
            - interval=<ms>            time between checks (default: 10000)
            - timeout=<ms>             check timeout (default: 5000)
            - failure-threshold=<n>    consecutive failures before marking unhealthy (default: 3)
            - recovery-threshold=<n>   consecutive successes before marking healthy (default: 5)

            additional parameters may be supported depending on the backend protocol

            examples:
            # target passthrough backends with a 5s interval and a 2s timeout
            -z pass:interval=5000,timeout=2000

            # target a passthrough backend named db with failure and recovery thresholds
            -z pass:db:failure-threshold=5,recovery-threshold=10

            # target passthrough backends using default parameters and a specific backend named api with 1s interval
            -z pass -z pass:api:interval=1000

            # target a custom-protocol backend named service with custom parameters
            -z custom-protocol:service:api-key="example"
            """,
    };

    public static Option<IPAddress?> GatewayIpOption { get; } = new("--gateway", "-g")
    {
        Description = "query a specific gateway IP address for NAT devices",
        CustomParser = result => result.Tokens is [var token] ? IPAddress.Parse(token.Value) : null,
        Validators =
        {
            result =>
            {
                if (result.Tokens is [var token] && !IPAddress.TryParse(token.Value, out var ip))
                {
                    result.AddError($"invalid gateway IP address: \"{string.Join(',',result.Tokens)}\"");
                }
            }
        }
    };

    public static Option<string?> NetworkInterfaceOption { get; } = new("--network-interface", "-i")
    {
        Description = "query for NAT devices on a specific network interface"
    };

    public static Option<int?> PortMapLifetimeOption { get; } = new("--port-map-lifetime")
    {
        Description =
            """
            port map lifetime in seconds - mappings will be auto-renewed upon expiration.
            if not specified, defaults will be used based on the protocol
            """
    };

    public static Option<int> PortMapRenewalLeadOption { get; } = new("--port-map-renewal-lead")
    {
        Description = "port map renewal lead time in seconds - perform renewals ahead of expiration to prevent gaps",
        DefaultValueFactory = _ => 10
    };

    public static Option<int> SelectionTimeoutOption { get; } = new("--selection-timeout", "-s")
    {
        Description = "backend selection timeout in milliseconds",
        DefaultValueFactory = _ => 5000
    };

    public static Option<int> BindTimeoutOption { get; } = new("--bind-timeout", "-t")
    {
        Description = "bind timeout in milliseconds",
        DefaultValueFactory = _ => 30000
    };

    public static Option<long?> PreviewSizeLimitOption { get; } = new("--preview-buffer-limit")
    {
        Description = "limit the number of bytes that can be buffered for backend selection",
        DefaultValueFactory = _ => null
    };

    public static Option<int> QueueLengthOption { get; } = new("--queue-length", "-q")
    {
        Description = "connection queue length",
        DefaultValueFactory = _ => 100
    };

    public static Option<bool> MuxOnlyOption { get; } = new("--mux-only")
    {
        Description = "multiplexer only, ports will not be mapped",
        DefaultValueFactory = _ => false
    };

    public static Option<bool> PortmapOnlyOption { get; } = new("--portmap-only")
    {
        Description = "port mapping only, multiplexer will be disabled",
        DefaultValueFactory = _ => false
    };

    public void AddRange(IEnumerable<Option> options)
    {
        foreach (var option in options)
        {
            Add(option);
        }
    }

    private async Task<int> ExecuteAsync(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                ConfigurationLoader.Load(builder, Options, parseResult);
            })
            .ConfigureHostOptions(options =>
            {
                options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost;
            })
            .ConfigureLogging((hostContext, logging) => LoggingConfigLoader.Load(hostContext.Configuration, logging))
            .ConfigureServices((hostContext, services) =>
            {
#if LINUX
                services.AddSystemd();
#elif WINDOWS
                services.AddWindowsService();
#endif
                services.AddPmmux(
                    hostContext,
                    () => ResolveListenerConfig(hostContext.Configuration),
                    () => ResolvePortMapperConfig(hostContext.Configuration),
                    () => ResolveRouterConfig(hostContext.Configuration),
                    _extensions);

                services.AddHostedService<MultiplexerHostedService>();
            })
            .Build();

        await builder.RunAsync(cancellationToken);

        return 0;
    }

    private static ListenerConfig ResolveListenerConfig(IConfiguration configuration)
    {
        var section = configuration.GetSection(ConfigurationLoader.ROOT_KEY);

        var portBindings = section.GetSection(PortBindingsOption.Name).GetChildren();
        var queueLength = section.GetValue<int>(QueueLengthOption.Name);
        var portmapOnly = section.GetValue<bool>(PortmapOnlyOption.Name);

        var bindings = portmapOnly
            ? []
            : portBindings.SelectMany(s => ParsePortBinding(s.Get<string>(), portmapOnly)).ToArray();

        return new()
        {
            PortBindings = bindings,
            QueueLength = queueLength,
        };
    }

    private static PortWardenConfig ResolvePortMapperConfig(IConfiguration configuration)
    {
        var section = configuration.GetSection(ConfigurationLoader.ROOT_KEY);

        var portBindings = section.GetSection(PortBindingsOption.Name).GetChildren();
        var portMapProtocol = section.GetValue<NatProtocol?>(PortMapProtocolOption.Name);
        var gatewayIp = section.GetValue<IPAddress?>(GatewayIpOption.Name);
        var networkInterface = section.GetValue<string?>(NetworkInterfaceOption.Name);
        var portMapLifetime = section.GetValue<int?>(PortMapLifetimeOption.Name);
        var portMapRenewalLead = section.GetValue<int>(PortMapRenewalLeadOption.Name);
        var bindTimeout = section.GetValue<int>(BindTimeoutOption.Name);
        var muxOnly = section.GetValue<bool>(MuxOnlyOption.Name);

        var portMaps = muxOnly
            ? []
            : portBindings.SelectMany(s => ParsePortMaps(s.Get<string>(), muxOnly)).ToArray();

        return new()
        {
            PortMaps = portMaps,
            NatProtocol = portMapProtocol,
            GatewayAddress = gatewayIp ?? IPAddress.Any,
            NetworkInterface = networkInterface,
            Lifetime = portMapLifetime is not null ? TimeSpan.FromSeconds(portMapLifetime.Value) : null,
            RenewalLead = TimeSpan.FromSeconds(portMapRenewalLead),
            Timeout = TimeSpan.FromMilliseconds(bindTimeout),
        };
    }

    private static RouterConfig ResolveRouterConfig(IConfiguration configuration)
    {
        var section = configuration.GetSection(ConfigurationLoader.ROOT_KEY);

        var backends = section.GetSection(BackendsOption.Name).GetChildren();
        var healthChecks = section.GetSection(HealthChecksOption.Name).GetChildren();
        var routingStrategy = section.GetValue<string>(RoutingStrategyOption.Name);
        var selectionTimeout = section.GetValue<int>(SelectionTimeoutOption.Name);
        var previewSizeLimit = section.GetValue<long?>(PreviewSizeLimitOption.Name);

        return new()
        {
            Backends = backends.Select(s => ParseBackend(s.Get<string>())) ?? [],
            HealthChecks = healthChecks.Select(s => ParseHealthCheck(s.Get<string>())) ?? [],
            RoutingStrategy = routingStrategy!,
            SelectionTimeout = TimeSpan.FromMilliseconds(selectionTimeout),
            PreviewSizeLimit = previewSizeLimit,
        };
    }

    private static BackendSpec ParseBackend(string? backend)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(backend);

        var segments = CompactConfig.Parse(backend).ToArray();
        if (segments is [IdentifierSegment name, IdentifierSegment protocol, .. var rest])
        {
            if (rest is [PropertiesSegment props])
            {
                return new(name.Name, protocol.Name, props.Properties);
            }
            if (rest is [])
            {
                return new(name.Name, protocol.Name, new Dictionary<string, string>());
            }
        }
        throw new ArgumentException($"invalid backend spec: \"{backend}\"", nameof(backend));
    }

    private static HealthCheckSpec ParseHealthCheck(string? healthCheck)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(healthCheck);

        var segments = CompactConfig.Parse(healthCheck).ToArray();

        HealthCheckSpec healthCheckSpec;

        if (segments is [PropertiesSegment protocolProps])
        {
            healthCheckSpec = new(protocolProps.Properties);
        }
        else if (segments is [IdentifierSegment protocol, .. var rest])
        {
            if (rest is [IdentifierSegment backend, .. var backendRest])
            {
                healthCheckSpec = new(backendRest is [PropertiesSegment props] ? props.Properties : [])
                {
                    ProtocolName = protocol.Name,
                    BackendName = backend.Name,
                };
            }
            else
            {
                healthCheckSpec = new(rest is [PropertiesSegment props] ? props.Properties : [])
                {
                    ProtocolName = protocol.Name,
                };
            }
        }
        else
        {
            throw new ArgumentException($"invalid health check spec: \"{healthCheck}\"", nameof(healthCheck));
        }
        if (healthCheckSpec.Parameters.TryGetValue("initial-delay", out var initialDelay))
        {
            if (!int.TryParse(initialDelay, out var initialDelayMs))
            {
                throw new ArgumentException($"invalid initial delay: \"{initialDelay}\"", nameof(healthCheck));
            }
            healthCheckSpec = healthCheckSpec with { InitialDelay = TimeSpan.FromMilliseconds(initialDelayMs) };
        }
        if (healthCheckSpec.Parameters.TryGetValue("interval", out var interval))
        {
            if (!int.TryParse(interval, out var intervalMs))
            {
                throw new ArgumentException($"invalid interval: \"{interval}\"", nameof(healthCheck));
            }
            healthCheckSpec = healthCheckSpec with { Interval = TimeSpan.FromMilliseconds(intervalMs) };
        }
        if (healthCheckSpec.Parameters.TryGetValue("timeout", out var timeout))
        {
            if (!int.TryParse(timeout, out var timeoutMs))
            {
                throw new ArgumentException($"invalid timeout: \"{timeout}\"", nameof(healthCheck));
            }
            healthCheckSpec = healthCheckSpec with { Timeout = TimeSpan.FromMilliseconds(timeoutMs) };
        }
        if (healthCheckSpec.Parameters.TryGetValue("failure-threshold", out var fail))
        {
            if (!int.TryParse(fail, out var failCount))
            {
                throw new ArgumentException($"invalid failure threshold: \"{fail}\"", nameof(healthCheck));
            }
            healthCheckSpec = healthCheckSpec with { FailureThreshold = failCount };
        }
        if (healthCheckSpec.Parameters.TryGetValue("recovery-threshold", out var recover))
        {
            if (!int.TryParse(recover, out var recoverCount))
            {
                throw new ArgumentException($"invalid recovery threshold: \"{recover}\"", nameof(healthCheck));
            }
            healthCheckSpec = healthCheckSpec with { RecoveryThreshold = recoverCount };
        }
        return healthCheckSpec;
    }

    private static IEnumerable<ListenerConfig.BindingConfig> ParsePortBinding(string? portBinding, bool portmapOnly)
    {
        if (portmapOnly)
        {
            yield break;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(portBinding);

        var segments = CompactConfig.Parse(portBinding).ToArray();

        if (segments is [IdentifierSegment, IdentifierSegment localPortSegment, .. var rest])
        {
            var localPort = localPortSegment.Name switch
            {
                "?" =>
                    null,
                string localPortString when int.TryParse(localPortString, out var port) =>
                    (int?)port,
                _ =>
                    throw new ArgumentException($"invalid port: \"{localPortSegment.Name}\"", nameof(portBinding)),
            };
            if (localPort is not null)
            {
                if (rest is [])
                {
                    yield return new(Protocol.Tcp, (int)localPort);
                    yield return new(Protocol.Udp, (int)localPort);
                    yield break;
                }
                if (rest is [IdentifierSegment protocolSegment] &&
                    Enum.TryParse<Protocol>(protocolSegment.Name, ignoreCase: true, out var protocol))
                {
                    yield return new(protocol, (int)localPort);
                    yield break;
                }
            }
        }
        throw new ArgumentException($"invalid port binding spec: {portBinding}", nameof(portBinding));
    }

    private static IEnumerable<PortWardenConfig.PortMapConfig> ParsePortMaps(string? portMapping, bool muxOnly)
    {
        if (muxOnly)
        {
            yield break;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(portMapping);

        var segments = CompactConfig.Parse(portMapping).ToArray();

        if (segments is [IdentifierSegment publicPortSegment, IdentifierSegment localPortSegment, .. var rest])
        {
            // ! means no port mapping for this binding
            if (publicPortSegment.Name == "!")
            {
                yield break;
            }

            var publicPort = publicPortSegment.Name switch
            {
                "?" =>
                    null,
                string publicPortString when int.TryParse(publicPortString, out var port) =>
                    (int?)port,
                _ =>
                    throw new ArgumentException($"invalid port: \"{publicPortSegment.Name}\"", nameof(portMapping)),
            };
            var localPort = localPortSegment.Name switch
            {
                "?" =>
                    null,
                string localPortString when int.TryParse(localPortString, out var port) =>
                    (int?)port,
                _ =>
                    throw new ArgumentException($"invalid port: \"{localPortSegment.Name}\"", nameof(portMapping)),
            };
            if (rest is [])
            {
                yield return new(Protocol.Tcp, localPort, publicPort);
                yield return new(Protocol.Udp, localPort, publicPort);
                yield break;
            }
            if (rest is [IdentifierSegment protocolSegment] &&
                Enum.TryParse<Protocol>(protocolSegment.Name, ignoreCase: true, out var protocol))
            {
                yield return new(protocol, localPort, publicPort);
                yield break;
            }
        }
        throw new ArgumentException($"invalid port spec: {portMapping}", nameof(portMapping));
    }
}
