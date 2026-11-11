using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.YahooFinance.Responses
{
    public class YahooSearchApiResponse
    {
        [JsonPropertyName("quotes")]
        public List<YahooQuote> Quotes { get; set; } = [];
    }

    public class YahooQuote
    {
        [JsonPropertyName("symbol")]
        public string Ticker { get; set; } = "";

        [JsonPropertyName("quoteType")]
        public string? QuoteType { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("isYahooFinance")]
        public bool IsYahooFinance { get; set; }
    }
}

