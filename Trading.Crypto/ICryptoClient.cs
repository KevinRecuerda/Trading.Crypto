using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trading.Crypto
{
    public interface ICryptoClient
    {
        string Name { get; }
        string BaseUrl { get; }

        IMarketDataClient MarketData { get; }

        IPrivateCryptoClient Private { get; }

        IPlatformInfoClient PlatformInfo { get; }
    }

    public interface IMarketDataClient
    {
        Task<Trade> GetLastTrade(CurrencyPair currencyPair);
        Task<List<Trade>> GetTrades(CurrencyPair currencyPair, int? limit = null);

        /// <summary>
        /// Get order book from currency pair and limit.
        /// Limited to 50 orders.
        /// </summary>
        /// <param name="currencyPair">currency pair</param>
        /// <param name="limit">number of orders (max=50)</param>
        /// <returns>Order book associated to currency pair, having limited orders</returns>
        Task<OrderBook> GetOrderBook(CurrencyPair currencyPair, int limit = 1);
    }

    public class OrderBook
    {
        public Order[] Asks { get; set; }
        public Order[] Bids { get; set; }
    }

    public class Order
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }

    public interface IPrivateCryptoClient
    {
    }

    public interface IPlatformInfoClient
    {
    }

    public class Trade
    {
        public DateTime Time { get; set; }
        public long TradeId { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public string Side { get; set; }
    }
    public enum CurrencyPair
    {
        BtcUsd,
        BtcEur,
        BtcGbp,
        EthUsd,
        EthEur,
        EthBtc,
        LtcUsd,
        LtcEur,
        LtcBtc,
        BchUsd,
        XrpEur,
        XrpBtc
    }
    public enum Currency
    {
        USD,
        EUR,
        GBP,
        BTC,
        LTC,
        ETH,
        BCH
    }
}
