using Cairn.Application.DTO.FluxBancaires;

namespace Cairn.Application.Interfaces.Services
{
    public interface IFluxBancaireService
    {
        Task<DateTime?> GetDateDernierFlux(int compteId);

        Task<List<FluxBancaireDto>> GetFluxBancaire(int userId);

        Task<List<FluxBancaireDto>> GetFluxByCompteId(int compteId);

        Task<IEnumerable<BudgetsByRoleMacroCategorieDto>> GetBudgetMensuelByMacroCategorie(int compteId);

        Task AddFluxBancaire(List<FluxBancaireImportDto>? flux, int userId, int comptePowensId);

        Task UpdateFluxMensuel(List<FluxBancaireDto> fluxMensuelVM, int userId);
    }
}

