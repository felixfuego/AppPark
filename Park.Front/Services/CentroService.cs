using Park.Comun.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Park.Front.Services
{
    public class CentroService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CentroService> _logger;
        private readonly AuthService _authService;

        public CentroService(HttpClient httpClient, ILogger<CentroService> logger, AuthService authService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _authService = authService;
        }

        public async Task<List<CentroDto>> GetAllCentrosAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<CentroDto>>("api/centro");
                return response ?? new List<CentroDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los centros");
                throw;
            }
        }

        public async Task<List<CentroDto>> GetActiveCentrosAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<CentroDto>>("api/centro/active");
                return response ?? new List<CentroDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros activos");
                throw;
            }
        }

        public async Task<CentroDto> CreateCentroAsync(CreateCentroDto createCentroDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/centro", createCentroDto);
                response.EnsureSuccessStatusCode();
                
                var centro = await response.Content.ReadFromJsonAsync<CentroDto>();
                if (centro == null)
                {
                    throw new InvalidOperationException("No se pudo crear el centro");
                }
                
                return centro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear centro: {Nombre}", createCentroDto.Nombre);
                throw;
            }
        }

        public async Task<CentroDto> UpdateCentroAsync(UpdateCentroDto updateCentroDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"api/centro/{updateCentroDto.Id}", updateCentroDto);
                response.EnsureSuccessStatusCode();
                
                var centro = await response.Content.ReadFromJsonAsync<CentroDto>();
                if (centro == null)
                {
                    throw new InvalidOperationException("No se pudo actualizar el centro");
                }
                
                return centro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar centro con ID {Id}", updateCentroDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteCentroAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/centro/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"No se puede eliminar el centro: {errorContent}");
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
                _logger.LogError(ex, "Error al eliminar centro con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateCentroAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PatchAsync($"api/centro/{id}/activate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar centro con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateCentroAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PatchAsync($"api/centro/{id}/deactivate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar centro con ID {Id}", id);
                throw;
            }
        }
    }
}
