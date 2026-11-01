using Cairn.Application.DTO.FluxBancaires;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface ICompteBanqueRepository
    {
        Task<IEnumerable<CompteBanque>> GetAllNonInvestissement();

        Task<IEnumerable<CompteBanque>> GetAllInvestissement();

        Task<IEnumerable<CompteBanque>> GetAllByBanqueId(int banqueId);

        Task<List<CompteBanque>> GetAllByUserId(int userId);

        Task<CompteBanque?> GetById(int id);

        Task UpdateNom(SourceDto source);

        Task Add(CompteBanque compte);

        Task Update(CompteBanque compte);

        Task SaveChanges();
    }
}
