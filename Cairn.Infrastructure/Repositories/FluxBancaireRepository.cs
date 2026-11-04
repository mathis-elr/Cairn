using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.DTO.FluxBancaires;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class FluxBancaireRepository(IDbContextFactory<CairnDbContext> dbContext) : IFluxBancaireRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<DateTime?> GetDateDernierFluxByCompteId(int compteId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.FluxBancaire
                .Where(f => f.CompteBanqueId == compteId)
                .OrderByDescending(f => f.Date)
                .Select(f => (DateTime?)f.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<List<FluxBancaire>> GetByUserId(int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.FluxBancaire
                .Where(f => f.CompteBanque.Banque.UtilisateurId == userId)
                .Include(f => f.MicroCategorie)
                .ToListAsync();
        }

        public async Task<List<FluxBancaire>> GetByCompteId(int compteId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.FluxBancaire
                .Where(f => f.CompteBanqueId == compteId)
                .Include(f => f.MicroCategorie)
                .ToListAsync();
        }

        public async Task<IEnumerable<FluxBancaire>> GetAllSansCategorie()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            return await context.FluxBancaire
                .Where(f => f.MicroCategorie == null)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int?>> GetCorrespondancesCategories()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            return await context.FluxBancaire
                .Where(f => f.MicroCategorieId != null)
                .GroupBy(f => f.LibellePowens.Trim().ToLower())
                .ToDictionaryAsync(g => g.Key, g => g.First().MicroCategorieId);
        }

        public async Task<List<BudgetCategorieRawDto>> GetBudgetParMacroCategorieParMois(int compteId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            //return await context.FluxBancaire
            //    .Where(f => f.CompteBanqueId == compteId)
            //    .Where(f => f.MicroCategorieId != null)
            //    .Where(f => f.MicroCategorie!.MacroCategorie != null)
            //    .GroupBy(t => new
            //    {
            //        t.Date.Year,
            //        t.Date.Month,
            //        t.MicroCategorie!.MacroCategorie
            //    })
            //    .Select(d => new BudgetCategorieRawDto
            //    {
            //        Categorie = d.Key.MacroCategorie,
            //        Date = new DateTime(d.Key.Year, d.Key.Month, 1),
            //        BudgetCategorie = d.Sum(f => f.Valeur)
            //    })
            //    .ToListAsync();
            throw new NotImplementedException("Cette méthode n'est pas encore implémentée. Elle nécessite une refonte de la structure des catégories.");
        }

        public async Task AddRangeForUserId(List<FluxBancaireImportDto> flux, int userId, int compteId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            var idsExistants = (await context.FluxBancaire
                .Where(f => f.CompteBanque.Banque.UtilisateurId == userId)
                .Select(f => f.Id)
                .ToListAsync())
                .ToHashSet();

            var nvFlux = flux
                .Where(f => !idsExistants.Contains(f.Id))
                .Select(f => new FluxBancaire
                {
                    Id = f.Id,
                    Date = f.Date,
                    Valeur = f.Valeur,
                    LibellePowens = f.Libelle ?? string.Empty,
                    CompteBanqueId = compteId
                });

            context.FluxBancaire.AddRange(nvFlux);

            await context.SaveChangesAsync();
        }

        public async Task UpdateRangeForUserId(List<FluxBancaireDto> fluxMensuelVM, int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            var idVM = fluxMensuelVM.Select(f => f.Id);

            var fluxMensuelEnregistree = await context.FluxBancaire
                .Where(f => idVM.Contains(f.Id))
                .Where(f => f.CompteBanque.Banque.UtilisateurId == userId)
                .ToListAsync();

            var fluxDic = fluxMensuelEnregistree.ToDictionary(e => e.Id);

            foreach (var fluxVm in fluxMensuelVM)
            {
                if (fluxDic.TryGetValue(fluxVm.Id, out var fluxEnregistre))
                {
                    fluxEnregistre.LibellePersonnalise = fluxVm.Libelle;
                    fluxEnregistre.MicroCategorieId = fluxVm.IdCategorie == 0 ? null : fluxVm.IdCategorie;
                    fluxEnregistre.EstSuggestion = false;
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task UpdateRangeSuggestions(IEnumerable<FluxBancaire> fluxList)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            context.FluxBancaire.UpdateRange(fluxList);
            await context.SaveChangesAsync();
        }
    }
}
