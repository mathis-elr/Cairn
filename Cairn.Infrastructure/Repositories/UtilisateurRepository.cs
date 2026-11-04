using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class UtilisateurRepository(IDbContextFactory<CairnDbContext> dbContext) : IUtilisateurRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<Utilisateur?> GetByEmail(string email)
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.Utilisateur.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<int> Add(Utilisateur utilisateur)
        {
            using var dbContext = await _dbFactory.CreateDbContextAsync();
            dbContext.Utilisateur.Add(utilisateur);
            await dbContext.SaveChangesAsync();
            return utilisateur.Id;
        }
    }
}
