using Cairn.Domain.Modeles;

namespace Cairn.Application.DTO.PositionsInvestissements
{
    public class PositionInvestissementDto
    {
        public Actif Actif { get; set; } = null!;

        public decimal Quantite { get; set; }
        public decimal? PrixAchat { get; set; }
        public decimal? PrixCourant { get; set; }
        public decimal? Valorisation { get; set; }
        public DateTime DateCours { get; set; }

        public bool PerformanceDisponible => PrixAchat is > 0;
        public decimal? ValeurCourante => Valorisation ?? Quantite * PrixCourant;
        public decimal? EvolutionPourcentage => PrixAchat.HasValue ? (PrixCourant - PrixAchat.Value) / PrixAchat.Value : (decimal?)null;
        public decimal? EvolutionValeur => PrixAchat.HasValue ? Quantite * PrixCourant - Quantite * PrixAchat.Value : (decimal?)null;

        public string UrlLogo { get; set; } = string.Empty;

        public DateTime DerniereMaj { get; set; }

        public CompteBanque CompteBanque { get; set; } = null!;
    }
}
