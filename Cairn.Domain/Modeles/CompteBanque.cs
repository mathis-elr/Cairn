using Cairn.Domain.Enums;

namespace Cairn.Domain.Modeles
{
    public class CompteBanque
    {
        public int Id { get; set; }

        public int IdPowens { get; set; }

        public required string TypePowens { get; set; }

        public TypeCompte TypeCompte { get; set; }

        public required string NomPowens { get; set; }
        public string? NomPersonnalise{ get; set; }

        public decimal Solde { get; set; }

        public DateTime? DerniereSynchro { get; set; }

        public int BanqueId { get; set; }
        public Banque Banque { get; set; } = null!;

        public ICollection<FluxBancaire> FluxBancaires { get; set; } = [];

        public ICollection<PositionInvestissement> PositionsInvestissement { get; set; } = [];

        public ICollection<HistoriqueSoldeCompte> HistoriqueSoldeComptes { get; set; } = [];
    }
}
