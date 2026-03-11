using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Extensions.Acme.Models;

namespace Pmmux.Extensions.Acme;

internal sealed class AcmeStateStore(AcmeConfig config, ILoggerFactory loggerFactory)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    private readonly ILogger _logger = loggerFactory.CreateLogger("acme-state-store");
    private AcmeAccountInfo? _accountInfo;
    private AcmeStateData _stateData = new();

    private string StoragePath => config.AcmeStaging
        ? Path.Combine(config.AcmeStoragePath, "staging")
        : config.AcmeStoragePath;
    private string AccountFilePath => Path.Combine(StoragePath, "account.json");
    private string StateFilePath => Path.Combine(StoragePath, "state.json");
    private string CertificatesPath => Path.Combine(StoragePath, "certificates");

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(StoragePath);
        Directory.CreateDirectory(CertificatesPath);

        if (OperatingSystem.IsWindows())
        {
            RestrictDirectoryAccess(StoragePath);
            RestrictDirectoryAccess(CertificatesPath);
        }
        else
        {
            var unixDirMode = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute;

            File.SetUnixFileMode(StoragePath, unixDirMode);
            File.SetUnixFileMode(CertificatesPath, unixDirMode);
        }

        if (File.Exists(AccountFilePath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(AccountFilePath, cancellationToken).ConfigureAwait(false);

                _accountInfo = JsonSerializer.Deserialize<AcmeAccountInfo>(json, JsonOptions);

                _logger.LogDebug("loaded acme account from {Path}", AccountFilePath);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "corrupt account.json, will create a new ACME account");
                _accountInfo = null;
            }
        }

        if (File.Exists(StateFilePath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(StateFilePath, cancellationToken).ConfigureAwait(false);

                _stateData = JsonSerializer.Deserialize<AcmeStateData>(json, JsonOptions) ?? new();

                _logger.LogDebug("loaded acme state with {Count} certificates", _stateData.Certificates.Count);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "corrupt state.json, starting with fresh state");
                _stateData = new();
            }
        }
    }

    public AcmeAccountInfo? GetAccountInfo()
    {
        return _accountInfo;
    }

    public async Task SaveAccountInfoAsync(AcmeAccountInfo info, CancellationToken cancellationToken)
    {
        _accountInfo = info;

        await WriteFileAtomicAsync(AccountFilePath, info, sensitive: true, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("saved acme account to {Path}", AccountFilePath);
    }

    public AcmeStateData GetStateData()
    {
        return _stateData;
    }

    public async Task SaveStateDataAsync(AcmeStateData data, CancellationToken cancellationToken)
    {
        _stateData = data;

        await WriteFileAtomicAsync(StateFilePath, data, sensitive: false, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("saved acme state with {Count} certificates", data.Certificates.Count);
    }

    public async Task SaveCertificateAsync(
        string domain,
        byte[] pfxBytes,
        CancellationToken cancellationToken)
    {
        var pfxPath = GetSafeCertificatePath(domain) + ".pfx";

        await WriteBytesAtomicAsync(pfxPath, pfxBytes, sensitive: true, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("saved certificate for {Domain}", domain);
    }

    public async Task<byte[]?> LoadCertificateAsync(
        string domain,
        CancellationToken cancellationToken)
    {
        var pfxPath = GetSafeCertificatePath(domain) + ".pfx";

        if (!File.Exists(pfxPath))
        {
            return null;
        }

        return await File.ReadAllBytesAsync(pfxPath, cancellationToken).ConfigureAwait(false);
    }

    public void DeleteCertificate(string domain)
    {
        var pfxPath = GetSafeCertificatePath(domain) + ".pfx";

        if (File.Exists(pfxPath))
        {
            File.Delete(pfxPath);
        }

        _logger.LogDebug("deleted certificate for {Domain}", domain);
    }

    private static async Task WriteFileAtomicAsync<T>(
        string path,
        T data,
        bool sensitive,
        CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(data, JsonOptions);
        await WriteBytesAtomicAsync(path, Encoding.UTF8.GetBytes(json), sensitive, cancellationToken)
            .ConfigureAwait(false);
    }

    private static async Task WriteBytesAtomicAsync(
        string path,
        byte[] data,
        bool sensitive,
        CancellationToken cancellationToken)
    {
        var tmpPath = path + ".tmp";

        if (sensitive && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var options = new FileStreamOptions
            {
                Mode = FileMode.Create,
                Access = FileAccess.Write,
                Share = FileShare.None,
                Options = FileOptions.Asynchronous,
                UnixCreateMode = UnixFileMode.UserRead | UnixFileMode.UserWrite
            };
            await using var fs = new FileStream(tmpPath, options);
            await fs.WriteAsync(data, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            await File.WriteAllBytesAsync(tmpPath, data, cancellationToken).ConfigureAwait(false);
        }

        File.Move(tmpPath, path, overwrite: true);
    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    private static void RestrictDirectoryAccess(string path)
    {
        var security = new DirectorySecurity();
        security.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);

        var inheritFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

        security.AddAccessRule(new(
            WindowsIdentity.GetCurrent().User!,
            FileSystemRights.FullControl,
            inheritFlags,
            PropagationFlags.None,
            AccessControlType.Allow));

        security.AddAccessRule(new(
            new SecurityIdentifier(WellKnownSidType.LocalSystemSid, domainSid: null),
            FileSystemRights.FullControl,
            inheritFlags,
            PropagationFlags.None,
            AccessControlType.Allow));

        security.AddAccessRule(new(
            new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, domainSid: null),
            FileSystemRights.FullControl,
            inheritFlags,
            PropagationFlags.None,
            AccessControlType.Allow));

        new DirectoryInfo(path).SetAccessControl(security);
    }

    private string GetSafeCertificatePath(string domain)
    {
        var sanitized = SanitizeDomain(domain);
        if (string.IsNullOrWhiteSpace(sanitized))
        {
            throw new InvalidOperationException($"invalid domain '{domain}'");
        }

        var certsDir = Path.GetFullPath(CertificatesPath);
        var fullPath = Path.GetFullPath(Path.Combine(certsDir, sanitized));

        if (!fullPath.StartsWith(certsDir + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(fullPath, certsDir, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"invalid domain '{domain}'");
        }

        return fullPath;
    }

    private static string SanitizeDomain(string domain)
    {
        var name = domain.Replace("*", "_wildcard_");

        foreach (var c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
