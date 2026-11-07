using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Cairn.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class CompteBanqueRepository(IDbContextFactory<CairnDbContext> dbContext) : ICompteBanqueRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<IEnumerable<CompteBanque>> GetAllNonInvestissement()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
                .AsNoTracking()
                .Include(c => c.Banque)
                .Where(c => c.TypeCompte != TypeCompte.Investissement)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompteBanque>> GetAllInvestissement()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
                .AsNoTracking()
                .Include(c => c.Banque)
                .Where(c => c.TypeCompte == TypeCompte.Investissement)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompteBanque>> GetAllByBanqueId(int banqueId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
                .AsNoTracking()
                .Where(b => b.BanqueId== banqueId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompteBanque>> GetAllByUserId(int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
                .AsNoTracking()
                .Include(c => c.Banque)
                    .ThenInclude(b => b.Utilisateur)
                        .ThenInclude(u => u.UtilisateurPowens)
                .Where(c => c.Banque.UtilisateurId == userId)
                .ToListAsync();
        }

        public async Task<CompteBanque?> GetById(int id)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
                .AsNoTracking()
                .Include(c => c.Banque)
                    .ThenInclude(b => b.Utilisateur)
                        .ThenInclude(u => u.UtilisateurPowens)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddRange(List<CompteBanque> comptes)
        {
            if (comptes.Count == 0)
                return;

            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.CompteBanque.AddRangeAsync(comptes);
            await context.SaveChangesAsync();
        }

        public async Task UpdateRange(List<CompteBanque> comptes)
        {
            if (comptes.Count == 0)
                return;

            await using var context = await _dbFactory.CreateDbContextAsync();
            context.CompteBanque.UpdateRange(comptes);
            await context.SaveChangesAsync();
        }

        public async Task UpdateNomPersonnalise(int compteId, int userId, string? nom)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.CompteBanque
                .Where(c => c.Id == compteId && c.Banque.UtilisateurId == userId)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.NomPersonnalise, nom));
        }
    }
}
