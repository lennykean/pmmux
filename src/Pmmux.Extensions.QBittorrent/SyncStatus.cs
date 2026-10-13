using System;

namespace Pmmux.Extensions.QBittorrent;

internal sealed record SyncStatus(
    string State,
    int? MappedPort = null,
    int? ApplicationPort = null,
    DateTimeOffset? LastVerifiedAt = null);
