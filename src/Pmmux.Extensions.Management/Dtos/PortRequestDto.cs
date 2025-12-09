using Mono.Nat;

namespace Pmmux.Extensions.Management.Dtos;

/// <summary>
/// A DTO object for port map add/remove requests.
/// </summary>
/// <param name="NetworkProtocol">The network protocol.</param>
/// <param name="LocalPort">The local port number.</param>
/// <param name="PublicPort">The public port number.</param>
public record PortRequestDto(Protocol NetworkProtocol, int? LocalPort, int? PublicPort);

