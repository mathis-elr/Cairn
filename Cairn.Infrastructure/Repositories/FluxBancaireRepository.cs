using Cairn.Application.DTO.FluxBancaires;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Enums;
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
                .AsNoTracking()
                .Where(f => f.CompteBanqueId == compteId)
                .MaxAsync(f => (DateTime?)f.Date);
        }

        public async Task<List<FluxBancaire>> GetByCompteId(int compteId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.FluxBancaire
                .AsNoTracking()
                .Include(f => f.MicroCategorie)
                .Where(f => f.CompteBanqueId == compteId)
                .OrderByDescending(f => f.Date)
                .ToListAsync();
        }

        public async Task<List<FluxBancaire>> GetByIdsForUserId(List<int> ids, int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            return await context.FluxBancaire
                .AsNoTracking()
                .Where(f => ids.Contains(f.Id) && f.CompteBanque.Banque.UtilisateurId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FluxBancaire>> GetAllSansCategorie()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            return await context.FluxBancaire
                .AsNoTracking()
                .Where(f => f.MicroCategorie == null)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetCorrespondancesCategories(int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            return await context.FluxBancaire
                .AsNoTracking()
                .Where(f => f.CompteBanque.Banque.UtilisateurId == userId
                         && f.MicroCategorieId != null
                         && !f.EstSuggestion)
                .GroupBy(f => f.LibellePowens.Trim().ToLower())
                .Select(g => new
                {
                    Libelle = g.Key,
                    MicroCategorieId = g.Max(f => f.MicroCategorieId!.Value)
                })
                .ToDictionaryAsync(x => x.Libelle, x => x.MicroCategorieId);
        }

        public async Task<List<BudgetRoleMacroCategorieRawDto>> GetBudgetMensuelByRoleMacroCategorie(int compteId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            return await context.FluxBancaire
                .AsNoTracking()
                .Where(f => f.CompteBanqueId == compteId
                         && f.MicroCategorieId != null
                         && f.MicroCategorie!.MacroCategorie != null
                         && f.MicroCategorie!.MacroCategorie.Role != RoleCategorie.Neutre)
                .GroupBy(t => new
                {
                    t.Date.Year,
                    t.Date.Month,
                    t.MicroCategorie!.MacroCategorie.Role,
                    t.MicroCategorie!.MacroCategorie.CouleurHexa
                })
                .Select(d => new BudgetRoleMacroCategorieRawDto
                {
                    RoleMacroCategorie = d.Key.Role,
                    CouleurHexa = d.Key.CouleurHexa,
                    Date = new DateTime(d.Key.Year, d.Key.Month, 1),
                    BudgetCategorie = d.Sum(f => f.Valeur)
                })
                .ToListAsync();
        }

        // A VERIFIER
        public async Task<int> AddRange(List<FluxBancaire> flux, int compteId)
        {
            if (flux.Count == 0) return 0;

            await using var context = await _dbFactory.CreateDbContextAsync();

            var idsPowensRecus = flux.Select(f => f.IdPowens).Distinct().ToList();

            var dejaPresents = (await context.FluxBancaire
                    .Where(f => f.CompteBanqueId == compteId && idsPowensRecus.Contains(f.IdPowens))
                    .Select(f => f.IdPowens)
                    .ToListAsync())
                .ToHashSet();

            var nouveaux = flux
                .DistinctBy(f => f.IdPowens)                    
                .Where(f => !dejaPresents.Contains(f.IdPowens))
                .ToList();

            if (nouveaux.Count == 0) return 0;

            foreach (var f in nouveaux)
                f.CompteBanqueId = compteId;                  

            context.FluxBancaire.AddRange(nouveaux);
            await context.SaveChangesAsync();

            return nouveaux.Count;
        }

        public async Task UpdateRange(List<FluxBancaire> flux)
        {
            if (flux.Count == 0) return;

            await using var context = await _dbFactory.CreateDbContextAsync();

            context.FluxBancaire.UpdateRange(flux);
            await context.SaveChangesAsync();
        }
    }
}
