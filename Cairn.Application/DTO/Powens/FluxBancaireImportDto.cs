namespace Cairn.Application.DTO.Powens
{
    public class FluxBancaireImportDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Valeur { get; set; }

        public string Libelle { get; set; } = string.Empty;
    }
}
