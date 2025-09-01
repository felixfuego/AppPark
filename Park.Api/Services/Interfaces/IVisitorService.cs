using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IVisitorService
    {
        Task<IEnumerable<VisitorDto>> GetAllVisitorsAsync();
        Task<VisitorDto?> GetVisitorByIdAsync(int id);
        Task<VisitorDto?> GetVisitorByEmailAsync(string email);
        Task<VisitorDto?> GetVisitorByDocumentAsync(string documentType, string documentNumber);
        Task<VisitorDto> CreateVisitorAsync(CreateVisitorDto createVisitorDto);
        Task<VisitorDto?> UpdateVisitorAsync(int id, UpdateVisitorDto updateVisitorDto);
        Task<bool> DeleteVisitorAsync(int id);
        Task<bool> VisitorExistsAsync(int id);
        Task<bool> VisitorEmailExistsAsync(string email);
        Task<bool> VisitorDocumentExistsAsync(string documentType, string documentNumber);
        Task<IEnumerable<VisitDto>> GetVisitorVisitsAsync(int visitorId);
        Task<IEnumerable<VisitorDto>> SearchVisitorsAsync(string searchTerm);
    }
}
