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
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Banque?> GetByCompteIdForUserId(int compteId, int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Banque
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.UtilisateurId == userId
                                       && b.ComptesBanques.Any(c => c.Id == compteId));
        }

        public async Task<Banque?> GetByConnectionIdForUserId(int idConnectionPowens, int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            return await context.Banque
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.IdConnectionPowens == idConnectionPowens && b.UtilisateurId == userId);
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
    }
}
