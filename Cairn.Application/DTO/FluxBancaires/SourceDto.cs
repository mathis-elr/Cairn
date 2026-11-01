using Cairn.Domain.Enums;

namespace Cairn.Application.DTO.FluxBancaires
{
    public class SourceDto
    {
        public int Id { get; set; }

        public string NomSource { get; set; } = string.Empty;

        public string NomCompte { get; set; } = string.Empty;

        public TypeCompte TypeCompte { get; set; }

        public string LogoUrl { get; set; } = string.Empty;

        public StatutConnexion StatutConnexion { get; set; }

        public DateTime? DerniereSychro { get; set; }
    }
}
