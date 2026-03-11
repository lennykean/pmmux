using Certes.Acme;

namespace Pmmux.Extensions.Acme.Abstractions;

/// <summary>
/// An ACME authorization for a domain.
/// </summary>
/// <param name="Domain">The domain that the authorization covers.</param>
/// <param name="Challenge">The challenge context chosen for validation.</param>
public record AuthorizationInfo(string Domain, IChallengeContext Challenge);
