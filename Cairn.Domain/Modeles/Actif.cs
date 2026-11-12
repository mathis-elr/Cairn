using Cairn.Domain.Enums;

namespace Cairn.Domain.Modeles
{
    public class Actif
    {
        public int Id { get; set; }

        public required string Libelle { get; set; }

        public string? CodePowens { get; set; }

        public string? Symbole { get; set; }
        public DateTime? DateRechercheSymbole { get; set; } = DateTime.UtcNow;

        public TypeActif Type { get; set; }

        public LogoSource LogoSource { get; set; }

        public ICollection<PositionInvestissement> PositionsInvestissement { get; set; } = [];
        public ICollection<CoursActif> CoursActif { get; set; } = [];
    }
}
