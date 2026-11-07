using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class UtilisateurPowensRepository(IDbContextFactory<CairnDbContext> dbContext) : IUtilisateurPowensRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<UtilisateurPowens?> GetByUserId(int userId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.UtilisateurPowens
                .Include(u => u.Utilisateur)
                    .ThenInclude(u => u.Banques)
                .Where(u => u.UtilisateurId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task Add(UtilisateurPowens acces)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.UtilisateurPowens.AddAsync(acces);
            await context.SaveChangesAsync();
        }
    }
}
