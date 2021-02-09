using AssetsAPI.AssetsAPI.Crypto.v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetsAPI.AssetsAPI.Crypto.v1.Mappers;
using AssetsAPI.AssetsAPI.Crypto.v1.Repository;

namespace AssetsAPI.AssetsAPI.Crypto.v1
{
    public interface ICryptoManager
    {
        Task<List<CryptoPrice>> GetPrices(CryptoCurrency baseCryptoCurrency,
            CryptoCurrency quoteCryptoCurrency,
            DateTime startDate,
            DateTime endDate);

        Task<List<PerformanceResult>> GetAllCryptoPerformance();
        Task<List<PerformanceResult>> GetDayOfMonthPerformance(CryptoCurrency baseCryptoCurrency,
            CryptoCurrency quoteCryptoCurrency);

        List<CryptoCurrency> GetSupportedCryptoCurrencies();
        Task<bool> FillRates(List<CryptoCurrency> currenciesToFill);
    }
    
    public class CryptoManager : ICryptoManager
    {
        private readonly ICryptoCurrencyPriceProvider _cryptoPriceProvider;
        private readonly IPerformanceResultMapper _performanceResultMapper;
        private readonly ICryptoRepository _repository;

        public CryptoManager(ICryptoCurrencyPriceProvider cryptoPriceProvider,
            IPerformanceResultMapper performanceResultMapper,
            ICryptoRepository repository)
        {
            _cryptoPriceProvider = cryptoPriceProvider;
            _performanceResultMapper = performanceResultMapper;
            _repository = repository;
        }

        public async Task<List<CryptoPrice>> GetPrices(CryptoCurrency baseCryptoCurrency, 
            CryptoCurrency quoteCryptoCurrency,
            DateTime startDate,
            DateTime endDate)
        {
            return await _cryptoPriceProvider.GetPricesForDateRange(baseCryptoCurrency, startDate, endDate,
                quoteCryptoCurrency);
        }

        private List<CryptoCurrency> CryptoCurrencies = new List<CryptoCurrency>
            {CryptoCurrency.BTC, CryptoCurrency.ETH, CryptoCurrency.LTC, CryptoCurrency.BCH};
        public async Task<List<PerformanceResult>> GetAllCryptoPerformance()
        {
            var performanceResults = new List<PerformanceResult>();
            foreach (var cryptoCurrency in CryptoCurrencies)
            {
                var prices = await _cryptoPriceProvider.GetPricesForDates(cryptoCurrency, GetPerformanceDateList(DateTime.Today));
                var currentPrice = await _cryptoPriceProvider.GetPricesForDates(cryptoCurrency, new List<DateTime>
                    {
                        DateTime.Today
                    });
                var performanceResult = _performanceResultMapper.Map(prices, cryptoCurrency.ToString(), currentPrice.Single().Value);
                performanceResults.Add(performanceResult);
            }

            return performanceResults;
        }

        public async Task<List<PerformanceResult>> GetDayOfMonthPerformance(CryptoCurrency baseCryptoCurrency,
            CryptoCurrency quoteCryptoCurrency)
        {
            var today = DateTime.Today;
            var currentDate = today;
            var performanceResults = new List<PerformanceResult>();

            do
            {
                var prices = await _cryptoPriceProvider.GetPricesForDates(baseCryptoCurrency,
                    GetPerformanceDateList(currentDate),
                    quoteCryptoCurrency);
                var currentPrice = await _cryptoPriceProvider.GetPricesForDates(baseCryptoCurrency, new List<DateTime>
                {
                    DateTime.Today
                });
                var performanceResult = _performanceResultMapper.Map(prices, currentDate.Day.ToString(), currentPrice.Single().Value);
                performanceResults.Add(performanceResult);

                currentDate = currentDate.AddDays(-1);
            } while (currentDate.Day != today.Day);

            return performanceResults;
        }

        public List<DateTime> GetPerformanceDateList(DateTime rootDate)
        {
            var listOfDates = new List<DateTime>{rootDate};

            for (var i = 1; i < 61; i++)
            {
                listOfDates.Add(rootDate.AddMonths(-i));
            }

            return listOfDates;
        }

        public List<CryptoCurrency> GetSupportedCryptoCurrencies()
        {
            return _repository.GetSupportedCryptoCurrencies();
        }

        public async Task<bool> FillRates(List<CryptoCurrency> currenciesToFill)
        {
            foreach (var currency in currenciesToFill)
            {
                var result = await FillForCurrency(currency);

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> FillForCurrency(CryptoCurrency currency)
        {
            var yesterday = DateTime.Today.AddDays(-1);

            var result = await _cryptoPriceProvider.GetPricesForDates(currency,
                    new List<DateTime> { yesterday },
                    CryptoCurrency.USD);

            return result.Count == 1;
        }
    }
}
