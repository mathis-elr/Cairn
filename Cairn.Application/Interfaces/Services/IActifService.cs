using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Services
{
    public interface IActifService
    {
        Task<List<Actif>> GetAll();

        Task<IEnumerable<string>> GetTickers();

        string NettoyerLibelle(string libelle);

        Task<int> AddActif(Actif actif);

        Task UpdateActif(Actif actif);
    }
}
