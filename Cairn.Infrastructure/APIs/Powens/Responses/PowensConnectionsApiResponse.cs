using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public class PowensConnectionsApiResponse
    {
        [JsonPropertyName("connections")]
        public List<PowensConnectionApiResponse> Connections { get; set; } = new();
    }
}
