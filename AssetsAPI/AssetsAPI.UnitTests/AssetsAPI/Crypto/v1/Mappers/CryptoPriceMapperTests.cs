using AssetsAPI.AssetsAPI.Crypto.v1.Mappers;
using AssetsAPI.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssetsAPI.UnitTests.AssetsAPI.Crypto.v1.Mappers
{
    [TestClass]
    public class CryptoPriceMapperTests
    {
        [TestMethod]
        public void CryptoPriceMapper_Maps_To_CryptoPrice()
        {
            var price = new price()
            {
                asset_id = 1,
                value = 1337,
                timestamp = new System.DateTime()
            };

            var cryptoPriceMapper = new CryptoPriceMapper();

            var result = cryptoPriceMapper.MapToCryptoPrice(price, "BTC", "USD");

            Assert.AreEqual("BTC", result.BaseCurrency);
            Assert.AreEqual("USD", result.QuoteCurrency);
            Assert.AreEqual(1337, result.Value);
            Assert.AreEqual(new System.DateTime(), result.Timestamp);
        }
    }
}
