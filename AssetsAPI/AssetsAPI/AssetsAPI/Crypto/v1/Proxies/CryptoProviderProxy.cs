using AssetsAPI.AssetsAPI.Crypto.v1.Models;
using AssetsAPI.AssetsAPI.Crypto.v1.Providers.Coinbase;
using CoinbasePro.Services.Products.Models;
using CoinbasePro.Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetsAPI.AssetsAPI.Crypto.v1.Proxies
{
    public interface ICryptoProviderProxy
    {
        Task<List<CryptoPrice>> GetCryptoPrices(CryptoCurrency baseCryptoCurrency, 
            CryptoCurrency quoteCryptoCurrency, DateTime startDate, DateTime endDate);
    }

    public class CryptoProviderProxy : ICryptoProviderProxy
    {
        private readonly ICoinbaseProxy _coinbaseProxy;

        public CryptoProviderProxy(ICoinbaseProxy coinbaseProxy)
        {
            _coinbaseProxy = coinbaseProxy;
        }

        public async Task<List<CryptoPrice>> GetCryptoPrices(CryptoCurrency baseCryptoCurrency,
            CryptoCurrency quoteCryptoCurrency, 
            DateTime startDate, 
            DateTime endDate)
        {
            var productType = GetProductType(baseCryptoCurrency, quoteCryptoCurrency);
            var candles = await _coinbaseProxy.GetHistoricPrices(productType, startDate, endDate);

            return candles.Select(x => Map(x, baseCryptoCurrency, quoteCryptoCurrency)).ToList();
        }

        private ProductType GetProductType(CryptoCurrency baseCryptoCurrency, CryptoCurrency quoteCryptoCurrency)
        {
            switch (baseCryptoCurrency)
            {
                case CryptoCurrency.BTC:
                    return ProductType.BtcUsd;
                case CryptoCurrency.ETH:
                    return ProductType.EthUsd;
                case CryptoCurrency.LTC:
                    return ProductType.LtcUsd;
                case CryptoCurrency.BCH:
                    return ProductType.BchUsd;
                default:
                    throw new ArgumentOutOfRangeException($"{baseCryptoCurrency} / {quoteCryptoCurrency} is not supported");
            }
        }

        private CryptoPrice Map(Candle candle, CryptoCurrency baseCryptoCurrency, CryptoCurrency quoteCryptoCurrency)
        {
            return new CryptoPrice
            {
                BaseCurrency = baseCryptoCurrency.ToString(),
                QuoteCurrency = quoteCryptoCurrency.ToString(),
                Value = candle.Close.GetValueOrDefault(),
                Timestamp = candle.Time
            };
        }
    }
}
