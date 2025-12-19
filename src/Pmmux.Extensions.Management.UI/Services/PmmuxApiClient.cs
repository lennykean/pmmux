using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Pmmux.Extensions.Management.Models;
using Pmmux.Extensions.Tls.Abstractions;

namespace Pmmux.Extensions.Management.UI.Services;

internal class PmmuxApiClient(HttpClient http, ILogger<PmmuxApiClient> logger)
{
    internal sealed class ApiException(string message, HttpStatusCode statusCode) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }

    private readonly HttpClient _http = http;
    private readonly ILogger<PmmuxApiClient> _logger = logger;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    private async Task<string?> TryReadContentAsync(HttpResponseMessage response)
    {
        try
        {
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read response content");
            return null;
        }
    }

    private async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var detail = await TryReadContentAsync(response).ConfigureAwait(false);

        var message = response.StatusCode switch
        {
            HttpStatusCode.BadRequest => "Bad request",
            HttpStatusCode.NotFound => "Not found",
            HttpStatusCode.Conflict => "Conflict",
            HttpStatusCode.UnprocessableEntity => "Invalid data",
            HttpStatusCode.InternalServerError => "Server error",
            HttpStatusCode.ServiceUnavailable => "Service unavailable",
            _ => $"Request failed ({(int)response.StatusCode})"
        };

        if (!string.IsNullOrWhiteSpace(detail) && detail.Length < 200)
        {
            message += $": {detail}";
        }

        throw new ApiException(message, response.StatusCode);
    }

    public async Task<bool> HealthCheckAsync()
    {
        try
        {
            var response = await _http.GetAsync("/healthz").ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Health check failed");
            return false;
        }
    }

    public async Task<List<BackendStatusInfoDto>> GetBackendsAsync(Mono.Nat.Protocol protocol)
    {
        var url = $"/api/backends?networkProtocol={protocol}";

        return await _http.GetFromJsonAsync<List<BackendStatusInfoDto>>(url, _jsonOptions).ConfigureAwait(false) ?? [];
    }

    public async Task CreateBackendAsync(string protocolName, string name, BackendRequest request)
    {
        var url = $"/api/backends/{protocolName}/{name}?networkProtocol={request.NetworkProtocol}";
        var response = await _http.PostAsJsonAsync(url, request.Parameters, _jsonOptions).ConfigureAwait(false);

        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task UpdateBackendAsync(string protocolName, string name, BackendRequest request)
    {
        var url = $"/api/backends/{protocolName}/{name}?networkProtocol={request.NetworkProtocol}";
        var response = await _http.PutAsJsonAsync(url, request.Parameters, _jsonOptions).ConfigureAwait(false);

        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task DeleteBackendAsync(string protocolName, string name, Mono.Nat.Protocol networkProtocol)
    {
        var response = await _http.DeleteAsync(
            $"/api/backends/{protocolName}/{name}?networkProtocol={networkProtocol}")
            .ConfigureAwait(false);

        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task<List<HealthCheckSpecDto>> GetHealthChecksAsync()
    {
        return await _http.GetFromJsonAsync<List<HealthCheckSpecDto>>("/api/health-checks", _jsonOptions)
            .ConfigureAwait(false) ?? [];
    }

    public async Task CreateHealthCheckAsync(HealthCheckSpecDto spec)
    {
        var response = await _http.PostAsJsonAsync("/api/health-checks", spec, _jsonOptions).ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task DeleteHealthCheckAsync(HealthCheckSpecDto spec)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/health-checks")
        {
            Content = JsonContent.Create(spec, options: _jsonOptions)
        };
        var response = await _http.SendAsync(request).ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task<List<ListenerInfoDto>> GetListenersAsync()
    {
        return await _http.GetFromJsonAsync<List<ListenerInfoDto>>("/api/listeners", _jsonOptions)
            .ConfigureAwait(false) ?? [];
    }

    public async Task<ListenerInfoDto?> CreateListenerAsync(ListenerRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/listeners", request, _jsonOptions).ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<ListenerInfoDto>(_jsonOptions).ConfigureAwait(false);
    }

    public async Task DeleteListenerAsync(Mono.Nat.Protocol protocol, int port)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/listeners")
        {
            Content = JsonContent.Create(
                new ListenerRequest { NetworkProtocol = protocol, Port = port },
                options: _jsonOptions)
        };
        var response = await _http.SendAsync(request).ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task<List<PortMapInfoDto>> GetPortMapsAsync()
    {
        return await _http.GetFromJsonAsync<List<PortMapInfoDto>>("/api/port-maps", _jsonOptions)
            .ConfigureAwait(false) ?? [];
    }

    public async Task<NatDeviceInfoDto?> GetNatDeviceAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<NatDeviceInfoDto>("/api/port-maps/device", _jsonOptions)
                .ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to get NAT device");
            return null;
        }
    }

    public async Task<PortMapInfoDto?> CreatePortMapAsync(PortMapRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/port-maps", request, _jsonOptions).ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<PortMapInfoDto>(_jsonOptions).ConfigureAwait(false);
    }

    public async Task DeletePortMapAsync(Mono.Nat.Protocol protocol, int localPort, int publicPort)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/port-maps")
        {
            Content = JsonContent.Create(new PortMapRequest
            {
                NetworkProtocol = protocol,
                LocalPort = localPort,
                PublicPort = publicPort
            }, options: _jsonOptions)
        };
        var response = await _http.SendAsync(request).ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task<List<string>> GetCertificatesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<string>>("/api/tls/certificates", _jsonOptions)
                .ConfigureAwait(false) ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to get certificates");
            return [];
        }
    }

    public async Task<bool> IsTlsAvailableAsync()
    {
        try
        {
            var response = await _http.GetAsync("/api/tls/certificates").ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to check TLS availability");
            return false;
        }
    }

    public async Task UploadCertificateAsync(string name, CertificateType type, byte[] data, string? password = null)
    {
        var url = $"/api/tls/certificates?certificateName={Uri.EscapeDataString(name)}&certificateType={type}";
        if (!string.IsNullOrEmpty(password))
        {
            url += $"&password={Uri.EscapeDataString(password)}";
        }

        using var content = new ByteArrayContent(data);
        var response = await _http.PostAsync(url, content).ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task DeleteCertificateAsync(string name)
    {
        var response = await _http.DeleteAsync($"/api/tls/certificates?certificateName={Uri.EscapeDataString(name)}")
            .ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task<List<CertificateMappingDto>> GetCertificateMappingsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<CertificateMappingDto>>(
                "/api/tls/certificate-mappings",
                _jsonOptions)
                .ConfigureAwait(false) ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to get certificate mappings");
            return [];
        }
    }

    public async Task CreateCertificateMappingAsync(CertificateMappingDto mapping)
    {
        var response = await _http.PostAsJsonAsync("/api/tls/certificate-mappings", mapping, _jsonOptions)
            .ConfigureAwait(false);
        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }

    public async Task DeleteCertificateMappingAsync(string hostname)
    {
        var response = await _http.DeleteAsync(
            $"/api/tls/certificate-mappings?hostname={Uri.EscapeDataString(hostname)}")
            .ConfigureAwait(false);

        await EnsureSuccessAsync(response).ConfigureAwait(false);
    }
}
