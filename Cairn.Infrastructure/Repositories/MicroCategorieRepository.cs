using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class MicroCategorieRepository(IDbContextFactory<CairnDbContext> dbContext) : IMicroCategorieRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<List<MicroCategorie>> GetAll()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.MicroCategorie.ToListAsync();
        }
    }
}
