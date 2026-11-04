using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Cairn.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Cairn.Application.DTO.FluxBancaires;

namespace Cairn.Infrastructure.Repositories
{
    public class CompteBanqueRepository(IDbContextFactory<CairnDbContext> dbContext) : ICompteBanqueRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<IEnumerable<CompteBanque>> GetAllNonInvestissement()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
                .Where(c => c.TypeCompte != TypeCompte.Investissement)
                .Include(c => c.Banque)
                    .ThenInclude(b => b.Utilisateur)
                        .ThenInclude(u => u.UtilisateurPowens)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompteBanque>> GetAllInvestissement()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
                .Where(c => c.TypeCompte == TypeCompte.Investissement)
                .Include(c => c.Banque)
                    .ThenInclude(b => b.Utilisateur)
                        .ThenInclude(u => u.UtilisateurPowens)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompteBanque>> GetAllByBanqueId(int banqueId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
                .Include(b => b.Banque)
                .Where(b => b.Banque.Id == banqueId)
                .ToListAsync();
        }

        public async Task<List<CompteBanque>> GetAllByUserId(int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.CompteBanque
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
                .Include(c => c.Banque)
                    .ThenInclude(b => b.Utilisateur)
                        .ThenInclude(u => u.UtilisateurPowens)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateNom(SourceDto source)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            var compte = await context.CompteBanque.FindAsync(source.Id);
            if (compte != null)
            {
                compte.NomPersonnalise = source.NomCompte;
                context.CompteBanque.Update(compte);
                await context.SaveChangesAsync();
            }
        }

        public async Task Add(CompteBanque compte)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.CompteBanque.AddAsync(compte);
            await context.SaveChangesAsync();
        }

        public async Task Update(CompteBanque compte)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            context.CompteBanque.Update(compte);
            await context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.SaveChangesAsync();
        }
    }
}
