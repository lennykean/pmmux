using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using Pmmux.Extensions.Management.Abstractions;

namespace Pmmux.Extensions.QBittorrent;

internal sealed class QBittorrentEndpointGroup(QBittorrentSynchronizer synchronizer) : IManagementEndpointGroup
{
    public string Name => "qbittorrent";

    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("", () => Results.Ok(synchronizer.Status));
    }
}
