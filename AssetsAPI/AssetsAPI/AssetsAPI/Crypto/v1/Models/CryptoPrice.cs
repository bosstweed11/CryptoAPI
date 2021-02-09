using System;

namespace AssetsAPI.AssetsAPI.Crypto.v1.Models
{
    public class CryptoPrice
    {
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
    }
}
