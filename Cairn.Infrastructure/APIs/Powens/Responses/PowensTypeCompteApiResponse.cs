using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public class PowensTypeCompteApiResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? NomCompte { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("balance")]
        public decimal? Solde { get; set; }

        [JsonPropertyName("disabled")]
        public DateTime? Disabled { get; set; }
    }
}
