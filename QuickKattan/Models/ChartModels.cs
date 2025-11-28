namespace QuickKattan.Models
{
    public class ChartDataPoint
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string? Color { get; set; }
    }

    public class TimeSeriesPoint
    {
        public DateTime X { get; set; }
        public decimal Y { get; set; }
    }

    public class CandlestickPoint
    {
        public DateTime X { get; set; }
        public decimal[] Y { get; set; } = new decimal[4]; // [Open, High, Low, Close]
    }
}