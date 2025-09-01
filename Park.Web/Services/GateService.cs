using Microsoft.Extensions.Logging;
using Park.Web.Models;
using System.Net.Http.Json;

namespace Park.Web.Services;

public class GateService : BaseApiService<Gate, CreateGate, UpdateGate>, IGateService
{
    public GateService(HttpClientService httpClientService, ILogger<GateService> logger) 
        : base(httpClientService, logger, "api/gate")
    {
    }

    public async Task<Gate?> GetByNameAsync(string name)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/name/{name}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Gate>();
            }
            
            _logger.LogWarning("Error al obtener puerta por nombre {Name}: {StatusCode}", name, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener puerta por nombre {Name}", name);
            return null;
        }
    }

    public async Task<Gate?> GetByNumberAsync(string gateNumber)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/number/{gateNumber}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Gate>();
            }
            
            _logger.LogWarning("Error al obtener puerta por número {Number}: {StatusCode}", gateNumber, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener puerta por número {Number}", gateNumber);
            return null;
        }
    }

    public async Task<Gate?> GetByAccessCodeAsync(string accessCode)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/access-code/{accessCode}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Gate>();
            }
            
            _logger.LogWarning("Error al obtener puerta por código de acceso {AccessCode}: {StatusCode}", accessCode, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener puerta por código de acceso {AccessCode}", accessCode);
            return null;
        }
    }

    public async Task<IEnumerable<Gate>?> GetByZoneAsync(int zoneId)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/zone/{zoneId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Gate>>();
            }
            
            _logger.LogWarning("Error al obtener puertas de la zona {ZoneId}: {StatusCode}", zoneId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener puertas de la zona {ZoneId}", zoneId);
            return null;
        }
    }
}
