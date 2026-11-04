using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IMacroCategorieRepository
    {
        Task<List<MacroCategorie>> GetAll();

        Task AddRangesAsync(List<MacroCategorie> macros);
    }
}
