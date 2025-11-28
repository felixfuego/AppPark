using System.Text.Json;
using QuickKattan.Models;

namespace QuickKattan.Services
{
    public class FinancialDataService : IFinancialDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public FinancialDataService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        private async Task EnsureAuthenticatedAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<StockData>> GetStockDataAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            // Por ahora devolvemos datos simulados, después conectarás con tu API real
            await Task.CompletedTask; // Simular operación async
            return GenerateMockStockData(symbol, startDate, endDate);
        }

        public async Task<List<CompanyFinancialData>> GetCompaniesFinancialDataAsync()
        {
            await EnsureAuthenticatedAsync();
            
            // Por ahora devolvemos datos simulados
            // Aquí harías: var response = await _httpClient.GetAsync("api/companies/financial");
            return GenerateMockCompaniesData();
        }

        public async Task<CompanyFinancialData?> GetCompanyFinancialDataAsync(int companyId)
        {
            await EnsureAuthenticatedAsync();
            
            // Por ahora devolvemos datos simulados
            // Aquí harías: var response = await _httpClient.GetAsync($"api/companies/{companyId}/financial");
            var companies = GenerateMockCompaniesData();
            return companies.FirstOrDefault(c => c.Id == companyId);
        }

        // Métodos auxiliares para datos de prueba
        private List<StockData> GenerateMockStockData(string symbol, DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            var data = new List<StockData>();
            var currentDate = startDate;
            var price = 100.0m;

            while (currentDate <= endDate)
            {
                var change = (decimal)(random.NextDouble() - 0.5) * 10;
                price = Math.Max(price + change, 10); // Precio mínimo de 10

                var high = price + (decimal)random.NextDouble() * 5;
                var low = price - (decimal)random.NextDouble() * 5;
                var open = low + (decimal)random.NextDouble() * (high - low);
                var close = low + (decimal)random.NextDouble() * (high - low);

                data.Add(new StockData
                {
                    Date = currentDate,
                    Symbol = symbol,
                    Open = open,
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = random.Next(10000, 1000000)
                });

                currentDate = currentDate.AddDays(1);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                    currentDate = currentDate.AddDays(2);
            }

            return data;
        }

        private List<CompanyFinancialData> GenerateMockCompaniesData()
        {
            var random = new Random();
            return new List<CompanyFinancialData>
            {
                new CompanyFinancialData
                {
                    Id = 1,
                    Name = "Empresa A",
                    Symbol = "EPA",
                    Revenue = 1000000 + random.Next(0, 500000),
                    Profit = 200000 + random.Next(-50000, 100000),
                    MarketCap = 5000000 + random.Next(0, 2000000),
                    LastUpdate = DateTime.Now.AddHours(-random.Next(1, 24))
                },
                new CompanyFinancialData
                {
                    Id = 2,
                    Name = "Empresa B",
                    Symbol = "EPB",
                    Revenue = 800000 + random.Next(0, 400000),
                    Profit = 150000 + random.Next(-30000, 80000),
                    MarketCap = 4000000 + random.Next(0, 1500000),
                    LastUpdate = DateTime.Now.AddHours(-random.Next(1, 24))
                },
                new CompanyFinancialData
                {
                    Id = 3,
                    Name = "Empresa C",
                    Symbol = "EPC",
                    Revenue = 1200000 + random.Next(0, 600000),
                    Profit = 300000 + random.Next(-70000, 120000),
                    MarketCap = 6000000 + random.Next(0, 2500000),
                    LastUpdate = DateTime.Now.AddHours(-random.Next(1, 24))
                }
            };
        }
    }
}