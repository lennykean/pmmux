using System;
using System.Collections.Generic;

using Pmmux.Abstractions;
using Pmmux.Extensions.Management.Models;

namespace Pmmux.Extensions.Management;

internal static class DtoExtensions
{
    public static BackendSpecDto ToDto(this BackendSpec spec) => new()
    {
        Name = spec.Name,
        ProtocolName = spec.ProtocolName,
        Parameters = new Dictionary<string, string>(spec.Parameters)
    };

    public static BackendStatusInfoDto ToDto(this BackendInfo info) => new()
    {
        Spec = info.Spec.ToDto(),
        Properties = new Dictionary<string, string>(info.Properties),
        PriorityTier = info.PriorityTier,
        Status = BackendStatus.Unknown
    };

    public static BackendStatusInfoDto ToDto(this BackendStatusInfo info) => new()
    {
        Spec = info.Backend.Spec.ToDto(),
        Properties = new Dictionary<string, string>(info.Backend.Properties),
        PriorityTier = info.Backend.PriorityTier,
        Status = info.Status,
        StatusReason = info.StatusReason,
        LastHealthCheck = info.LastHealthCheck,
        HealthCheckFailureCount = info.HealthCheckFailureCount,
        HealthCheckSuccessCount = info.HealthCheckSuccessCount
    };

    public static ListenerInfoDto ToDto(this ListenerInfo info) => new()
    {
        NetworkProtocol = info.NetworkProtocol,
        Address = info.LocalEndPoint.Address.ToString(),
        Port = info.LocalEndPoint.Port,
        Properties = new Dictionary<string, string>(info.Properties)
    };

    public static PortMapInfoDto ToDto(this PortMapInfo info) => new()
    {
        NetworkProtocol = info.NetworkProtocol,
        PublicAddress = info.PublicEndpoint.Address.ToString(),
        PublicPort = info.PublicEndpoint.Port,
        LocalPort = info.LocalPort,
        NatProtocol = info.NatProtocol.ToString()
    };

    public static NatDeviceInfoDto ToDto(this NatDeviceInfo info) => new()
    {
        NatProtocol = info.NatProtocol.ToString(),
        DeviceAddress = info.Endpoint.Address.ToString(),
        DevicePort = info.Endpoint.Port,
        PublicAddress = info.PublicAddress.ToString(),
        Discovered = info.Discovered.ToString("O")
    };

    public static HealthCheckSpecDto ToDto(this HealthCheckSpec spec) => new()
    {
        ProtocolName = spec.ProtocolName,
        BackendName = spec.BackendName,
        InitialDelay = (int)spec.InitialDelay.TotalMilliseconds,
        Interval = (int)spec.Interval.TotalMilliseconds,
        Timeout = (int)spec.Timeout.TotalMilliseconds,
        FailureThreshold = spec.FailureThreshold,
        RecoveryThreshold = spec.RecoveryThreshold,
        Parameters = new Dictionary<string, string>(spec.Parameters)
    };

    public static HealthCheckSpec ToHealthCheckSpec(this HealthCheckSpecDto request) => new(request.Parameters ?? [])
    {
        ProtocolName = request.ProtocolName,
        BackendName = request.BackendName,
        InitialDelay = request.InitialDelay is { } delay ? TimeSpan.FromMilliseconds(delay) : TimeSpan.Zero,
        Interval = request.Interval is { } interval ? TimeSpan.FromMilliseconds(interval) : TimeSpan.FromSeconds(10),
        Timeout = request.Timeout is { } timeout ? TimeSpan.FromMilliseconds(timeout) : TimeSpan.FromSeconds(5),
        FailureThreshold = request.FailureThreshold ?? 3,
        RecoveryThreshold = request.RecoveryThreshold ?? 5,
    };
}
