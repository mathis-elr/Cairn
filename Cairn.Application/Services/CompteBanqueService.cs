using Cairn.Application.DTO.FluxBancaires;
using Cairn.Application.DTO.Powens;
using Cairn.Application.Interfaces.APIs;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Domain.Extensions;
using Cairn.Domain.Modeles;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cairn.Application.Services
{
    public class CompteBanqueService(IHistoriqueSoldeCompteService historiqueSoldeCompteService,
                                     ICompteBanqueRepository compteBanqueRepository,
                                     ILogoDevApiService logoDevApiService) : ICompteBanqueService
    {
        private readonly IHistoriqueSoldeCompteService _historiqueSoldeCompteService = historiqueSoldeCompteService;
        private readonly ICompteBanqueRepository _compteBanqueRepository = compteBanqueRepository;
        private readonly ILogoDevApiService _logoDevApiService = logoDevApiService;

        public async Task<List<SourceDto>> GetAllByUserId(int userId)
        {
            var comptes = await _compteBanqueRepository.GetAllByUserId(userId);

            return comptes.Select(c => new SourceDto
            {
                Id = c.Id,
                NomSource = c.Banque.Nom,
                NomCompte = c.NomPersonnalise ?? c.NomPowens,
                TypeCompte = c.TypeCompte,
                LogoUrl = _logoDevApiService.GetUrlLogoByName(c.Banque.Nom),
                StatutConnexion = c.Banque.StatutConnexion,
                DerniereSychro = c.Banque.DerniereSynchro
            }).ToList();
        }

        public async Task SaveComptes(IEnumerable<CompteImportDto> comptesPowens, int banqueId)
        {
            var existants = (await _compteBanqueRepository.GetAllByBanqueId(banqueId))
                .ToDictionary(c => c.IdPowens);

            var aMettreAJour = new List<CompteBanque>();
            var nouveaux = new List<CompteBanque>();
            var comptesAHistoriser = new List<CompteBanque>();

            foreach (var c in comptesPowens.Where(c => !c.Desactive))
            {
                if (existants.TryGetValue(c.Id, out var compte))
                {
                    if (c.Solde != null && compte.Solde != c.Solde)
                    {
                        compte.Solde = c.Solde.Value;
                        aMettreAJour.Add(compte);
                    }
                }
                else
                {
                    nouveaux.Add(new CompteBanque
                    {
                        IdPowens = c.Id,
                        NomPowens = c.NomCompte ?? "Inconnu",
                        TypePowens = c.Type ?? "Inconnu",
                        TypeCompte = TypeCompteExtensions.ToTypeCompte(c.Type),
                        Solde = c.Solde ?? 0,
                        BanqueId = banqueId
                    });
                }

                if (c.Solde.HasValue)
                    comptesAHistoriser.Add(compte);
            }

            if (aMettreAJour.Count > 0)
                await _compteBanqueRepository.UpdateRange(aMettreAJour);

            if (nouveaux.Count > 0)
                await _compteBanqueRepository.AddRange(nouveaux);

            var historiques = comptesAHistoriser
                .Select(compte => new HistoriqueSoldeCompte
                {
                    Date = DateTime.Today,
                    Solde = compte.Solde,
                    CompteBanqueId = compte.Id
                })
                .ToList();

            await _historiqueSoldeCompteService.SaveRange(historiques);
        }

        public async Task UpdateNom(SourceDto source, int userId)
        {
            var nom = string.IsNullOrWhiteSpace(source.NomCompte) ? null : source.NomCompte.Trim();
            await _compteBanqueRepository.UpdateNomPersonnalise(source.Id, userId, nom);
        }
    }
}
