using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace Park.Web.Services;

public abstract class BaseApiService<T, TCreate, TUpdate> : IApiService<T, TCreate, TUpdate> 
    where T : class 
    where TCreate : class 
    where TUpdate : class
{
    protected readonly HttpClientService _httpClientService;
    protected readonly ILogger _logger;
    protected readonly string _baseUrl;

    protected BaseApiService(HttpClientService httpClientService, ILogger logger, string baseUrl)
    {
        _httpClientService = httpClientService;
        _logger = logger;
        _baseUrl = baseUrl;
    }

    public virtual async Task<IEnumerable<T>?> GetAllAsync()
    {
        try
        {
            var response = await _httpClientService.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
            }
            
            _logger.LogWarning("Error al obtener datos: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los registros de {EntityType}", typeof(T).Name);
            return null;
        }
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            
            _logger.LogWarning("Error al obtener registro con ID {Id}: {StatusCode}", id, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener registro con ID {Id} de {EntityType}", id, typeof(T).Name);
            return null;
        }
    }

    public virtual async Task<T?> CreateAsync(TCreate createDto)
    {
        try
        {
            _logger.LogInformation("Iniciando creación de {EntityType} con datos: {@CreateDto}", typeof(T).Name, createDto);
            
            var jsonContent = JsonContent.Create(createDto);
            var response = await _httpClientService.PostAsync(_baseUrl, jsonContent);
            
            _logger.LogInformation("Respuesta del servidor: StatusCode={StatusCode}, IsSuccessStatusCode={IsSuccess}", 
                response.StatusCode, response.IsSuccessStatusCode);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                _logger.LogInformation("Registro creado exitosamente: {@Result}", result);
                return result;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Error al crear registro: StatusCode={StatusCode}, Content={Content}", 
                response.StatusCode, errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear registro de {EntityType}", typeof(T).Name);
            return null;
        }
    }

    public virtual async Task<T?> UpdateAsync(int id, TUpdate updateDto)
    {
        try
        {
            var jsonContent = JsonContent.Create(updateDto);
            var response = await _httpClientService.PutAsync($"{_baseUrl}/{id}", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            
            _logger.LogWarning("Error al actualizar registro con ID {Id}: {StatusCode}", id, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar registro con ID {Id} de {EntityType}", id, typeof(T).Name);
            return null;
        }
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await _httpClientService.DeleteAsync($"{_baseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            
            _logger.LogWarning("Error al eliminar registro con ID {Id}: {StatusCode}", id, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar registro con ID {Id} de {EntityType}", id, typeof(T).Name);
            return false;
        }
    }
}
