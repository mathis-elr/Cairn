namespace Cairn.Domain.Modeles
{
    public class PositionInvestissement
    {
        public int Id { get; set; }
        public int IdPowens { get; set; }

        public decimal Quantite { get; set; } 
        
        public decimal PrixAchat { get; set; }    
        
        public decimal PrixCourant { get; set; }

        public decimal Valorisation { get; set; }

        public DateTime DateCours { get; set; }  

        public DateTime DerniereMaj { get; set; }

        public int ActifId { get; set; }
        public Actif Actif { get; set; } = null!;

        public int CompteBanqueId { get; set; }
        public CompteBanque CompteBanque { get; set; } = null!;
    }
}