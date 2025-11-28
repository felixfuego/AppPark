using Park.Comun.DTOs;
using Park.Comun.Enums;
using System.Net.Http.Json;

namespace Park.Front.Services
{
    public class VisitaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VisitaService> _logger;
        private readonly AuthService _authService;

        public VisitaService(HttpClient httpClient, ILogger<VisitaService> logger, AuthService authService)
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

        // Métodos básicos de CRUD
        public async Task<List<VisitaDto>> GetAllVisitasAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>("api/visita");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las visitas");
                throw;
            }
        }

        public async Task<VisitaDto?> GetVisitaByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<VisitaDto>($"api/visita/{id}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita con ID {Id}", id);
                throw;
            }
        }

        public async Task<VisitaDto?> GetVisitaByNumeroSolicitudAsync(string numeroSolicitud)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<VisitaDto>($"api/visita/numero/{numeroSolicitud}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita con número de solicitud {NumeroSolicitud}", numeroSolicitud);
                throw;
            }
        }

        public async Task<List<VisitaDto>> GetVisitasByUserAsync(int userId)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>($"api/visita/user/{userId}");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del usuario {UserId}", userId);
                throw;
            }
        }

        public async Task<List<VisitaDto>> GetVisitasByGuardiaZonaAsync(int guardiaId)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>($"api/visita/guardia-zona/{guardiaId}");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas por zona del guardia {GuardiaId}", guardiaId);
                throw;
            }
        }

        public async Task<List<VisitaDto>> GetVisitasByCompaniaAsync(int idCompania)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>($"api/visita/compania/{idCompania}");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas de la compañía {IdCompania}", idCompania);
                throw;
            }
        }

        public async Task<List<VisitaDto>> GetVisitasByCentroAsync(int idCentro)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>($"api/visita/centro/{idCentro}");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del centro {IdCentro}", idCentro);
                throw;
            }
        }

        public async Task<List<VisitaDto>> GetVisitasByEstadoAsync(VisitStatus estado)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>($"api/visita/estado/{estado}");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas con estado {Estado}", estado);
                throw;
            }
        }

        public async Task<List<VisitaDto>> GetVisitasByFechaAsync(DateTime fecha)
        {
            try
            {
                await SetAuthorizationHeader();
                var fechaStr = fecha.ToString("yyyy-MM-dd");
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>($"api/visita/fecha/{fechaStr}");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas de la fecha {Fecha}", fecha);
                throw;
            }
        }

        public async Task<List<VisitaDto>> GetVisitasActivasAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>("api/visita/activas");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas activas");
                throw;
            }
        }

        public async Task<List<VisitaDto>> GetVisitasExpiradasAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaDto>>("api/visita/expiradas");
                return response ?? new List<VisitaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas expiradas");
                throw;
            }
        }

        // Métodos de búsqueda
        public async Task<PagedResultDto<VisitaDto>> SearchVisitasAsync(VisitaSearchDto searchDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync("api/visita/search", searchDto);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<PagedResultDto<VisitaDto>>();
                if (result == null)
                {
                    throw new InvalidOperationException("No se pudo realizar la búsqueda de visitas");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar visitas");
                throw;
            }
        }

        // Métodos de creación y actualización
        public async Task<VisitaDto> CreateVisitaAsync(CreateVisitaDto createVisitaDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync("api/visita", createVisitaDto);
                response.EnsureSuccessStatusCode();

                var visita = await response.Content.ReadFromJsonAsync<VisitaDto>();
                if (visita == null)
                {
                    throw new InvalidOperationException("No se pudo crear la visita");
                }

                return visita;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visita");
                throw;
            }
        }

        public async Task<VisitaDto> UpdateVisitaAsync(UpdateVisitaDto updateVisitaDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PutAsJsonAsync($"api/visita/{updateVisitaDto.Id}", updateVisitaDto);
                response.EnsureSuccessStatusCode();

                var visita = await response.Content.ReadFromJsonAsync<VisitaDto>();
                if (visita == null)
                {
                    throw new InvalidOperationException("No se pudo actualizar la visita");
                }

                return visita;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar visita con ID {Id}", updateVisitaDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteVisitaAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"api/visita/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar visita con ID {Id}", id);
                throw;
            }
        }

        // Métodos para visitas masivas
        public async Task<VisitaMasivaDto> CreateVisitaMasivaAsync(CreateVisitaMasivaDto createVisitaMasivaDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync("api/visita/masiva", createVisitaMasivaDto);
                response.EnsureSuccessStatusCode();

                var visitaMasiva = await response.Content.ReadFromJsonAsync<VisitaMasivaDto>();
                if (visitaMasiva == null)
                {
                    throw new InvalidOperationException("No se pudo crear la visita masiva");
                }

                return visitaMasiva;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visita masiva");
                throw;
            }
        }

        public async Task<List<VisitaMasivaDto>> GetVisitasMasivasAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<List<VisitaMasivaDto>>("api/visita/masivas");
                return response ?? new List<VisitaMasivaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas masivas");
                throw;
            }
        }

        public async Task<VisitaMasivaDto?> GetVisitaMasivaByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetFromJsonAsync<VisitaMasivaDto>($"api/visita/masiva/{id}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita masiva con ID {Id}", id);
                throw;
            }
        }

        // Métodos para operaciones de guardia
        public async Task<VisitaDto> CheckInVisitaAsync(VisitaCheckInDto checkInDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync($"api/visita/{checkInDto.Id}/checkin", checkInDto);
                response.EnsureSuccessStatusCode();

                var visita = await response.Content.ReadFromJsonAsync<VisitaDto>();
                if (visita == null)
                {
                    throw new InvalidOperationException("No se pudo realizar el check-in");
                }

                return visita;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar check-in para visita con ID {Id}", checkInDto.Id);
                throw;
            }
        }

        public async Task<VisitaDto> CheckOutVisitaAsync(VisitaCheckOutDto checkOutDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync($"api/visita/{checkOutDto.Id}/checkout", checkOutDto);
                response.EnsureSuccessStatusCode();

                var visita = await response.Content.ReadFromJsonAsync<VisitaDto>();
                if (visita == null)
                {
                    throw new InvalidOperationException("No se pudo realizar el check-out");
                }

                return visita;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar check-out para visita con ID {Id}", checkOutDto.Id);
                throw;
            }
        }

        public async Task<bool> CancelarVisitaAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsync($"api/visita/{id}/cancel", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar visita con ID {Id}", id);
                throw;
            }
        }

        // Método para subir archivo Excel
        public async Task<VisitaMasivaDto> UploadExcelVisitasAsync(MultipartFormDataContent formData)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsync("api/visita/upload-excel", formData);
                response.EnsureSuccessStatusCode();

                var visitaMasiva = await response.Content.ReadFromJsonAsync<VisitaMasivaDto>();
                if (visitaMasiva == null)
                {
                    throw new InvalidOperationException("No se pudo procesar el archivo Excel");
                }

                return visitaMasiva;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir archivo Excel de visitas");
                throw;
            }
        }

        // Métodos de utilidad
        public async Task<bool> ExpirarVisitasAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.PostAsync("api/visita/expirar", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al expirar visitas");
                throw;
            }
        }

        public string GenerarNumeroSolicitud()
        {
            var fecha = DateTime.Now;
            var numero = DateTime.Now.Ticks.ToString().Substring(10); // Últimos dígitos del tick
            return $"VIS-{fecha:yyyy-MM-dd}-{numero}";
        }
    }
}
