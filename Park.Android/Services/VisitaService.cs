using Park.Comun.DTOs;

namespace Park.Android.Services;

public class VisitaService : IVisitaService
{
    private readonly IApiService _apiService;

    public VisitaService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<VisitaDto>> GetVisitasDelDiaAsync(int guardiaId)
    {
        try
        {
            // Usar el endpoint correcto que filtra por zona del guardia
            var visitas = await _apiService.GetAsync<List<VisitaDto>>($"api/visita/guardia-zona/{guardiaId}");
            return visitas ?? new List<VisitaDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error obteniendo visitas del guardia {guardiaId}: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return new List<VisitaDto>();
        }
    }

    public async Task<List<VisitaDto>> GetVisitasActivasAsync()
    {
        try
        {
            var visitas = await _apiService.GetAsync<List<VisitaDto>>("api/visita/activas");
            return visitas ?? new List<VisitaDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error obteniendo visitas activas: {ex.Message}");
            return new List<VisitaDto>();
        }
    }

    public async Task<List<VisitaDto>> GetVisitasByFechaAsync(DateTime fecha)
    {
        try
        {
            var fechaStr = fecha.ToString("yyyy-MM-dd");
            var visitas = await _apiService.GetAsync<List<VisitaDto>>($"api/visita/fecha/{fechaStr}");
            return visitas ?? new List<VisitaDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error obteniendo visitas por fecha {fecha}: {ex.Message}");
            return new List<VisitaDto>();
        }
    }

    public async Task<VisitaDto?> GetVisitaByIdAsync(int id)
    {
        try
        {
            return await _apiService.GetAsync<VisitaDto>($"api/visita/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error obteniendo visita {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<List<VisitaDto>> SearchVisitasAsync(string searchTerm)
    {
        try
        {
            var visitas = await _apiService.GetAsync<List<VisitaDto>>($"api/visita/search?term={searchTerm}");
            return visitas ?? new List<VisitaDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error buscando visitas: {ex.Message}");
            return new List<VisitaDto>();
        }
    }

    public async Task<VisitaDto?> CheckInAsync(int visitaId, int guardiaId, string? observaciones = null)
    {
        try
        {
            var checkInDto = new VisitaCheckInDto
            {
                Id = visitaId,
                FechaLlegada = DateTime.Now,
                IdGuardia = guardiaId,
                Observaciones = observaciones
            };

            return await _apiService.PostAsync<VisitaDto>($"api/visita/{visitaId}/checkin", checkInDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en check-in: {ex.Message}");
            throw;
        }
    }

    public async Task<VisitaDto?> CheckOutAsync(int visitaId, int guardiaId, string? observaciones = null)
    {
        try
        {
            var checkOutDto = new VisitaCheckOutDto
            {
                Id = visitaId,
                FechaSalida = DateTime.Now,
                IdGuardia = guardiaId,
                Observaciones = observaciones
            };

            return await _apiService.PostAsync<VisitaDto>($"api/visita/{visitaId}/checkout", checkOutDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en check-out: {ex.Message}");
            throw;
        }
    }
}
