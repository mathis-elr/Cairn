using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public class PowensConnectorApiResponse
    {
        [JsonPropertyName("name")]
        public string? NomBanque { get; set; }
    }
}
