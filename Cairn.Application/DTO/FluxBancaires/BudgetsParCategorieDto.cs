namespace Cairn.Application.DTO.FluxBancaires
{
    public class BudgetsParCategorieDto
    {
        public required string Categorie { get; set; }

        public List<BudgetParMoisLineChartDto> BudgetCategorieParMois { get; set; } = [];
    }
}
