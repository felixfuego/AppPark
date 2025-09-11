using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IVisitorService
    {
        Task<IEnumerable<VisitorDto>> GetAllVisitorsAsync();
        Task<IEnumerable<VisitorDto>> GetActiveVisitorsAsync();
        Task<VisitorDto?> GetVisitorByIdAsync(int id);
        Task<VisitorDto?> GetVisitorByEmailAsync(string email);
        Task<VisitorDto?> GetVisitorByDocumentAsync(string documentType, string documentNumber);
        Task<IEnumerable<VisitorDto>> GetVisitorsByCompanyAsync(string company);
        Task<VisitorDto> CreateVisitorAsync(CreateVisitorDto createVisitorDto);
        Task<VisitorDto> UpdateVisitorAsync(UpdateVisitorDto updateVisitorDto);
        Task<bool> DeleteVisitorAsync(int id);
        Task<bool> ActivateVisitorAsync(int id);
        Task<bool> DeactivateVisitorAsync(int id);
        Task<PagedResultDto<VisitorDto>> SearchVisitorsAsync(VisitorSearchDto searchDto);
        
        // Nuevos métodos para gestión de visitantes desde visitas
        Task<VisitorExisteDto> BuscarPorIdentidadAsync(string documentNumber);
        Task<VisitorDto> CrearDesdeVisitaAsync(CrearVisitorDesdeVisitaDto dto);
        Task<VisitorDto> ActualizarDesdeVisitaAsync(int id, CrearVisitorDesdeVisitaDto dto);
    }
}
