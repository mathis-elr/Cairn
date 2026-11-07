using System.Text.Json.Serialization;
namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public class PowensLinks
    {
        [JsonPropertyName("next")]
        public PowensLink? Next { get; set; }
    }

    public class PowensLink
    {
        [JsonPropertyName("href")]
        public string Href { get; set; } = string.Empty;
    }
}
