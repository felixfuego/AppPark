using Park.Comun.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Park.Front.Services
{
    public class VisitorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VisitorService> _logger;
        private readonly AuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions;

        public VisitorService(HttpClient httpClient, ILogger<VisitorService> logger, AuthService authService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _authService = authService;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<VisitorDto>> GetAllVisitorsAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<VisitorDto>>("api/visitor");
                return response ?? new List<VisitorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los visitantes");
                throw;
            }
        }

        public async Task<List<VisitorDto>> GetActiveVisitorsAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<VisitorDto>>("api/visitor/active");
                return response ?? new List<VisitorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitantes activos");
                throw;
            }
        }

        public async Task<VisitorDto?> GetVisitorByIdAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<VisitorDto>($"api/visitor/{id}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con ID {Id}", id);
                throw;
            }
        }

        public async Task<VisitorDto?> GetVisitorByEmailAsync(string email)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<VisitorDto>($"api/visitor/email/{email}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con email {Email}", email);
                throw;
            }
        }

        public async Task<VisitorDto?> GetVisitorByDocumentAsync(string documentType, string documentNumber)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<VisitorDto>($"api/visitor/document/{documentType}/{documentNumber}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con documento {DocumentType} {DocumentNumber}", documentType, documentNumber);
                throw;
            }
        }

        public async Task<List<VisitorDto>> GetVisitorsByCompanyAsync(string company)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<VisitorDto>>($"api/visitor/company/{company}");
                return response ?? new List<VisitorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitantes de la empresa {Company}", company);
                throw;
            }
        }

        public async Task<PagedResultDto<VisitorDto>> SearchVisitorsAsync(VisitorSearchDto searchDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(searchDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/visitor/search", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PagedResultDto<VisitorDto>>(responseContent, _jsonOptions) ?? new PagedResultDto<VisitorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar visitantes");
                throw;
            }
        }

        public async Task<VisitorDto> CreateVisitorAsync(CreateVisitorDto createVisitorDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(createVisitorDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/visitor", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<VisitorDto>(responseContent, _jsonOptions) ?? new VisitorDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visitante");
                throw;
            }
        }

        public async Task<VisitorDto> UpdateVisitorAsync(UpdateVisitorDto updateVisitorDto)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(updateVisitorDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/visitor/{updateVisitorDto.Id}", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<VisitorDto>(responseContent, _jsonOptions) ?? new VisitorDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar visitante");
                throw;
            }
        }

        public async Task<bool> DeleteVisitorAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/visitor/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar visitante");
                throw;
            }
        }

        public async Task<bool> ActivateVisitorAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync($"api/visitor/{id}/activate", null);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar visitante");
                throw;
            }
        }

        public async Task<bool> DeactivateVisitorAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync($"api/visitor/{id}/deactivate", null);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar visitante");
                throw;
            }
        }
    }
}
