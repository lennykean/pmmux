using System.Net;

namespace Pmmux.Abstractions;

/// <summary>
/// Endpoint information for a remote client.
/// </summary>
/// <param name="LocalEndpoint">Local endpoint that received the connection.</param>
/// <param name="RemoteEndpoint">Remote endpoint (client address) that initiated the connection.</param>
public sealed record ClientInfo(IPEndPoint? LocalEndpoint, IPEndPoint? RemoteEndpoint);
