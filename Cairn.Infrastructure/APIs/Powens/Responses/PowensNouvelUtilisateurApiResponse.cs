using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public class PowensNouvelUtilisateurApiResponse
    {
        [JsonPropertyName("auth_token")]
        public string? AuthToken { get; set; }

        [JsonPropertyName("id_user")]
        public int IdUser { get; set; }
    }
}
