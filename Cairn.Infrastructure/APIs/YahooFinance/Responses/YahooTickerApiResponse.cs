using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.YahooFinance.Responses
{
    public class YahooTickerApiResponse
    {
        [JsonPropertyName("symbol")]
        public string? Ticker { get; set; }
    }
}

