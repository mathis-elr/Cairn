using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IHistoriqueSoldeCompteRepository
    {
        Task<List<HistoriqueSoldeCompte>> GetByDate(DateTime date, IEnumerable<int> compteIds);

        Task AddRange(IEnumerable<HistoriqueSoldeCompte> historiqueSoldeComptes);
        Task UpdateRange(IEnumerable<HistoriqueSoldeCompte> historiqueSoldeComptes);
    }
}
