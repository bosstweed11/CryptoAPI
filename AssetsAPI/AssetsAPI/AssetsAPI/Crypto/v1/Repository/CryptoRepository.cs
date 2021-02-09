using AssetsAPI.AssetsAPI.Crypto.v1.Models;
using AssetsAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using AssetsAPI.AssetsAPI.Crypto.v1.Mappers;

namespace AssetsAPI.AssetsAPI.Crypto.v1.Repository
{
    public interface ICryptoRepository
    {
        List<CryptoPrice> GetCryptoPrices(string baseCryptoCurrency,
            string quoteCryptoCurrency,
            DateTime startDate,
            DateTime endDate);

        List<CryptoPrice> GetCryptoPricesSpecificDates(string baseCryptoCurrency,
            string quoteCryptoCurrency,
            List<DateTime> dates);

        void SavePrices(List<CryptoPrice> cryptoPrices);

        List<CryptoCurrency> GetSupportedCryptoCurrencies();
    }

    public class CryptoRepository : ICryptoRepository
    {
        private readonly CurrencyContext _context;
        private readonly ICryptoPriceMapper _modelMapper;

        public CryptoRepository(CurrencyContext context,
            ICryptoPriceMapper modelMapper)
        {
            _context = context;
            _modelMapper = modelMapper;
        }

        public List<CryptoCurrency> GetSupportedCryptoCurrencies()
        {
            var baseCurrencyIds = _context.assets.Select(x => x.base_currency_id).ToList();

            return baseCurrencyIds.Select(x => (CryptoCurrency)x).ToList();
        }

        public List<CryptoPrice> GetCryptoPrices(string baseCryptoCurrency,
            string quoteCryptoCurrency,
            DateTime startDate,
            DateTime endDate)
        {
            var baseCurrencyId = Map(baseCryptoCurrency);
            var quoteCurrencyId = Map(quoteCryptoCurrency);

            var priceTuples = _context.prices
                .Join(_context.assets,
                price => price.asset_id,
                asset => asset.id,
                (price, asset) => new { price, asset })
                .Where(x => x.price.timestamp >= startDate
                    && x.price.timestamp <= endDate
                    && x.asset.base_currency_id == baseCurrencyId
                    && x.asset.quote_currency_id == quoteCurrencyId);

            return priceTuples.Select(x => _modelMapper.MapToCryptoPrice(x.price,
                    baseCryptoCurrency, 
                    quoteCryptoCurrency))
                .ToList();
        }

        public List<CryptoPrice> GetCryptoPricesSpecificDates(string baseCryptoCurrency,
            string quoteCryptoCurrency,
            List<DateTime> dates)
        {
            var baseCurrencyId = Map(baseCryptoCurrency);
            var quoteCurrencyId = Map(quoteCryptoCurrency);

            var priceTuples = _context.prices
                .Join(_context.assets,
                    price => price.asset_id,
                    asset => asset.id,
                    (price, asset) => new { price, asset })
                .Where(x => dates.Contains(x.price.timestamp)
                            && x.asset.base_currency_id == baseCurrencyId
                            && x.asset.quote_currency_id == quoteCurrencyId);

            return priceTuples.Select(x => _modelMapper.MapToCryptoPrice(x.price,
                    baseCryptoCurrency,
                    quoteCryptoCurrency))
                .ToList();
        }

        private long Map(string cryptoCurrency)
        {
            return _context.currencies.First(x => x.name.Equals(cryptoCurrency)).id;
        }

        public void SavePrices(List<CryptoPrice> cryptoPrices)
        {
            var assetId = GetAssetId(cryptoPrices.First().BaseCurrency, cryptoPrices.First().QuoteCurrency);
            var prices = cryptoPrices.ConvertAll(x => _modelMapper.MapToModel(x, assetId));

            _context.prices.AddRange(prices);
            _context.SaveChanges();
        }

        public long GetAssetId(string baseCryptoCurrency, string quoteCryptoCurrency)
        {
            var baseCurrencyId = Map(baseCryptoCurrency);
            var quoteCurrencyId = Map(quoteCryptoCurrency);
            return _context.assets.First(x => x.base_currency_id == baseCurrencyId && x.quote_currency_id == quoteCurrencyId).id;
        }
    }
}
