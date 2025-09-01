using Park.Maui.Models;

namespace Park.Maui.Services
{
    public interface IVisitService
    {
        Task<List<Visit>?> GetVisitsAsync();
        Task<Visit?> GetVisitByIdAsync(int visitId);
        Task<bool> CheckInVisitAsync(int visitId, int gateId);
        Task<bool> CheckOutVisitAsync(int visitId, int gateId);
        Task<List<Visit>?> GetVisitsByStatusAsync(string status);
        Task<List<Visit>?> GetVisitsByCompanyAsync(int companyId);
    }
}
