namespace Cairn.Domain.Modeles
{
    public class FluxBancaire
    {
        public int Id { get; set; }
        public int IdPowens { get; set; }

        public DateTime Date { get; set; }

        public decimal Valeur { get; set; }

        public required string LibellePowens { get; set; }
        public string? LibellePersonnalise { get; set; }

        public bool EstSuggestion { get; set; } = false;
        public int? MicroCategorieId { get; set; }
        public MicroCategorie? MicroCategorie { get; set; }

        public int CompteBanqueId { get; set; }
        public CompteBanque CompteBanque { get; set; } = null!;
    }
}

