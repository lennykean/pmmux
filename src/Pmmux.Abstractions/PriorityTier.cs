namespace Pmmux.Abstractions;

/// <summary>
/// Routing priority tier for backends.
/// </summary>
/// <remarks>
/// Priority tiers influence backend selection when multiple backends match a request.
/// Some routing strategies (like <c>least-requests</c>) prefer higher tiers (<see cref="Vip"/>) over
/// lower tiers (<see cref="Fallback"/>).
/// </remarks>
public enum PriorityTier
{
    /// <summary>
    /// Fallback priority - lowest priority, used only when all higher-priority backends are unavailable.
    /// </summary>
    /// <remarks>
    /// Typically used for error pages, maintenance notices, or blackhole backends.
    /// </remarks>
    Fallback = 0,

    /// <summary>
    /// Standby priority - used when normal-priority backends are unavailable.
    /// </summary>
    /// <remarks>
    /// Typically used for backup servers or disaster recovery resources.
    /// </remarks>
    Standby = 1,

    /// <summary>
    /// Normal priority - standard priority for most production backends.
    /// </summary>
    /// <remarks>
    /// This is the default priority when not explicitly specified.
    /// </remarks>
    Normal = 2,

    /// <summary>
    /// VIP priority - highest priority, always preferred when healthy.
    /// </summary>
    /// <remarks>
    /// Typically used for premium or primary servers that should receive traffic first.
    /// </remarks>
    Vip = 3
}
