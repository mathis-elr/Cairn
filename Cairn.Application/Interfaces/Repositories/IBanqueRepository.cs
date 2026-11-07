using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IBanqueRepository
    {
        Task<IEnumerable<Banque>> GetAll();

        Task<Banque?> GetByCompteIdForUserId(int idCompte, int userId);

        Task<Banque?> GetByConnectionIdForUserId(int idConnectionPowens, int userId);

        Task Add(Banque acces);

        Task Update(Banque acces);
    }
}
