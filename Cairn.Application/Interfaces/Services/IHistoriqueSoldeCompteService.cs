using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Services
{
    public interface IHistoriqueSoldeCompteService
    {
        Task SaveRange(IEnumerable<HistoriqueSoldeCompte> historiques);
    }
}
