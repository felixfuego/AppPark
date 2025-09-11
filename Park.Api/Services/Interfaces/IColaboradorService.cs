using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IColaboradorService
    {
        Task<IEnumerable<ColaboradorDto>> GetAllColaboradoresAsync();
        Task<ColaboradorDto?> GetColaboradorByIdAsync(int id);
        Task<ColaboradorDto?> GetColaboradorByIdentidadAsync(string identidad);
        Task<IEnumerable<ColaboradorDto>> GetColaboradoresByCompaniaAsync(int idCompania);
        Task<ColaboradorDto> CreateColaboradorAsync(CreateColaboradorDto createColaboradorDto);
        Task<ColaboradorDto> UpdateColaboradorAsync(UpdateColaboradorDto updateColaboradorDto);
        Task<bool> DeleteColaboradorAsync(int id);
        Task<bool> ActivateColaboradorAsync(int id);
        Task<bool> DeactivateColaboradorAsync(int id);
        Task<bool> ToggleBlackListAsync(int id);
        Task<IEnumerable<ColaboradorDto>> GetActiveColaboradoresAsync();
        Task<IEnumerable<ColaboradorDto>> GetBlackListedColaboradoresAsync();
        Task<IEnumerable<CompanyDto>> GetEmpresasByZonaAsync(int idZona);
        Task<IEnumerable<CentroDto>> GetCentrosByZonaAsync(int idZona);
    }
}
