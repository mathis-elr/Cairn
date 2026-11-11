using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class HistoriqueSoldeCompteRepository(IDbContextFactory<CairnDbContext> dbContext) : IHistoriqueSoldeCompteRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<List<HistoriqueSoldeCompte>> GetByDate(DateTime date, IEnumerable<int> compteIds)
        {
            var ids = compteIds.Distinct().ToArray();

            if (ids.Length == 0)
                return [];

            await using var context = await _dbFactory.CreateDbContextAsync();

            return await context.HistoriqueSoldeCompte
                .AsNoTracking()
                .Where(h => h.Date.Date == date.Date && ids.Contains(h.CompteBanqueId))
                .ToListAsync();
        }


        public async Task AddRange(IEnumerable<HistoriqueSoldeCompte> historiqueSoldeComptes)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.HistoriqueSoldeCompte.AddRangeAsync(historiqueSoldeComptes);
            await context.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<HistoriqueSoldeCompte> historiqueSoldeComptes)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            context.HistoriqueSoldeCompte.UpdateRange(historiqueSoldeComptes);
            await context.SaveChangesAsync();
        }
    }
}
