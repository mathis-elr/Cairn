using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class CategorieFluxRepository(IDbContextFactory<InvestissementDbContext> dbContext) : ICategorieFluxRepository
    {
        private readonly IDbContextFactory<InvestissementDbContext> _dbFactory = dbContext;

        public async Task<List<CategorieFlux>> GetAll()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return context.CategorieFlux.ToList();
        }
    }
}
