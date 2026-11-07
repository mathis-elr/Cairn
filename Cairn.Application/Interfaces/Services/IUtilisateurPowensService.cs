using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Services
{
    public interface IUtilisateurPowensService
    {
        Task<UtilisateurPowens> GetUtilisateurPowens(int userId);
    }
}
