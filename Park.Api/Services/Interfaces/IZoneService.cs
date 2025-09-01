using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IZoneService
    {
        Task<IEnumerable<ZoneDto>> GetAllZonesAsync();
        Task<ZoneDto?> GetZoneByIdAsync(int id);
        Task<ZoneDto?> GetZoneByNameAsync(string name);
        Task<ZoneDto> CreateZoneAsync(CreateZoneDto createZoneDto);
        Task<ZoneDto?> UpdateZoneAsync(int id, UpdateZoneDto updateZoneDto);
        Task<bool> DeleteZoneAsync(int id);
        Task<bool> ZoneExistsAsync(int id);
        Task<bool> ZoneNameExistsAsync(string name);
        Task<IEnumerable<CompanyDto>> GetZoneCompaniesAsync(int zoneId);
        Task<IEnumerable<GateDto>> GetZoneGatesAsync(int zoneId);
    }
}
