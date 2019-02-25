using System;

namespace Trading.Crypto.Kraken
{
    public class GdaxClient : ICryptoClient
    {
        public GdaxClient() : this("https://api.gdax.com")
        {
        }

        public GdaxClient(string baseUrl)
        {
            this.BaseUrl = baseUrl;

            this.MarketData = new GdaxMarketDataClient(baseUrl);
        }

        public string BaseUrl { get; }
        public string Name => "Gdax";

        public IMarketDataClient MarketData { get; }
        public IPrivateCryptoClient Private { get; }
        public IPlatformInfoClient PlatformInfo { get; }
    }

    public class GdaxMarketDataClient : BaseClient, IMarketDataClient
    {
        public GdaxMarketDataClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<Trade> GetLastTrade(ProductType productType)
        {
            var trades = await this.GetTrades(productType, 1);
            return trades.FirstOrDefault();
        }

        public async Task<List<Trade>> GetTrades(ProductType productType, int? limit = null)
        {
            var request = new RestRequest("/products/{productId}/trades", Method.GET);
            var productId = productType.ToString().ToUpper().Insert(3, "-");
            request.AddUrlSegment("productId", productId);
            if (limit.HasValue)
                request.AddParameter("limit", limit.Value);

            var trades = await this.GetAsync<List<Trade>>(request);
            return trades;
        }
    }
}
