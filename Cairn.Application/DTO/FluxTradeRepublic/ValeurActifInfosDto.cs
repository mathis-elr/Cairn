using Cairn.Domain.Enums;

namespace Cairn.Application.DTO.FluxTradeRepublic
{
    public class ValeurActifInfosDto
    {
        public required string Actif { get; set; }

        public string? Logo { get; set; }

        public decimal ValeurInvestit { get; set; }

        public Dictionary<LapsTemps, VariationDataDto> VariationsParLapsTemps { get; set; } = null!;
    }

    public class VariationDataDto
    {
        public decimal VariationPourcentage { get; set; }

        public decimal VariationValeur { get; set; }
    }
}
