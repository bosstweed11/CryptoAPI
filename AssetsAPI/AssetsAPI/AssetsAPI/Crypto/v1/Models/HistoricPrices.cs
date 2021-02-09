namespace AssetsAPI.AssetsAPI.Crypto.v1.Models
{
    public class HistoricPrices
    {
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? PriceThreeMonthsAgo { get; set; }
        public decimal? PriceSixMonthsAgo { get; set; }
        public decimal? PriceOneYearAgo { get; set; }
        public decimal? PriceThreeYearsAgo { get; set; }
        public decimal? PriceFiveYearsAgo { get; set; }
    }
}
