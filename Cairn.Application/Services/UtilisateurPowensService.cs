using Cairn.Application.Interfaces.APIs;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.Services.Encrypt;
using Cairn.Domain.Modeles;
using Microsoft.Extensions.Options;

namespace Cairn.Application.Services
{
    public class UtilisateurPowensService(IUtilisateurPowensRepository utilisateurPowensRepository,
                                          IOptions<CryptOptions> optionsEncryption,
                                          IPowensApiService powensApiService,
                                          ICryptService cryptService) : IUtilisateurPowensService
    {
        private readonly IUtilisateurPowensRepository _utilisateurPowensRepository = utilisateurPowensRepository;
        private readonly CryptOptions _optionsEncryption = optionsEncryption.Value;
        private readonly IPowensApiService _powensApiService = powensApiService;
        private readonly ICryptService _encryptService = cryptService;

        public async Task<UtilisateurPowens> GetUtilisateurPowens(int userId)
        {
            var utilisateurPowens = await _utilisateurPowensRepository.GetByUserId(userId);
            if (utilisateurPowens != null)
                return utilisateurPowens;

            var nouvelUtilisateurImport = await _powensApiService.CreeNouvelUtilisateur(userId);

            var nouvelUtilisateur = new UtilisateurPowens
            {
                IdPowens = nouvelUtilisateurImport.IdUser,
                AccessTokenCrypte = _encryptService.Encrypt(nouvelUtilisateurImport.AuthToken, _optionsEncryption.MasterKey),
                UtilisateurId = userId,
            };

            await _utilisateurPowensRepository.Add(nouvelUtilisateur);

            return nouvelUtilisateur;
        }
    }
}
