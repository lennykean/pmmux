using System;

namespace Pmmux.Extensions.QBittorrent;

internal sealed record QBittorrentConfig
{
    public Uri Endpoint { get; init; } = new("http://127.0.0.1:18080/");
    public int? PeerPort { get; init; }
    public TimeSpan Interval { get; init; } = TimeSpan.FromSeconds(30);
    public string? Username { get; init; }
    public string? PasswordEnvironment { get; init; }
}
