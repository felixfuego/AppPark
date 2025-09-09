using Park.Comun.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Park.Front.Services
{
    public class ColaboradorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ColaboradorService> _logger;
        private readonly AuthService _authService;

        public ColaboradorService(HttpClient httpClient, ILogger<ColaboradorService> logger, AuthService authService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _authService = authService;
        }

        private async Task SetAuthorizationHeader()
        {
            var token = await _authService.GetValidTokenAsync();
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("No hay token válido");
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<ColaboradorDto>> GetAllColaboradoresAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<ColaboradorDto>>("api/colaborador");
                return response ?? new List<ColaboradorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los colaboradores");
                throw;
            }
        }

        public async Task<List<ColaboradorDto>> GetActiveColaboradoresAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<ColaboradorDto>>("api/colaborador/active");
                return response ?? new List<ColaboradorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores activos");
                throw;
            }
        }

        public async Task<List<ColaboradorDto>> GetBlackListedColaboradoresAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<ColaboradorDto>>("api/colaborador/blacklist");
                return response ?? new List<ColaboradorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores en lista negra");
                throw;
            }
        }

        public async Task<List<ColaboradorDto>> GetColaboradoresByCompaniaAsync(int idCompania)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<ColaboradorDto>>($"api/colaborador/compania/{idCompania}");
                return response ?? new List<ColaboradorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores de la compañía {IdCompania}", idCompania);
                throw;
            }
        }

        public async Task<ColaboradorDto?> GetColaboradorByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<ColaboradorDto>($"api/colaborador/{id}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<ColaboradorDto?> GetColaboradorByIdentidadAsync(string identidad)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<ColaboradorDto>($"api/colaborador/identidad/{identidad}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaborador con identidad {Identidad}", identidad);
                throw;
            }
        }

        public async Task<ColaboradorDto> CreateColaboradorAsync(CreateColaboradorDto createColaboradorDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync("api/colaborador", createColaboradorDto);
                response.EnsureSuccessStatusCode();

                var colaborador = await response.Content.ReadFromJsonAsync<ColaboradorDto>();
                if (colaborador == null)
                {
                    throw new InvalidOperationException("No se pudo crear el colaborador");
                }

                return colaborador;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear colaborador");
                throw;
            }
        }

        public async Task<ColaboradorDto> UpdateColaboradorAsync(UpdateColaboradorDto updateColaboradorDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PutAsJsonAsync($"api/colaborador/{updateColaboradorDto.Id}", updateColaboradorDto);
                response.EnsureSuccessStatusCode();

                var colaborador = await response.Content.ReadFromJsonAsync<ColaboradorDto>();
                if (colaborador == null)
                {
                    throw new InvalidOperationException("No se pudo actualizar el colaborador");
                }

                return colaborador;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar colaborador con ID {Id}", updateColaboradorDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteColaboradorAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"api/colaborador/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateColaboradorAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PatchAsync($"api/colaborador/{id}/activate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateColaboradorAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PatchAsync($"api/colaborador/{id}/deactivate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ToggleBlackListAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PatchAsync($"api/colaborador/{id}/toggle-blacklist", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado de lista negra para colaborador con ID {Id}", id);
                throw;
            }
        }
    }
}
