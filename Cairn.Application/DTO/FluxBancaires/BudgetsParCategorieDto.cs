using Cairn.Domain.Enums;

namespace Cairn.Application.DTO.FluxBancaires
{
    public class BudgetsByRoleMacroCategorieDto
    {
        public required RoleCategorie RoleCategorie { get; set; }

        public required string CouleurHexa { get; set; }

        public List<BudgetParMoisLineChartDto> BudgetCategorieParMois { get; set; } = [];
    }
}
