namespace Cairn.Domain.Modeles
{
    public class HistoriqueSoldeCompte
    {
        public DateTime Date { get; set; }

        public decimal Solde { get; set; }

        public decimal? MontantInvesti { get; set; }

        public int CompteBanqueId { get; set; }
        public CompteBanque CompteBanque { get; set; } = null!;
    }
}
