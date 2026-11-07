using Cairn.Application.DTO.FluxBancaires;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IFluxBancaireRepository
    {
        Task<DateTime?> GetDateDernierFluxByCompteId(int compteId);

        Task<List<FluxBancaire>> GetByCompteId(int compteId);
        Task<IEnumerable<FluxBancaire>> GetAllSansCategorie();
        Task<List<BudgetRoleMacroCategorieRawDto>> GetBudgetMensuelByRoleMacroCategorie(int compteId);
        Task<Dictionary<string, int>> GetCorrespondancesCategories(int userId);
        Task<List<FluxBancaire>> GetByIdsForUserId(List<int> ids, int userId);

        Task<int> AddRange(List<FluxBancaire> flux, int compteId);
        Task UpdateRange(List<FluxBancaire> flux);
    }
}
