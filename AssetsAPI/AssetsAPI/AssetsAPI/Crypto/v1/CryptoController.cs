using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetsAPI.AssetsAPI.Crypto.v1.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetsAPI.AssetsAPI.Crypto.v1
{
    [Route("[controller]/v1")]
    [ApiController]
    public class CryptoController : ControllerBase
    {

        private readonly ICryptoManager _manager;

        public CryptoController(ICryptoManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Route("prices")]
        public async Task<List<CryptoPrice>> Get([FromQuery] CryptoCurrency baseCurrency,
            [FromQuery] CryptoCurrency quoteCurrency,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            return await _manager.GetPrices(baseCurrency, quoteCurrency, startDate, endDate);
        }

        [HttpGet]
        [Route("performance/daysOfMonth")]
        public async Task<List<PerformanceResult>> Get([FromQuery] CryptoCurrency baseCurrency,
            [FromQuery] CryptoCurrency quoteCurrency)
        {
            return await _manager.GetDayOfMonthPerformance(baseCurrency, quoteCurrency);
        }

        [HttpGet]
        [Route("performance")]
        public async Task<List<PerformanceResult>> GetPeformance()
        {
            return await _manager.GetAllCryptoPerformance();
        }

        [HttpGet]
        [Route("supportedCurrencies")]
        public List<CryptoCurrency> GetCurrencies()
        {
            return _manager.GetSupportedCryptoCurrencies();
        }

        [HttpPost]
        [Route("fillRates")]
        public async Task<IActionResult> PostFillRates()
        {
            var cryptoCurrencies = new List<CryptoCurrency> {
                CryptoCurrency.BTC,
                CryptoCurrency.ETH,
                CryptoCurrency.LTC,
                CryptoCurrency.BCH
            };
            var result = await _manager.FillRates(cryptoCurrencies);

            if (!result)
            {
                throw new Exception("Some data was not found.");
            }

            return NoContent();
        }
    }
}
