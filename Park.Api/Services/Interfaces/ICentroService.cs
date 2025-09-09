using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface ICentroService
    {
        Task<IEnumerable<CentroDto>> GetAllCentrosAsync();
        Task<CentroDto?> GetCentroByIdAsync(int id);
        Task<IEnumerable<CentroDto>> GetCentrosByZonaAsync(int idZona);
        Task<CentroDto> CreateCentroAsync(CreateCentroDto createCentroDto);
        Task<CentroDto> UpdateCentroAsync(UpdateCentroDto updateCentroDto);
        Task<bool> DeleteCentroAsync(int id);
        Task<bool> ActivateCentroAsync(int id);
        Task<bool> DeactivateCentroAsync(int id);
        Task<IEnumerable<CentroDto>> GetActiveCentrosAsync();
    }
}
