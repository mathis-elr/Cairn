using Cairn.Domain.Modeles;

namespace Cairn.Application.DTO
{
    public class PositionInvestissementDto
    {
        public Actif Actif { get; set; } = null!;

        public decimal Quantite { get; set; }
        public decimal PrixAchat { get; set; }
        public decimal PrixCourant { get; set; }
        public DateTime DateCours { get; set; }

        public decimal ValeurCourante => Quantite * PrixCourant;
        public decimal EvolutionPourcentage => (PrixCourant - PrixAchat) / PrixAchat;
        public decimal EvolutionValeur => Quantite * PrixCourant - Quantite * PrixAchat;

        public string UrlLogo { get; set; } = string.Empty;

        public DateTime DerniereMaj { get; set; }

        public CompteBanque CompteBanque { get; set; } = null!;
    }
}
