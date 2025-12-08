namespace Pmmux.Abstractions;

/// <summary>
/// Factory to create <see cref="IClientWriter"/> instances.
/// </summary>
public interface IClientWriterFactory
{
    /// <summary>
    /// Create a client writer for the specified client.
    /// </summary>
    /// <param name="clientInfo">The client to create a writer for.</param>
    /// <returns>An instance of <see cref="IClientWriter"/> for the client.</returns>
    IClientWriter CreateWriter(ClientInfo clientInfo);
}
