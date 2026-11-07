using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IActifRepository
    {
        Task<List<Actif>> GetAll();

        Task<Actif?> GetByCodeOuLibelle(string? code, string libelle);

        Task<IEnumerable<string>> GetAllSymboles();

        Task<int> Add(Actif actif);

        Task Update(Actif actif);
    }
}
