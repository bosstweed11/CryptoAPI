using AssetsAPI.AssetsAPI.Crypto.v1.Models;
using AssetsAPI.Data.Entities;

namespace AssetsAPI.AssetsAPI.Crypto.v1.Mappers
{
    public interface ICryptoPriceMapper
    {
        price MapToModel(CryptoPrice cryptoPrice, long assetId);
        CryptoPrice MapToCryptoPrice(price price, string baseCurrency, string quoteCurrency);
    }

    public class CryptoPriceMapper : ICryptoPriceMapper
    {
        public price MapToModel(CryptoPrice cryptoPrice, long assetId)
        {
            return new price
            {
                value = cryptoPrice.Value,
                timestamp = cryptoPrice.Timestamp,
                asset_id = assetId
            };
        }

        public CryptoPrice MapToCryptoPrice(price price, string baseCurrency, string quoteCurrency)
        {
            return new CryptoPrice
            {
                Value = price.value,
                Timestamp = price.timestamp,
                BaseCurrency = baseCurrency,
                QuoteCurrency = quoteCurrency
            };
        }
    }
}
