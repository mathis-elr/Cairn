namespace Cairn.Domain.Modeles
{
    public class CoursActif
    {
        public DateTime Date { get; set; }

        public decimal Prix { get; set; }

        public int ActifId { get; set; }
        public Actif Actif { get; set; } = null!;
    }
}
