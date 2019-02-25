using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Trading.Crypto.Gdax.Tests
{
    public class GdaxClientTests
    {
        [Fact]
        public async Task Should_return_last_trade()
        {
            var gdaxClient = new GdaxClient();

            var lastTrade = await gdaxClient.MarketData.GetLastTrade(CurrencyPair.BtcEur);

            Assert.NotNull(lastTrade);
        }

        [Fact]
        public async Task Should_return_top1_order_book()
        {
            var gdaxClient = new GdaxMarketDataClient("https://api.gdax.com");

            var orderBook = await gdaxClient.GetOrderBook(CurrencyPair.BtcEur, 1);

            Assert.NotNull(orderBook);
            AssertContainsNotEmptyOrder(orderBook.Asks, 1);
            AssertContainsNotEmptyOrder(orderBook.Bids, 1);
        }

        private static void AssertContainsNotEmptyOrder(IReadOnlyList<Order> orders, int expectedCount)
        {
            Assert.NotNull(orders);
            Assert.Equal(expectedCount, orders.Count);

            for (var i = 0; i < expectedCount; i++)
            {
                AssetNotEmptyOrder(orders[i]);
            }
        }

        private static void AssetNotEmptyOrder(Order order)
        {
            Assert.NotEqual(0, order.Price);
            Assert.NotEqual(0, order.Quantity);
        }
    }
}

