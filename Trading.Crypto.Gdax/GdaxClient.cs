using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Trading.Crypto.Tools;

namespace Trading.Crypto.Gdax
{
    public class GdaxClient : ICryptoClient
    {
        public GdaxClient(string baseUrl = "https://api.gdax.com")
        {
            this.BaseUrl = baseUrl;

            this.MarketData = new GdaxMarketDataClient(baseUrl);
        }

        public string Name => "Gdax";
        public string BaseUrl { get; }

        public IMarketDataClient MarketData { get; }
        public IPrivateCryptoClient Private { get; }
        public IPlatformInfoClient PlatformInfo { get; }
    }

    public class GdaxMarketDataClient : BaseClient, IMarketDataClient
    {
        public GdaxMarketDataClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<Trade> GetLastTrade(CurrencyPair currencyPair)
        {
            var trades = await this.GetTrades(currencyPair, 1);
            return trades.FirstOrDefault();
        }

        public async Task<List<Trade>> GetTrades(CurrencyPair currencyPair, int? limit = null)
        {
            var request = BuildProductRequest("trades", Method.GET, currencyPair);
            if (limit.HasValue)
                request.AddParameter("limit", limit.Value);

            var trades = await this.GetAsync<List<Trade>>(request);
            return trades;
        }

        public async Task<OrderBook> GetOrderBook(CurrencyPair currencyPair, int limit = 1)
        {
            if (!limit.IsInRange(1, 50))
                throw new ArgumentException("limit should be between 1 and 50", "limit");

            var request = BuildProductRequest("book", Method.GET, currencyPair);
            // level 1 = top 1
            // level 2 = top 50
            var level = limit > 1 ? 2 : 1;
            request.AddParameter("level", level);

            var orderBookDto = await this.GetAsync<OrderBookDto>(request);

            var orderBook = orderBookDto.ToOrderBook();
            return orderBook;
        }

        private static RestRequest BuildProductRequest(string ressource, Method method, CurrencyPair currencyPair)
        {
            var request = new RestRequest($"/products/{{productId}}/{ressource}", Method.GET);
            var productId = currencyPair.ToString().ToUpper().Insert(3, "-");
            request.AddUrlSegment("productId", productId);

            return request;
        }

        private class OrderBookDto
        {
            public object[,] Asks { get; set; }
            public object[,] Bids { get; set; }

            public OrderBook ToOrderBook()
            {
                var n = Math.Min(this.Asks.GetLength(0), this.Bids.GetLength(0));
                var orderBook = new OrderBook()
                {
                    Asks = new Order[n],
                    Bids = new Order[n]
                };

                for (var i = 0; i < n; i++)
                {
                    var ask = new Order()
                    {
                        Price = Convert.ToDecimal(this.Asks[i, 0], CultureInfo.InvariantCulture),
                        Quantity = Convert.ToDecimal(this.Asks[i, 1], CultureInfo.InvariantCulture)
                    };
                    orderBook.Asks[i] = ask;

                    var bid = new Order()
                    {
                        Price = Convert.ToDecimal(this.Asks[i, 0], CultureInfo.InvariantCulture),
                        Quantity = Convert.ToDecimal(this.Asks[i, 1], CultureInfo.InvariantCulture)
                    };
                    orderBook.Bids[i] = bid;
                }

                return orderBook;

            }
        }
    }
}