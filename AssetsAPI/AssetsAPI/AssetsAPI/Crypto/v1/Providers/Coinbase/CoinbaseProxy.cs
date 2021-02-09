using CoinbasePro;
using CoinbasePro.Services.Products.Models;
using CoinbasePro.Services.Products.Types;
using CoinbasePro.Shared.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetsAPI.AssetsAPI.Crypto.v1.Providers.Coinbase
{
    public interface ICoinbaseProxy
    {
        Task<List<Candle>> GetHistoricPrices(ProductType ticker, DateTime startDate, DateTime endDate);
    }

    public class CoinbaseProxy : ICoinbaseProxy
    {
        private readonly ICoinbaseProClient _client;
        private readonly int DaysPerRequest = 300; // https://docs.pro.coinbase.com/#get-historic-rates
        private readonly int MillisecondsToWaitPerRequest = 333; // https://docs.pro.coinbase.com/#rate-limits

        public CoinbaseProxy(ICoinbaseProClient client)
        {
            _client = client;
        }

        private List<TimeRange> GetTimeRanges(DateTime startDate, DateTime endDate)
        {
            var requestTimeRanges = new List<TimeRange>();
            var timeDifference = endDate - startDate;
            // This algorithm goes from endDate to start date, since we do not know exactly when coinbase is going to make
            // data available from. we have to count backwards so that we will be able to assign dates correctly
            // on the last request that gets data back
            if (timeDifference.TotalDays > DaysPerRequest)
            {
                var iterationEndDate = endDate;
                var nextStepDate = iterationEndDate.AddDays(-DaysPerRequest);
                while (nextStepDate > startDate)
                {
                    requestTimeRanges.Add(new TimeRange
                    {
                        startDate = nextStepDate,
                        endDate = iterationEndDate
                    });

                    iterationEndDate = nextStepDate.AddDays(-1);
                    nextStepDate = iterationEndDate.AddDays(-DaysPerRequest);
                }

                if (iterationEndDate > startDate)
                {
                    requestTimeRanges.Add(new TimeRange
                    {
                        startDate = startDate,
                        endDate = iterationEndDate
                    });
                }
            }
            else
            {
                requestTimeRanges.Add(new TimeRange
                {
                    startDate = startDate,
                    endDate = endDate
                });
            }

            return requestTimeRanges;
        }

        public async Task<List<Candle>> GetHistoricPrices(ProductType ticker, DateTime startDate, DateTime endDate)
        {
            // rate limiting
            var requestTimeRanges = GetTimeRanges(startDate, endDate);
            var candles = new List<Candle>();
            var candleSets = new List<IList<Candle>>();
            
            foreach(var timeRange in requestTimeRanges)
            {
                var rangeCandles = await _client.ProductsService.GetHistoricRatesAsync(
                    ticker,
                    timeRange.startDate,
                    timeRange.endDate,
                    CandleGranularity.Hour24
                );
                candleSets.Add(rangeCandles);
                if (rangeCandles.Count < (timeRange.endDate - timeRange.startDate).TotalDays)
                {
                    break;
                }

                await Task.Delay(MillisecondsToWaitPerRequest);
            }

            for(var i = candleSets.Count - 1; i >= 0; i--)
            {
                candles.AddRange(candleSets[i]);
            }

            return candles;
        }
    }

    public class TimeRange {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
