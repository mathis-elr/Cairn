using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IUtilisateurRepository
    {
        Task<Utilisateur?> GetByEmail(string email);

        Task<int> Add(Utilisateur utilisateur);
    }
}
