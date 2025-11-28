using QuickKattan.Models;

namespace QuickKattan.Services
{
    public interface IFinancialDataService
    {
        Task<List<StockData>> GetStockDataAsync(string symbol, DateTime startDate, DateTime endDate);
        Task<List<CompanyFinancialData>> GetCompaniesFinancialDataAsync();
        Task<CompanyFinancialData?> GetCompanyFinancialDataAsync(int companyId);
    }
}