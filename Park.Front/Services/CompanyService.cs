using Park.Comun.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Park.Front.Services
{
    public class CompanyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CompanyService> _logger;
        private readonly AuthService _authService;

        public CompanyService(HttpClient httpClient, ILogger<CompanyService> logger, AuthService authService)
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

        public async Task<List<CompanyDto>> GetAllCompaniesAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<CompanyDto>>("api/company");
                return response ?? new List<CompanyDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las empresas");
                throw;
            }
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<CompanyDto>($"api/company/{id}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresa con ID {Id}", id);
                throw;
            }
        }

        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync("api/company", createCompanyDto);
                response.EnsureSuccessStatusCode();

                var company = await response.Content.ReadFromJsonAsync<CompanyDto>();
                if (company == null)
                {
                    throw new InvalidOperationException("No se pudo crear la empresa");
                }

                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empresa");
                throw;
            }
        }

        public async Task<CompanyDto> UpdateCompanyAsync(UpdateCompanyDto updateCompanyDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PutAsJsonAsync($"api/company/{updateCompanyDto.Id}", updateCompanyDto);
                response.EnsureSuccessStatusCode();

                var company = await response.Content.ReadFromJsonAsync<CompanyDto>();
                if (company == null)
                {
                    throw new InvalidOperationException("No se pudo actualizar la empresa");
                }

                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar empresa con ID {Id}", updateCompanyDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"api/company/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar empresa con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateCompanyAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PatchAsync($"api/company/{id}/activate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar empresa con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateCompanyAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PatchAsync($"api/company/{id}/deactivate", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar empresa con ID {Id}", id);
                throw;
            }
        }

        public async Task<List<CentroDto>> GetCentrosBySitioAsync(int idSitio)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<CentroDto>>($"api/company/centros-by-sitio/{idSitio}");
                return response ?? new List<CentroDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros por sitio {IdSitio}", idSitio);
                throw;
            }
        }

        public async Task<string> GetDebugDataAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync("api/company/debug-data");
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de debug");
                throw;
            }
        }
    }
}
