using Cairn.Application.DTO.PositionsInvestissements;
using Cairn.Application.DTO.Powens;
using Cairn.Application.Interfaces.APIs;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.Services.Encrypt;
using Cairn.Domain.Modeles;
using Microsoft.Extensions.Options;

namespace Cairn.Application.Services
{
    public class PositionInvestissementService(IPositionInvestissementRepository positionInvestissementRepository,
                                               IUtilisateurPowensService utilisateurPowensService,
                                               ICompteBanqueRepository compteBanqueRepository,
                                               ILogoDevApiService logoDevApiService,
                                               IOptions<CryptOptions> optionsCrypt,
                                               IPowensApiService powensApiService,
                                               ICryptService encryptService,
                                               IActifService actifService) : IPositionInvestissementService
    {
        private readonly IPositionInvestissementRepository _positionInvestissementRepository = positionInvestissementRepository;
        private readonly IUtilisateurPowensService _utilisateurPowensService = utilisateurPowensService;
        private readonly ICompteBanqueRepository _compteBanqueRepository = compteBanqueRepository;
        private readonly ILogoDevApiService _logoDevApiService = logoDevApiService;
        private readonly IPowensApiService _powensApiService = powensApiService;
        private readonly CryptOptions _optionsCrypt = optionsCrypt.Value;
        private readonly ICryptService _encryptService = encryptService;
        private readonly IActifService _actifService = actifService;

        public async Task<IEnumerable<PositionInvestissementDto>> GetPositionsByCompte(int compteId)
        {
            var positions = await _positionInvestissementRepository.GetByCompteBanqueId(compteId);
            return positions.Select(p =>
                new PositionInvestissementDto
                {
                    Actif = p.Actif!,
                    Quantite = p.Quantite,
                    PrixAchat = p.PrixAchat,
                    PrixCourant = p.PrixCourant,
                    Valorisation = p.Valorisation,
                    DateCours = p.DateCours,
                    DerniereMaj = p.DerniereMaj,
                    UrlLogo = _logoDevApiService.GetUrlLogoByName(p.Actif!.Libelle),
                    CompteBanque = p.CompteBanque!
                }).ToList();
        }

        public async Task SynchroniserPositions()
        {
            var comptesBanque = await _compteBanqueRepository.GetAllInvestissement();

            foreach (var utilisateurs in comptesBanque.GroupBy(c => c.Banque.UtilisateurId))
            {
                var tokenClair = await GetTokenClair(utilisateurs.Key);

                foreach (var compte in utilisateurs)
                {
                    var positions = await _powensApiService.GetPositionInvestissements(tokenClair, compte.IdPowens);
                    await SavePositions(positions, compte.Id);
                }
            }
        }

        public async Task SavePositions(List<PositionInvestImportDto> positionsPowens, int compteBanqueId)
        {
            var existantes = (await _positionInvestissementRepository.GetByCompteBanqueIdSansInclude(compteBanqueId))
                .ToDictionary(p => p.IdPowens);

            var aAjouter = new List<PositionInvestissement>();
            var aMettreAJour = new List<PositionInvestissement>();

            foreach (var p in positionsPowens.DistinctBy(p => p.IdPowens))
            {
                var actifId = await _actifService.GetActifId(p.Code, p.Label);

                if (!existantes.TryGetValue(p.IdPowens, out var position))
                {
                    position = new PositionInvestissement
                    {
                        IdPowens = p.IdPowens,
                        ActifId = actifId,
                        CompteBanqueId = compteBanqueId
                    };
                    aAjouter.Add(position);
                }
                else
                {
                    aMettreAJour.Add(position);
                }

                AppliquerValeursPowens(position, p);
            }

            var idsPowensRecus = positionsPowens.Select(p => p.IdPowens).ToHashSet();
            var idsASupprimer = existantes.Values
                .Where(e => !idsPowensRecus.Contains(e.IdPowens))
                .Select(e => e.Id)
                .ToList();

            await _positionInvestissementRepository.AddRange(aAjouter);
            await _positionInvestissementRepository.UpdateRange(aMettreAJour);
            await _positionInvestissementRepository.DeleteByIds(idsASupprimer);
        }

        private static void AppliquerValeursPowens(PositionInvestissement position, PositionInvestImportDto p)
        {
            var quantite = p.Quantite ?? 0;
            var prixCourant = p.PrixCourant ?? 0;

            position.Quantite = quantite;
            position.PrixCourant = prixCourant;
            position.PrixAchat = p.PrixAchat > 0 ? p.PrixAchat : null;
            position.Valorisation = p.Valorisation ?? quantite * prixCourant;
            position.DateCours = p.DatePrixCourant ?? p.DateDerniereMAJ ?? DateTime.UtcNow;
            position.DerniereMaj = DateTime.UtcNow;
        }

        private async Task<string> GetTokenClair(int userId)
        {
            var utilisateurPowens = await _utilisateurPowensService.GetUtilisateurPowens(userId);
            return _encryptService.Decrypt(utilisateurPowens.AccessTokenCrypte, _optionsCrypt.MasterKey);
        }
    }
}
