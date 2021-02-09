using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetsAPI.AssetsAPI.Crypto.v1.Models;
using AssetsAPI.AssetsAPI.Crypto.v1.Proxies;
using AssetsAPI.AssetsAPI.Crypto.v1.Repository;

namespace AssetsAPI.AssetsAPI.Crypto.v1
{
    public interface ICryptoCurrencyPriceProvider
    {
        Task<List<CryptoPrice>> GetPricesForDateRange(CryptoCurrency baseCurrency, DateTime startDate, DateTime endDate,
            CryptoCurrency quoteCurrency = CryptoCurrency.USD);

        Task<List<CryptoPrice>> GetPricesForDates(CryptoCurrency baseCurrency, List<DateTime> dates,
            CryptoCurrency quoteCurrency = CryptoCurrency.USD);
    }
    public class CryptoCurrencyPriceProvider : ICryptoCurrencyPriceProvider
    {
        private readonly ICryptoRepository _repository;
        private readonly ICryptoProviderProxy _cryptoProxy;

        public CryptoCurrencyPriceProvider(ICryptoRepository repository,
            ICryptoProviderProxy cryptoProxy)
        {
            _repository = repository;
            _cryptoProxy = cryptoProxy;
        }

        public async Task<List<CryptoPrice>> GetPricesForDateRange(CryptoCurrency baseCurrency, DateTime startDate,
            DateTime endDate,
            CryptoCurrency quoteCurrency = CryptoCurrency.USD)
        {
            var cryptoPrices = _repository.GetCryptoPrices(baseCurrency.ToString(),
                quoteCurrency.ToString(),
                startDate,
                endDate);

            var lastDbPriceDate = cryptoPrices.Count > 1
                ? cryptoPrices.Max(x => x.Timestamp)
                : startDate.AddDays(-1);

            if (lastDbPriceDate < endDate)
            {
                var providerPrices = await _cryptoProxy.GetCryptoPrices(baseCurrency,
                    quoteCurrency,
                    lastDbPriceDate.AddDays(1),
                    endDate);

                _repository.SavePrices(providerPrices);

                cryptoPrices.AddRange(providerPrices);
            }

            return cryptoPrices.OrderBy(x => x.Timestamp).ToList();
        }

        public async Task<List<CryptoPrice>> GetPricesForDates(CryptoCurrency baseCurrency, List<DateTime> dates,
            CryptoCurrency quoteCurrency = CryptoCurrency.USD)
        {
            var prices = _repository.GetCryptoPricesSpecificDates(baseCurrency.ToString(),
                quoteCurrency.ToString(),
                dates);
            if (prices.Count != dates.Count)
            {
                var priceDates = prices.Select(x => x.Timestamp);
                foreach (var dateTime in dates.Where(dateTime => !priceDates.Contains(dateTime) && DateTime.Today.AddMonths(-12) < dateTime))
                {
                    var providerPrices = await _cryptoProxy.GetCryptoPrices(baseCurrency,
                        quoteCurrency,
                        dateTime,
                        dateTime);

                    _repository.SavePrices(providerPrices);

                    prices.AddRange(providerPrices);
                }
            }

            return prices.OrderByDescending(x => x.Timestamp).ToList();
        }
    }
}
