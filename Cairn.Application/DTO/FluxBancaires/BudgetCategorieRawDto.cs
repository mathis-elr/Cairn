using Cairn.Domain.Enums;

namespace Cairn.Application.DTO.FluxBancaires
{
    public class BudgetRoleMacroCategorieRawDto
    {
        public required RoleCategorie RoleMacroCategorie { get; set; }

        public required string CouleurHexa { get; set; }

        public DateTime Date { get; set; }

        public decimal BudgetCategorie { get; set; }
    }
}
