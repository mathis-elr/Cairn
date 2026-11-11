using Cairn.Application.DTO.FluxBancaires;
using Cairn.Application.DTO.Powens;
using Cairn.Application.Interfaces.APIs;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.Services.Encrypt;
using Cairn.Domain.Modeles;
using Microsoft.Extensions.Options;

namespace Cairn.Application.Services
{
    public class FluxBancaireService(IUtilisateurPowensService utilisateurPowensService,
                                     IFluxBancaireRepository fluxBancaireRepository,
                                     ICompteBanqueRepository compteBanqueRepository,
                                     IOptions<CryptOptions> optionsCrypt,
                                     IPowensApiService powensApiService,
                                     ICryptService encryptService) : IFluxBancaireService
    {
        private readonly IUtilisateurPowensService _utilisateurPowensService = utilisateurPowensService;
        private readonly IFluxBancaireRepository _fluxBancaireRepository = fluxBancaireRepository;
        private readonly ICompteBanqueRepository _compteBanqueRepository = compteBanqueRepository;
        private readonly IPowensApiService _powensApiService = powensApiService;
        private readonly CryptOptions _optionsCrypt = optionsCrypt.Value;
        private readonly ICryptService _encryptService = encryptService;

        public async Task SynchroniserFluxBancairesAsync()
        {
            var comptes = await _compteBanqueRepository.GetAllNonInvestissement();

            if (!comptes.Any())
                return;

            foreach (var compte in comptes)
            {
                var depuis = await _fluxBancaireRepository.GetDateDernierFluxByCompteId(compte!.Id);
                var tokenClair = await GetTokenClair(compte.Banque.UtilisateurId);
                var flux = await _powensApiService.GetTransactions(tokenClair, compte.IdPowens, depuis);

                await AddRange(flux, compte.Banque.UtilisateurId, compte.Id);
            }
        }

        public async Task<List<FluxBancaireDto>> GetFluxByCompteId(int compteId)
        {
            var flux = await _fluxBancaireRepository.GetByCompteId(compteId);

            return flux.Select(f => new FluxBancaireDto
            {
                Id = f.Id,
                Date = f.Date,
                Valeur = f.Valeur,
                Libelle = f.LibellePersonnalise ?? f.LibellePowens,
                IdCategorie = f.MicroCategorieId ?? 0,
                EstSuggestion = f.EstSuggestion,
                CompteBancaireId = f.CompteBanqueId
            }).ToList();
        }

        public async Task<List<BudgetsByRoleMacroCategorieDto>> GetBudgetMensuelByMacroCategorie(int compteId)
        {
            var rawData = await _fluxBancaireRepository.GetBudgetMensuelByRoleMacroCategorie(compteId);

            var moisPossibles = rawData
                .Select(r => r.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            return rawData
                .GroupBy(r => new { r.RoleMacroCategorie, r.CouleurHexa })
                .Select(g =>
                {
                    var budgetsParDate = g.ToDictionary(x => x.Date, x => x.BudgetCategorie);

                    return new BudgetsByRoleMacroCategorieDto
                    {
                        RoleCategorie = g.Key.RoleMacroCategorie,
                        CouleurHexa = g.Key.CouleurHexa,
                        BudgetCategorieParMois = moisPossibles
                            .Select(m => new BudgetParMoisLineChartDto
                            {
                                Date = m,
                                Budget = budgetsParDate.GetValueOrDefault(m, 0)
                            })
                            .ToList()
                    };
                })
                .OrderByDescending(s => s.BudgetCategorieParMois.Sum(b => b.Budget))
                .ToList();
        }

        private async Task AddRange(List<FluxBancaireImportDto>? fluxDto, int userId, int compteId)
        {
            if (fluxDto == null || fluxDto.Count == 0)
                return;

            var flux = fluxDto.Select(f => new FluxBancaire
            {
                IdPowens = f.Id,
                Date = f.Date,
                Valeur = f.Valeur,
                LibellePowens = f.Libelle ?? string.Empty,
                CompteBanqueId = compteId
            }).ToList();

            var nbFluxAjoutes = await _fluxBancaireRepository.AddRange(flux, compteId);

            if (nbFluxAjoutes > 0)
                await DeterminerCategorieFlux(userId);
        }

        public async Task UpdateRange(List<FluxBancaireDto> fluxDto, int userId)
        {
            var dtoParId = fluxDto.ToDictionary(f => f.Id);
            var flux = await _fluxBancaireRepository.GetByIdsForUserId(dtoParId.Keys.ToList(), userId);

            foreach (var f in flux)
            {
                var dto = dtoParId[f.Id];

                f.LibellePersonnalise = string.IsNullOrWhiteSpace(dto.Libelle) || dto.Libelle == f.LibellePowens
                    ? null
                    : dto.Libelle.Trim();
                f.MicroCategorieId = dto.IdCategorie == 0 ? null : dto.IdCategorie;
                f.EstSuggestion = false;
            }

            await _fluxBancaireRepository.UpdateRange(flux);
        }

        private async Task DeterminerCategorieFlux(int userId)
        {
            var fluxSansCategorie = await _fluxBancaireRepository.GetAllSansCategorie();
            var correspondances = await _fluxBancaireRepository.GetCorrespondancesCategories(userId);

            var fluxModifies = new List<FluxBancaire>();

            foreach (var flux in fluxSansCategorie)
            {
                var cle = flux.LibellePowens.Trim().ToLowerInvariant();

                if (correspondances.TryGetValue(cle, out int idCategorie))
                {
                    flux.MicroCategorieId = idCategorie;
                    flux.EstSuggestion = true;
                    fluxModifies.Add(flux);
                }
            }

            await _fluxBancaireRepository.UpdateRange(fluxModifies);
        }

        private async Task<string> GetTokenClair(int userId)
        {
            var utilisateurPowens = await _utilisateurPowensService.GetUtilisateurPowens(userId);
            return _encryptService.Decrypt(utilisateurPowens.AccessTokenCrypte, _optionsCrypt.MasterKey);
        }
    }
}