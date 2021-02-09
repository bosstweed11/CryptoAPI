namespace AssetsAPI.AssetsAPI.Crypto.v1.Models
{
    public class PerformanceResult
    {
        public string Name { get; set; }
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public decimal ThreeMonthGainLoss { get; set; }
        public decimal SixMonthGainLoss { get; set; }
        public decimal OneYearGainLoss { get; set; }
        public decimal ThreeYearGainLoss { get; set; }
        public decimal FiveYearGainLoss { get; set; }
    }
}
