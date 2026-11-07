using Cairn.Application.DTO.FluxBancaires;

namespace Cairn.Application.Interfaces.Services
{
    public interface IFluxBancaireService
    {
        Task SynchroniserFluxBancairesAsync();

        Task<List<FluxBancaireDto>> GetFluxByCompteId(int compteId);

        Task<List<BudgetsByRoleMacroCategorieDto>> GetBudgetMensuelByMacroCategorie(int userId);

        Task UpdateRange(List<FluxBancaireDto> fluxMensuelVM, int userId);
    }
}

