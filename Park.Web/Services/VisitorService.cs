using Microsoft.Extensions.Logging;
using Park.Web.Models;
using System.Net.Http.Json;

namespace Park.Web.Services;

public class VisitorService : BaseApiService<Visitor, CreateVisitor, UpdateVisitor>, IVisitorService
{
    public VisitorService(HttpClientService httpClientService, ILogger<VisitorService> logger) 
        : base(httpClientService, logger, "api/visitor")
    {
    }

    public async Task<Visitor?> GetByEmailAsync(string email)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/email/{email}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Visitor>();
            }
            
            _logger.LogWarning("Error al obtener visitante por email {Email}: {StatusCode}", email, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visitante por email {Email}", email);
            return null;
        }
    }

    public async Task<Visitor?> GetByDocumentAsync(string documentType, string documentNumber)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/document/{documentType}/{documentNumber}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Visitor>();
            }
            
            _logger.LogWarning("Error al obtener visitante por documento {Type}/{Number}: {StatusCode}", documentType, documentNumber, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visitante por documento {Type}/{Number}", documentType, documentNumber);
            return null;
        }
    }

    public async Task<Visitor?> GetByIdNumberAsync(string idNumber)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/id-number/{idNumber}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Visitor>();
            }
            
            _logger.LogWarning("Error al obtener visitante por número de identificación {IdNumber}: {StatusCode}", idNumber, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visitante por número de identificación {IdNumber}", idNumber);
            return null;
        }
    }
}
