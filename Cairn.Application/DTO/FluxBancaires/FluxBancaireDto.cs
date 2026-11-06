namespace Cairn.Application.DTO.FluxBancaires
{
    public class FluxBancaireDto
    {

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Valeur { get; set; }

        public required string Libelle { get; set; }

        public int IdCategorie { get; set; }

        public bool EstSuggestion { get; set; }

        public int? CompteBancaireId { get; set; }
    }
}

