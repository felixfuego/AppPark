using Park.Comun.DTOs;

namespace Park.Android.Services;

public interface IVisitaService
{
    Task<List<VisitaDto>> GetVisitasDelDiaAsync(int guardiaId);
    Task<List<VisitaDto>> GetVisitasByFechaAsync(DateTime fecha);
    Task<List<VisitaDto>> GetVisitasActivasAsync();
    Task<VisitaDto?> GetVisitaByIdAsync(int id);
    Task<List<VisitaDto>> SearchVisitasAsync(string searchTerm);
    Task<VisitaDto?> CheckInAsync(int visitaId, int guardiaId, string? observaciones = null);
    Task<VisitaDto?> CheckOutAsync(int visitaId, int guardiaId, string? observaciones = null);
}
