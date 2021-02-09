using System;
using System.Collections.Generic;
using System.Linq;
using AssetsAPI.AssetsAPI.Crypto.v1.Models;
using AssetsAPI.Data.Entities;

namespace AssetsAPI.AssetsAPI.Crypto.v1.Mappers
{
    public interface IPerformanceResultMapper
    {
        PerformanceResult Map(List<CryptoPrice> prices, string name, decimal currentAssetValue);
    }
    public class PerformanceResultMapper : IPerformanceResultMapper
    {
        public PerformanceResult Map(List<CryptoPrice> prices, string name, decimal currentAssetValue)
        {
            var result = new PerformanceResult
            {
                BaseCurrency = prices.First().BaseCurrency,
                QuoteCurrency = prices.First().QuoteCurrency,
                Name = name,
                ThreeMonthGainLoss = CalculateGainLoss(prices, 4, currentAssetValue),
                SixMonthGainLoss = CalculateGainLoss(prices, 7, currentAssetValue),
                OneYearGainLoss = CalculateGainLoss(prices, 13, currentAssetValue),
                ThreeYearGainLoss = CalculateGainLoss(prices, 37, currentAssetValue),
                FiveYearGainLoss = CalculateGainLoss(prices, 61, currentAssetValue)
            };

            return result;
        }

        private decimal CalculateGainLoss(List<CryptoPrice> prices, int timePeriodInMonths, decimal currentAssetValue)
        {
            var investmentAmount = 1000;
            decimal totalInvestmentAmount = 0;
            var pricesEarliestToLatest = prices.OrderBy(x => x.Timestamp).ToList();
            decimal result = 0;
            var assetAmounts = new List<decimal>();

            if (pricesEarliestToLatest.Count >= timePeriodInMonths)
            {
                for (var i = 0; i < timePeriodInMonths - 1; i++)
                {
                    var currentPrice = pricesEarliestToLatest[pricesEarliestToLatest.Count - timePeriodInMonths + i];
                    assetAmounts.Add(investmentAmount / currentPrice.Value);
                    totalInvestmentAmount += investmentAmount;
                }

                assetAmounts.Sort();
                int countToRemove = timePeriodInMonths / 12;
                if (countToRemove > 0)
                {
                    assetAmounts.RemoveRange(0, countToRemove);
                    assetAmounts.RemoveRange(assetAmounts.Count - countToRemove - 1, countToRemove);
                    totalInvestmentAmount = totalInvestmentAmount - investmentAmount * countToRemove * 2;
                }

                var totalAssetValue = assetAmounts.Sum() * currentAssetValue;
                result = Math.Round((totalAssetValue - totalInvestmentAmount) / totalInvestmentAmount * 100, 2);
            }

            return result;
        }
    }
}
