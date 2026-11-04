using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class MacroCategorieRepository(IDbContextFactory<CairnDbContext> dbContext) : IMacroCategorieRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<List<MacroCategorie>> GetAll()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.MacroCategorie.ToListAsync();
        }

        public async Task AddRangesAsync(List<MacroCategorie> macros)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.MacroCategorie.AddRangeAsync(macros);
            await context.SaveChangesAsync();
        }
    }
}
