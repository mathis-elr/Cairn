using Cairn.Application.Interfaces.Repositories;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Repositories
{
    public class ActifRepository(IDbContextFactory<CairnDbContext> dbContext) : IActifRepository
    {
        private readonly IDbContextFactory<CairnDbContext> _dbFactory = dbContext;

        public async Task<List<Actif>> GetAll()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Actif.ToListAsync();
        }

        public async Task<Actif?> GetByCodeOuLibelle(string? code, string libelle)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();

            var query = context.Actif.AsNoTracking();

            query = code != null
                ? query.Where(a => a.CodePowens == code)
                : query.Where(a => a.Libelle == libelle); 

            return await query.FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<string>> GetAllSymboles()
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Actif
                .Where(f => f.Symbole != null)
                .Select(f => f.Symbole!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<int> Add(Actif actif)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            await context.AddAsync(actif);
            await context.SaveChangesAsync();
            return actif.Id;
        }

        public async Task Update(Actif actif)
        {
            await using var context = await _dbFactory.CreateDbContextAsync();
            context.Update(actif);
            await context.SaveChangesAsync();
        }
    }
}
