using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public abstract class PowensPageResponse
    {
        [JsonPropertyName("_links")]
        public PowensLinks? Links { get; set; }
    }

    public class PowensTransactionsApiResponse : PowensPageResponse
    {
        [JsonPropertyName("transactions")]
        public List<PowensTransactionApiResponse> Transactions { get; set; } = [];
    }
}

