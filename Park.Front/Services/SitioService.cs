using Park.Comun.DTOs;
using System.Net.Http.Json;

namespace Park.Front.Services
{
    public class SitioService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SitioService> _logger;
        private readonly AuthService _authService;

        public SitioService(HttpClient httpClient, ILogger<SitioService> logger, AuthService authService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _authService = authService;
        }

        public async Task<List<SitioDto>> GetAllSitiosAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<SitioDto>>("api/sitio");
                return response ?? new List<SitioDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los sitios");
                throw;
            }
        }

        public async Task<List<SitioDto>> GetActiveSitiosAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<SitioDto>>("api/sitio/active");
                return response ?? new List<SitioDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener sitios activos");
                throw;
            }
        }

        public async Task<SitioDto?> GetSitioByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SitioDto>($"api/sitio/{id}");
                return response;
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener sitio con ID {Id}", id);
                throw;
            }
        }

        public async Task<SitioDto> CreateSitioAsync(CreateSitioDto createSitioDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/sitio", createSitioDto);
                response.EnsureSuccessStatusCode();
                
                var sitio = await response.Content.ReadFromJsonAsync<SitioDto>();
                if (sitio == null)
                {
                    throw new InvalidOperationException("No se pudo crear el sitio");
                }
                
                return sitio;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear sitio: {Nombre}", createSitioDto.Nombre);
                throw;
            }
        }

        public async Task<SitioDto> UpdateSitioAsync(UpdateSitioDto updateSitioDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"api/sitio/{updateSitioDto.Id}", updateSitioDto);
                response.EnsureSuccessStatusCode();
                
                var sitio = await response.Content.ReadFromJsonAsync<SitioDto>();
                if (sitio == null)
                {
                    throw new InvalidOperationException("No se pudo actualizar el sitio");
                }
                
                return sitio;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar sitio con ID {Id}", updateSitioDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteSitioAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/sitio/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"No se puede eliminar el sitio: {errorContent}");
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
                _logger.LogError(ex, "Error al eliminar sitio con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateSitioAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PatchAsync($"api/sitio/{id}/activate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar sitio con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateSitioAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PatchAsync($"api/sitio/{id}/deactivate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar sitio con ID {Id}", id);
                throw;
            }
        }
    }
}
