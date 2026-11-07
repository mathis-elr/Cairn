using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface ICompteBanqueRepository
    {
        Task<IEnumerable<CompteBanque>> GetAllNonInvestissement();
        Task<IEnumerable<CompteBanque>> GetAllInvestissement();
        Task<IEnumerable<CompteBanque>> GetAllByBanqueId(int banqueId);
        Task<IEnumerable<CompteBanque>> GetAllByUserId(int userId);
        Task<CompteBanque?> GetById(int id);

        Task AddRange(List<CompteBanque> comptes);

        Task UpdateRange(List<CompteBanque> comptes);
        Task UpdateNomPersonnalise(int compteId, int userId, string? nom);

    }
}
