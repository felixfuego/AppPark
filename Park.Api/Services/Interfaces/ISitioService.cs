using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface ISitioService
    {
        Task<IEnumerable<SitioDto>> GetAllSitiosAsync();
        Task<SitioDto?> GetSitioByIdAsync(int id);
        Task<SitioDto> CreateSitioAsync(CreateSitioDto createSitioDto);
        Task<SitioDto> UpdateSitioAsync(UpdateSitioDto updateSitioDto);
        Task<bool> DeleteSitioAsync(int id);
        Task<bool> ActivateSitioAsync(int id);
        Task<bool> DeactivateSitioAsync(int id);
        Task<IEnumerable<SitioDto>> GetActiveSitiosAsync();
    }
}
