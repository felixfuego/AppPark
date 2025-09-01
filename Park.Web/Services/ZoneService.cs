using Microsoft.Extensions.Logging;
using Park.Web.Models;
using System.Net.Http.Json;

namespace Park.Web.Services;

public class ZoneService : BaseApiService<Zone, CreateZone, UpdateZone>, IZoneService
{
    public ZoneService(HttpClientService httpClientService, ILogger<ZoneService> logger) 
        : base(httpClientService, logger, "api/zone")
    {
    }

    public async Task<Zone?> GetByNameAsync(string name)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/name/{name}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Zone>();
            }
            
            _logger.LogWarning("Error al obtener zona por nombre {Name}: {StatusCode}", name, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener zona por nombre {Name}", name);
            return null;
        }
    }

    public async Task<IEnumerable<Gate>?> GetZoneGatesAsync(int zoneId)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/{zoneId}/gates");
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
