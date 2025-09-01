using Microsoft.Extensions.Logging;
using Park.Web.Models;
using System.Net.Http.Json;

namespace Park.Web.Services;

public class CompanyService : BaseApiService<Company, CreateCompany, UpdateCompany>, ICompanyService
{
    public CompanyService(HttpClientService httpClientService, ILogger<CompanyService> logger) 
        : base(httpClientService, logger, "api/company")
    {
    }

    public async Task<Company?> GetByNameAsync(string name)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/name/{name}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Company>();
            }
            
            _logger.LogWarning("Error al obtener empresa por nombre {Name}: {StatusCode}", name, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener empresa por nombre {Name}", name);
            return null;
        }
    }

    public async Task<Company?> GetByEmailAsync(string email)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/email/{email}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Company>();
            }
            
            _logger.LogWarning("Error al obtener empresa por email {Email}: {StatusCode}", email, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener empresa por email {Email}", email);
            return null;
        }
    }

    public async Task<IEnumerable<Zone>?> GetCompanyZonesAsync(int companyId)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/{companyId}/zones");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Zone>>();
            }
            
            _logger.LogWarning("Error al obtener zonas de la empresa {CompanyId}: {StatusCode}", companyId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener zonas de la empresa {CompanyId}", companyId);
            return null;
        }
    }
}
