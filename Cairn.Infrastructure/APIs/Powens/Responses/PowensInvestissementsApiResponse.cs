using System.Text.Json.Serialization;

namespace Cairn.Infrastructure.APIs.Powens.Responses
{
    public class PowensInvestissementsApiResponse
    {
        [JsonPropertyName("investments")]
        public List<PowensPositionInvestApiResponse> PositionsInvest { get; set; } = [];
    }
}
