# BitTorrent Extension

The BitTorrent extension provides protocol detection for BitTorrent traffic.

## Backend Protocols

Protocol | Description
-|-
`bittorrent-pass` | Passthrough backend with BitTorrent protocol detection

## Installation

The following example adds the BitTorrent extension to the configuration:

```sh
pmmux -x Pmmux.Extensions.BitTorrent.dll ...
```

Or in `pmmux.toml`:

```toml
[pmmux]
extensions = ["Pmmux.Extensions.BitTorrent.dll"]
```

## Usage

The `bittorrent-pass` protocol automatically detects BitTorrent handshakes and routes traffic accordingly.

### Basic Setup

```sh
pmmux \
  -b "torrent:bittorrent-pass:target.ip=127.0.0.1,target.port=6881" \
  -p 6881:6881:tcp
```

### Configuration Parameters

Parameter | Required | Description
-|-|-
`target.ip` | Yes | Target IP address
`target.port` | Yes | Target port

The `bittorrent-pass` backend also supports standard matching and routing parameters, including connection properties, IP/port matching, and priority tiers. See the [Configuration Guide](../../docs/configuration.md#common-parameters) for details.

### Protocol Detection

The extension identifies BitTorrent traffic by detecting the BitTorrent protocol handshake:
- First byte must be `19` (protocol length)
- Followed by the string "BitTorrent protocol"

This works for both TCP connections and UDP messages.

## Example: Multi-Protocol Port

Route BitTorrent traffic to one backend, other traffic to another:

```toml
[pmmux]
extensions = ["Pmmux.Extensions.BitTorrent.dll"]
port-bindings = ["6881:6881:tcp"]

backends = [
  "torrent:bittorrent-pass:target.ip=127.0.0.1,target.port=6882",
  "other:pass:target.ip=127.0.0.1,target.port=8080,priority=fallback",
]
```

In this configuration:
- BitTorrent traffic is routed to port 6882
- All other traffic falls back to port 8080
