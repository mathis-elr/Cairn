using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public class PowensTransactionsApiResponse
    {
        [JsonPropertyName("transactions")]
        public List<PowensTransactionApiResponse> Transactions { get; set; } = [];
    }
}

