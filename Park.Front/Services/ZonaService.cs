using Park.Comun.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Park.Front.Services
{
    public class ZonaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ZonaService> _logger;
        private readonly AuthService _authService;

        public ZonaService(HttpClient httpClient, ILogger<ZonaService> logger, AuthService authService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _authService = authService;
        }

        public async Task<List<ZonaDto>> GetAllZonasAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<ZonaDto>>("api/zona");
                return response ?? new List<ZonaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las zonas");
                throw;
            }
        }

        public async Task<List<ZonaDto>> GetActiveZonasAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<ZonaDto>>("api/zona/active");
                return response ?? new List<ZonaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener zonas activas");
                throw;
            }
        }

        public async Task<List<ZonaDto>> GetZonasBySitioAsync(int idSitio)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<ZonaDto>>($"api/zona/sitio/{idSitio}");
                return response ?? new List<ZonaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener zonas del sitio {IdSitio}", idSitio);
                throw;
            }
        }

        public async Task<ZonaDto> CreateZonaAsync(CreateZonaDto createZonaDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/zona", createZonaDto);
                response.EnsureSuccessStatusCode();
                
                var zona = await response.Content.ReadFromJsonAsync<ZonaDto>();
                if (zona == null)
                {
                    throw new InvalidOperationException("No se pudo crear la zona");
                }
                
                return zona;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear zona: {Nombre}", createZonaDto.Nombre);
                throw;
            }
        }

        public async Task<ZonaDto> UpdateZonaAsync(UpdateZonaDto updateZonaDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"api/zona/{updateZonaDto.Id}", updateZonaDto);
                response.EnsureSuccessStatusCode();
                
                var zona = await response.Content.ReadFromJsonAsync<ZonaDto>();
                if (zona == null)
                {
                    throw new InvalidOperationException("No se pudo actualizar la zona");
                }
                
                return zona;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar zona con ID {Id}", updateZonaDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteZonaAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/zona/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"No se puede eliminar la zona: {errorContent}");
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (InvalidOperationException)
            {
                // Re-lanzar excepciones de validación sin modificar
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar zona con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateZonaAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PatchAsync($"api/zona/{id}/activate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar zona con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateZonaAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PatchAsync($"api/zona/{id}/deactivate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar zona con ID {Id}", id);
                throw;
            }
        }
    }
}
