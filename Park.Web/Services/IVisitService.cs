using Park.Web.Models;

namespace Park.Web.Services;

public interface IVisitService : IApiService<Visit, CreateVisit, UpdateVisit>
{
    Task<Visit?> GetByCodeAsync(string visitCode);
    Task<Visit?> CheckInAsync(VisitCheckIn checkIn);
    Task<Visit?> CheckOutAsync(VisitCheckOut checkOut);
    Task<Visit?> UpdateStatusAsync(int id, VisitStatusUpdate statusUpdate);
    Task<IEnumerable<Visit>?> GetByCompanyAsync(int companyId);
    Task<IEnumerable<Visit>?> GetByGateAsync(int gateId);
    Task<IEnumerable<Visit>?> GetByStatusAsync(string status);
    Task<IEnumerable<Visit>?> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Visit>?> GetByVisitorAsync(int visitorId);
    Task<string?> GenerateQRCodeAsync(int visitId);
    Task<bool> ValidateQRCodeAsync(string qrCodeData);
    Task<Visit?> GetByQRCodeAsync(string qrCodeData);
}
