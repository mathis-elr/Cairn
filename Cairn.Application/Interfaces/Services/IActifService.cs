using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Services
{
    public interface IActifService
    {
        Task<List<Actif>> GetAll();

        Task<IEnumerable<string>> GetTickers();

        Task<int> GetActifId(string? code, string libelle);

        string NettoyerLibelle(string libelle);
    }
}
