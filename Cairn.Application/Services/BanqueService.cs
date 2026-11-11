using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.Services.Encrypt;
using Cairn.Application.Interfaces.APIs;
using Microsoft.Extensions.Options;
using Cairn.Domain.Extensions;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Services
{
    public class BanqueService(IUtilisateurPowensService utilisateurPowensService,
                               ICompteBanqueService compteBanqueService,
                               IOptions<CryptOptions> optionsEncryption,
                               IBanqueRepository banqueRepository,
                               IPowensApiService powensApiService,
                               ICryptService cryptService) : IBanqueService
    {
        private readonly IUtilisateurPowensService _utilisateurPowensService = utilisateurPowensService;
        private readonly ICompteBanqueService _compteBanqueService = compteBanqueService;
        private readonly CryptOptions _optionsEncryption = optionsEncryption.Value;
        private readonly IBanqueRepository _banqueRepository = banqueRepository;
        private readonly IPowensApiService _powensApiService = powensApiService;
        private readonly ICryptService _cryptService = cryptService;

        public async Task SynchroniserSoldeComptesAsync()
        {
            var banques = await _banqueRepository.GetAll();

            foreach (var utilisateur in banques.GroupBy(b => b.UtilisateurId))
            {
                var tokenClair = await GetTokenClair(utilisateur.Key);

                foreach (var banque in utilisateur)
                {
                    var comptesPowens = await _powensApiService.GetComptes(tokenClair, banque.IdConnectionPowens);
                    await _compteBanqueService.SaveComptes(comptesPowens, banque.Id);
                }
            }
        }

        public async Task SaveBanque(int idConnectionPowens, int userId)
        {
            var tokenClair = await GetTokenClair(userId);

            var banque = await _banqueRepository.GetByConnectionIdForUserId(idConnectionPowens, userId);
            var connection = await _powensApiService.GetConnection(tokenClair, idConnectionPowens);

            if (banque == null)
            {
                banque = new Banque
                {
                    IdConnectionPowens = idConnectionPowens,
                    IdConnectorPowens = connection.IdConnector,
                    Nom = await _powensApiService.GetNomBanque(tokenClair, connection.IdConnector),
                    StatutConnexion = StatutConnexionExtension.ToStatutConnexion(connection.PowensState),
                    DerniereSynchro = connection.LastUpdate,
                    UtilisateurId = userId,
                };
                await _banqueRepository.Add(banque);
            }
            else
            {
                banque.IdConnectorPowens = connection.IdConnector;
                banque.StatutConnexion = StatutConnexionExtension.ToStatutConnexion(connection.PowensState);
                banque.DerniereSynchro = connection.LastUpdate;

                await _banqueRepository.Update(banque);
            }

            var comptesPowens = await _powensApiService.GetComptes(tokenClair, idConnectionPowens);
            await _compteBanqueService.SaveComptes(comptesPowens, banque.Id);
        }

        private async Task<string> GetTokenClair(int userId)
        {
            var utilisateurPowens = await _utilisateurPowensService.GetUtilisateurPowens(userId);
            return _cryptService.Decrypt(utilisateurPowens.AccessTokenCrypte, _optionsEncryption.MasterKey);
        }
    }
}
