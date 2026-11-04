using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class BanqueRepository(IDbContextFactory<CairnDbContext> dbContext) : IBanqueRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<IEnumerable<Banque>> GetAll()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Banque
                .Include(b => b.Utilisateur)
                .ToListAsync();
        }

        public async Task<Banque?> GetByUserId(int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Banque
                .Include(b => b.Utilisateur)
                .Include(b => b.ComptesBanques)
                .Where(b => b.Utilisateur.Id == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<Banque?> GetByCompteId(int compteId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Banque
                    .Include(b => b.ComptesBanques)
                    .Include(b => b.Utilisateur)
                    .FirstOrDefaultAsync(b => b.ComptesBanques.Any(c => c.Id == compteId));
        }

        public async Task<IEnumerable<Banque>> GetAllByUserId(int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Banque
                .Include(b => b.Utilisateur)
                .Include(b => b.ComptesBanques)
                .Where(b => b.UtilisateurId == userId)
                .ToListAsync();
        }

        public async Task Add(Banque acces)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.Banque.AddAsync(acces);
            await context.SaveChangesAsync();
        }

        public async Task Update(Banque acces)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            context.Banque.Update(acces);
            await context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.SaveChangesAsync();
        }
    }
}
