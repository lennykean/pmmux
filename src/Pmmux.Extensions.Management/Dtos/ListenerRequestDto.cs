using Mono.Nat;

namespace Pmmux.Extensions.Management.Dtos;

/// <summary>
/// A DTO object for listener add/remove requests.
/// </summary>
/// <param name="NetworkProtocol">The network protocol.</param>
/// <param name="Port">The port number.</param>
public record ListenerRequestDto(Protocol NetworkProtocol, int Port);

