using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.YahooFinance.Responses
{
    public class YahooSearchApiResponse
    {
        [JsonPropertyName("quotes")]
        public List<YahooTickerApiResponse> Quotes { get; set; } = [];
    }
}

