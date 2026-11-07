using Cairn.Application.Interfaces.APIs;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.Services.Encrypt;
using Microsoft.Extensions.Options;

namespace Cairn.Application.Services
{
    public class SynchronisationPowensService(IUtilisateurPowensService utilisateurPowensService,
                                              IOptions<CryptOptions> optionsCrypt,
                                              IPowensApiService powensApiService,
                                              IBanqueRepository banqueRepository,
                                              ICryptService cryptService) : ISynchronisationPowensService
    {
        private readonly IUtilisateurPowensService _utilisateurPowensService = utilisateurPowensService;
        private readonly IBanqueRepository _banqueRepository = banqueRepository;
        private readonly IPowensApiService _powensApiService = powensApiService;
        private readonly CryptOptions _optionsCrypt = optionsCrypt.Value;
        private readonly ICryptService _cryptService = cryptService;

        public async Task<string> GetUrlConnexionPowens(int userId)
        {
            var tokenClair = await GetOuCreerTokenClair(userId);
            return await _powensApiService.GetUrlConnexion(tokenClair);
        }

        public async Task<string> GetUrlReconnexionPowens(int userId, int compteId)
        {
            var banque = await _banqueRepository.GetByCompteIdForUserId(compteId, userId)
                ?? throw new InvalidOperationException("Aucune banque associée à ce compte.");

            var tokenClair = await GetOuCreerTokenClair(userId);
            return await _powensApiService.GetUrlReconnexion(tokenClair, banque.IdConnectionPowens);
        }

        private async Task<string> GetOuCreerTokenClair(int userId)
        {
            var utilisateurPowens = await _utilisateurPowensService.GetUtilisateurPowens(userId);
            return _cryptService.Decrypt(utilisateurPowens.AccessTokenCrypte, _optionsCrypt.MasterKey);
        }
    }
}
