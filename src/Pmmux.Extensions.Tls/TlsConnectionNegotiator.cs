using System;
using System.ComponentModel;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Abstractions;
using Pmmux.Extensions.Tls.Abstractions;

using Result = Pmmux.Abstractions.IClientConnectionNegotiator.Result;

namespace Pmmux.Extensions.Tls;

internal sealed class TlsConnectionNegotiator(
    ICertificateManager certificateManager,
    TlsConfig tlsConfig,
    ILoggerFactory loggerFactory) : IClientConnectionNegotiator
{
    private const int CONTENT_TYPE_TLS = 0x16;
    private const int CLIENT_HELLO = 0x01;
    private const int CONTENT_TYPE_INDEX = 0;
    private const int RECORD_VERSION_INDEX = 1;
    private const int MESSAGE_TYPE_INDEX = 5;

    private readonly ILogger _logger = loggerFactory.CreateLogger("tls-connection-negotiator");

    public string Name => "tls";

    public async Task<Result> NegotiateAsync(
        ClientConnectionContext context,
        Func<Task<Result>> next,
        CancellationToken cancellationToken = default)
    {
        if ((!context.ClientConnectionStream.CanSeek || context.ClientConnectionStream.Position == 0) &&
            await DetectTlsHandshakeAsync(context.ClientConnection, cancellationToken))
        {
            _logger.LogTrace("tls handshake started [protocols={SupportedProtocols}]", tlsConfig.TlsProtocols);

            var sslStream = new SslStream(context.ClientConnectionStream, leaveInnerStreamOpen: false);
            var authOptions = new SslServerAuthenticationOptions
            {
                ClientCertificateRequired = false,
                CertificateRevocationCheckMode = X509RevocationMode.NoCheck,
                ServerCertificateSelectionCallback = (sender, hostname) =>
                {
                    _logger.LogTrace("selecting server certificate for SNI hostname: {HostName}", hostname);

                    if (hostname is not null)
                    {
                        context.Properties["tls.sni"] = hostname;
                    }

                    if (!certificateManager.TryGetCertificate(hostname, out var certificate) &&
                        !certificateManager.TryGetCertificate("localhost", out certificate))
                    {
                        throw new Exception($"error selecting certificate for hostname '{hostname}'");
                    }

                    return certificate;
                }
            };
            if (tlsConfig.TlsProtocols is not null)
            {
                authOptions.EnabledSslProtocols = tlsConfig.TlsProtocols.Value;
            }
            try
            {
                await sslStream.AuthenticateAsServerAsync(authOptions, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex.InnerException is Win32Exception wEx && (
                unchecked((uint)wEx.NativeErrorCode) == 0x80090327 ||
                unchecked((uint)wEx.NativeErrorCode) == 0x80090325 ||
                unchecked((uint)wEx.NativeErrorCode) == 0x80090328 ||
                unchecked((uint)wEx.NativeErrorCode) == 0x80090331))
            {
                return Result.Reject("client rejected tls handshake");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "failed to authenticate as server");

                return Result.Reject("failed to authenticate as server");
            }

            context.Properties["tls"] = "true";
            context.Properties["tls.protocol"] = sslStream.SslProtocol.ToString().ToLowerInvariant();
            context.Properties["tls.cipher-suite"] = sslStream.NegotiatedCipherSuite.ToString().ToLowerInvariant();
            context.Properties["tls.application-protocol"] = sslStream.NegotiatedApplicationProtocol.ToString()
                .ToLowerInvariant();

            context.ClientConnectionStream = sslStream;
        }
        else
        {
            context.Properties["tls"] = "false";

            _logger.LogTrace("tls handshake not detected");

            if (tlsConfig.TlsEnforce)
            {
                return Result.Reject("tls is required");
            }
        }
        return await next().ConfigureAwait(false);
    }

    private async Task<bool> DetectTlsHandshakeAsync(Socket clientConnection, CancellationToken cancellationToken)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(tlsConfig.TlsHelloTimeout);

        while (clientConnection.Available < 6 && !cts.Token.IsCancellationRequested)
        {
            await Task.Delay(1, cancellationToken).ConfigureAwait(false);
        }

        if (clientConnection.Available < 6)
        {
            return false;
        }

        var buffer = new byte[6];

        await clientConnection.ReceiveAsync(buffer, SocketFlags.Peek, cancellationToken).ConfigureAwait(false);

        if (buffer[CONTENT_TYPE_INDEX] != CONTENT_TYPE_TLS)
        {
            return false;
        }
        if (buffer[RECORD_VERSION_INDEX] != 0x03)
        {
            return false;
        }
        if (buffer[RECORD_VERSION_INDEX + 1] < 0x01)
        {
            return false;
        }
        if (buffer[MESSAGE_TYPE_INDEX] != CLIENT_HELLO)
        {
            return false;
        }
        return true;
    }
}

