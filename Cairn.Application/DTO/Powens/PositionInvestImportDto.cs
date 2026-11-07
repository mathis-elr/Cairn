namespace Cairn.Application.DTO.Powens
{
    public class PositionInvestImportDto
    {
        public int IdPowens { get; set; }                

        public string? CodeType { get; set; }           

        public string? Code { get; set; }

        public string Label { get; set; } = string.Empty;

        public decimal? Quantite { get; set; }

        public decimal? PrixAchat { get; set; }          

        public decimal? PrixCourant { get; set; }

        public decimal? Valorisation { get; set; }

        public DateTime? DatePrixCourant { get; set; }

        public DateTime? DateDerniereMAJ { get; set; }
    }
}
