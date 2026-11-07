namespace Cairn.Application.DTO.Powens
{
    public class CompteImportDto
    {
        public int Id { get; set; }

        public string? NomCompte { get; set; }

        public string? Type { get; set; }

        public decimal? Solde { get; set; }

        public bool Desactive { get; set; }
    }
}
