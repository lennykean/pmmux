using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Pmmux.Abstractions;

namespace Pmmux.Core;

/// <summary>
/// Base class for backend implementations providing common parameter support.
/// </summary>
/// <remarks>
/// Supports <c>local-ip</c>, <c>remote-ip</c>, <c>local-port</c>, <c>remote-port</c>,
/// <c>property[key]</c>, and <c>priority</c> parameters.
/// </remarks>
public abstract class BackendBase : IBackend
{
    private readonly Matcher<IPNetwork>[] _localIpMatchers;
    private readonly Matcher<IPNetwork>[] _remoteIpMatchers;
    private readonly Matcher<NumberRange<ushort>>[] _localPortMatchers;
    private readonly Matcher<NumberRange<ushort>>[] _remotePortMatchers;
    private readonly IReadOnlyDictionary<string, IEnumerable<Matcher<Regex>>> _propertyMatchers;

    /// <param name="spec">The backend specification.</param>
    /// <param name="properties">Protocol-specific properties of the backend.</param>
    /// <param name="defaultPriority">The default priority tier.</param>
    public BackendBase(BackendSpec spec, Dictionary<string, string> properties, PriorityTier defaultPriority)
    {
        var matchers = spec.GetMatchers();

        _localIpMatchers = matchers.TryGetValue("local-ip", out var localIpMatcher)
            ? [.. localIpMatcher.AsMultiValue().AsCidr()]
            : [];

        _remoteIpMatchers = matchers.TryGetValue("remote-ip", out var remoteIpMatcher)
            ? [.. remoteIpMatcher.AsMultiValue().AsCidr()]
            : [];

        _localPortMatchers = matchers.TryGetValue("local-port", out var localPortMatcher)
            ? [.. localPortMatcher.AsMultiValue().AsNumeric<ushort>()]
            : [];

        _remotePortMatchers = matchers.TryGetValue("remote-port", out var remotePortMatcher)
            ? [.. remotePortMatcher.AsMultiValue().AsNumeric<ushort>()]
            : [];

        _propertyMatchers = matchers
            .AsMultiValueIndexed("property")
            .ToDictionary(kv => kv.Key, kv => kv.Value.AsRegex(), StringComparer.OrdinalIgnoreCase);

        if (!spec.Parameters.TryGetValue("priority", out var p) ||
            !Enum.TryParse(p, true, out PriorityTier priority))
        {
            priority = defaultPriority;
        }

        Backend = new(spec, properties, priority);
    }

    /// <inheritdoc />
    public BackendInfo Backend { get; }

    /// <inheritdoc />
    public abstract Task InitializeAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc />
    protected bool MatchesClient(ClientInfo client, IReadOnlyDictionary<string, string> properties)
    {
        var localIp = client.LocalEndpoint?.Address;
        if (!_localIpMatchers.HasMatch(n => localIp is not null && n.Contains(localIp)))
        {
            return false;
        }

        var remoteIp = client.RemoteEndpoint?.Address;
        if (!_remoteIpMatchers.HasMatch(n => remoteIp is not null && n.Contains(remoteIp)))
        {
            return false;
        }

        var localPort = client.LocalEndpoint?.Port;
        if (!_localPortMatchers.HasMatch(r => localPort is not null && r.Contains((ushort)localPort.Value)))
        {
            return false;
        }

        var remotePort = client.RemoteEndpoint?.Port;
        if (!_remotePortMatchers.HasMatch(r => remotePort is not null && r.Contains((ushort)remotePort.Value)))
        {
            return false;
        }

        return MatchesProperties(properties);
    }

    /// <summary>
    /// Check if properties match the configured property matchers.
    /// </summary>
    /// <param name="properties">The properties to check.</param>
    /// <returns><c>true</c> if all property matchers pass; otherwise, <c>false</c>.</returns>
    protected bool MatchesProperties(IReadOnlyDictionary<string, string> properties)
    {
        foreach (var (property, matchers) in _propertyMatchers)
        {
            var propertyExists = properties.TryGetValue(property, out var value);
            if (!matchers.HasMatch(r => propertyExists && r.IsMatch(value!)))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return default;
    }
}
