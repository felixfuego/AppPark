using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IGateService
    {
        Task<IEnumerable<GateDto>> GetAllGatesAsync();
        Task<GateDto?> GetGateByIdAsync(int id);
        Task<GateDto?> GetGateByNameAsync(string name);
        Task<GateDto?> GetGateByNumberAsync(string gateNumber);
        Task<GateDto> CreateGateAsync(CreateGateDto createGateDto);
        Task<GateDto?> UpdateGateAsync(int id, UpdateGateDto updateGateDto);
        Task<bool> DeleteGateAsync(int id);
        Task<bool> GateExistsAsync(int id);
        Task<bool> GateNameExistsAsync(string name);
        Task<bool> GateNumberExistsAsync(string gateNumber);
        Task<IEnumerable<GateDto>> GetGatesByZoneAsync(int zoneId);
    }
}
