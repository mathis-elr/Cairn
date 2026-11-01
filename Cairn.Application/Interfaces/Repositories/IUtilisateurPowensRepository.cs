using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IUtilisateurPowensRepository
    {
        Task<UtilisateurPowens?> GetByUserId(int userId);

        Task Add(UtilisateurPowens acces);

        Task Update(UtilisateurPowens acces);
    }
}
