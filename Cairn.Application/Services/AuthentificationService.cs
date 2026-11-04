using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.DTO.Auth;
using Cairn.Domain.Modeles;
using Microsoft.AspNetCore.Identity;
using Flurl.Util;

namespace Cairn.Application.Services
{
    public class AuthentificationService(IUtilisateurRepository utilisateurRepository,
                                         IMacroCategorieService macroCategorieService) : IAuthentificationService
    {
        private readonly IUtilisateurRepository _utilisateurRepository = utilisateurRepository; 
        private readonly IMacroCategorieService _macroCategorieService = macroCategorieService;

        public async Task<int> Inscription(InscriptionDto infosInscription)
        {
            var utilisateur = await _utilisateurRepository.GetByEmail(infosInscription.Email);
            if (utilisateur != null)
                throw new Exception("Un compte avec cette adresse e-mail existe déjà.");

            var newUser = new Utilisateur
            {
                Email = infosInscription.Email,
                Prenom = char.ToUpper(infosInscription.Prenom[0]) + infosInscription.Prenom.Substring(1).ToLower(),
                MdpHash = HashPassword(infosInscription.Mdp), 
                DateCreationCompte = DateTime.UtcNow
            };

            await _utilisateurRepository.Add(newUser);

            await _macroCategorieService.InitialiserAsync(newUser.Id);

            return newUser.Id;
        }

        public async Task<Utilisateur> Connexion(ConnexionDto infosConnexion)
        {
            var utilisateur = await _utilisateurRepository.GetByEmail(infosConnexion.Email);

            if (utilisateur == null)
                throw new Exception("Adresse e-mail incorrect.");
            else if(!VerifyPassword(infosConnexion.Mdp, utilisateur.MdpHash))
                throw new Exception("Mot de passe incorrect.");

            return utilisateur;
        }

        private string HashPassword(string password)
        {
            var hasher = new PasswordHasher<Utilisateur>();
            return hasher.HashPassword(null!, password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hasher = new PasswordHasher<Utilisateur>();
            return hasher.VerifyHashedPassword(null!, hashedPassword, password) == PasswordVerificationResult.Success;
        }
    }
}
