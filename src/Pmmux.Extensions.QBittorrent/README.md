# qBittorrent Extension

Use this extension to allocate a TCP/UDP port through PortWarden and keep qBittorrent synchronized. No separate `port-bindings` entry is needed.

## Quickstart

Enable the extension in `pmmux.toml`:

```toml
[pmmux]
extensions = ["Pmmux.Extensions.QBittorrent.dll"]
port-map-protocol = "Pmp"

qbittorrent-enable = true
qbittorrent-peer-port = "?"
qbittorrent-url = "http://127.0.0.1:18080/"
```

Use `?` for automatic allocation, if supported by your NAT gateway, or a port number such as `6881`. The extension asks PortWarden to create and maintain matching TCP/UDP mappings without opening a listener. A fixed port must be available; mismatched mappings are rejected.

The extension updates qBittorrent on mapping changes and checks its settings periodically. It also disables qBittorrent's random port and UPnP settings.

## Configuration

Option | Default | Description
-|-|-
`--qbittorrent-enable` | `false` | Enable port synchronization
`--qbittorrent-url` | `http://127.0.0.1:18080/` | Web UI URL; HTTPS required except on loopback
`--qbittorrent-peer-port` | `?` | Incoming peer connection port or `?` for automatic allocation
`--qbittorrent-sync-seconds` | `30` | Settings check interval in seconds (1–3600)
`--qbittorrent-username` | - | Web UI login name
`--qbittorrent-password-env` | - | Environment variable containing the Web UI password

For authentication, set both the username and password environment variable name. Omit both when qBittorrent permits unauthenticated loopback access.

## Management API

When the Management extension is enabled, `GET /api/qbittorrent` returns synchronization status, mapped and application ports, and the last successful settings check. This checks configuration, not external connectivity.
