using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class PositionInvestissementRepository(IDbContextFactory<CairnDbContext> dbContext) : IPositionInvestissementRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<List<PositionInvestissement>> GetByCompteBanqueId(int compteId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.PositionInvestissement
                .Where(f => f.CompteBanqueId == compteId)
                .Include(p => p.Actif)
                .Include(p => p.CompteBanque)
                .ToListAsync();
        }

        public async Task<List<PositionInvestissement>> GetByCompteBanqueIdSansInclude(int compteBanqueId)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.PositionInvestissement
                .AsNoTracking()
                .Where(p => p.CompteBanqueId == compteBanqueId)
                .ToListAsync();
        }

        public async Task AddRange(List<PositionInvestissement> positions)
        {
            if (positions.Count == 0) return;

            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.AddRangeAsync(positions);
            await context.SaveChangesAsync();
        }

        public async Task UpdateRange(List<PositionInvestissement> positions)
        {
            if (positions.Count == 0) return;

            await using var context = await _dbFactory.CreateDbContextAsync();
            context.UpdateRange(positions);
            await context.SaveChangesAsync();
        }

        public async Task DeleteByIds(List<int> ids)
        {
            if (ids.Count == 0) return;

            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.PositionInvestissement
                .Where(p => ids.Contains(p.Id))
                .ExecuteDeleteAsync();
        }
    }
}
