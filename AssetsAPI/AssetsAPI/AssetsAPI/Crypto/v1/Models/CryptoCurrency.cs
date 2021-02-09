using System.Text.Json.Serialization;

namespace AssetsAPI.AssetsAPI.Crypto.v1.Models
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CryptoCurrency
    {
        
        NotSet,
        USD = 1,
        BTC = 2,
        ETH = 3,
        LTC = 4,
        BCH = 5
    }
}
