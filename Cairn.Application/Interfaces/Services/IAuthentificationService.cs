using Cairn.Application.DTO.Auth;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Services
{
    public interface IAuthentificationService
    {
        Task<int> Inscription(InscriptionDto infosInscription);

        Task<Utilisateur> Connexion(ConnexionDto infosConnexion);
    }
}
