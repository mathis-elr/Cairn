using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public class PowensCodeTemporaireApiResponse
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}
