using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IVisitService
    {
        Task<IEnumerable<VisitDto>> GetAllVisitsAsync();
        Task<VisitDto?> GetVisitByIdAsync(int id);
        Task<VisitDto?> GetVisitByCodeAsync(string visitCode);
        Task<VisitDto> CreateVisitAsync(CreateVisitDto createVisitDto, int createdById);
        Task<VisitDto?> UpdateVisitAsync(int id, UpdateVisitDto updateVisitDto);
        Task<bool> DeleteVisitAsync(int id);
        Task<bool> VisitExistsAsync(int id);
        Task<bool> VisitCodeExistsAsync(string visitCode);
        
        // Funcionalidad de Check-in/Check-out
        Task<VisitDto?> CheckInVisitAsync(VisitCheckInDto checkInDto);
        Task<VisitDto?> CheckOutVisitAsync(VisitCheckOutDto checkOutDto);
        Task<VisitDto?> UpdateVisitStatusAsync(int id, VisitStatusDto statusDto);
        
        // Consultas por filtros
        Task<IEnumerable<VisitDto>> GetVisitsByCompanyAsync(int companyId);
        Task<IEnumerable<VisitDto>> GetVisitsByGateAsync(int gateId);
        Task<IEnumerable<VisitDto>> GetVisitsByStatusAsync(string status);
        Task<IEnumerable<VisitDto>> GetVisitsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<VisitDto>> GetVisitsByVisitorAsync(int visitorId);
        Task<IEnumerable<VisitDto>> GetVisitsByCreatorAsync(int createdById);
        Task<IEnumerable<VisitDto>> GetVisitsByUserPermissionsAsync(int userId);
        
        // Funcionalidad de QR
        Task<string> GenerateQRCodeAsync(int visitId);
        Task<bool> ValidateQRCodeAsync(string qrCodeData);
        Task<VisitDto?> GetVisitByQRCodeAsync(string qrCodeData);
    }
}
