using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IZonaService
    {
        Task<IEnumerable<ZonaDto>> GetAllZonasAsync();
        Task<ZonaDto?> GetZonaByIdAsync(int id);
        Task<IEnumerable<ZonaDto>> GetZonasBySitioAsync(int idSitio);
        Task<ZonaDto> CreateZonaAsync(CreateZonaDto createZonaDto);
        Task<ZonaDto> UpdateZonaAsync(UpdateZonaDto updateZonaDto);
        Task<bool> DeleteZonaAsync(int id);
        Task<bool> ActivateZonaAsync(int id);
        Task<bool> DeactivateZonaAsync(int id);
        Task<IEnumerable<ZonaDto>> GetActiveZonasAsync();
    }
}
