namespace QuickKattan.Models
{
    public class StockData
    {
        public DateTime Date { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }

        // Propiedades calculadas
        public decimal Change => Close - Open;
        public decimal ChangePercent => Open != 0 ? (Change / Open) * 100 : 0;
        
        // Para gráficos de velas (candlestick)
        public decimal[] OHLC => new decimal[] { Open, High, Low, Close };
    }
}