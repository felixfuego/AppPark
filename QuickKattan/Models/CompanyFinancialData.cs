namespace QuickKattan.Models
{
    public class CompanyFinancialData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal Profit { get; set; }
        public decimal MarketCap { get; set; }
        public DateTime LastUpdate { get; set; }

        // Propiedades calculadas
        public decimal ProfitMargin => Revenue != 0 ? (Profit / Revenue) * 100 : 0;
        public bool IsProfitable => Profit > 0;
        public string ProfitMarginFormatted => $"{ProfitMargin:F2}%";
        public string RevenueFormatted => $"${Revenue:N0}";
        public string ProfitFormatted => $"${Profit:N0}";
        public string MarketCapFormatted => $"${MarketCap:N0}";
    }
}