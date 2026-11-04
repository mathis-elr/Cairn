using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure
{
    public class CairnDbContext(DbContextOptions<CairnDbContext> options) : DbContext(options)
    {
        public DbSet<Utilisateur> Utilisateur { get; set; }

        public DbSet<UtilisateurPowens> UtilisateurPowens { get; set; }
        public DbSet<Banque> Banque { get; set; }
        public DbSet<CompteBanque> CompteBanque { get; set; }
        public DbSet<HistoriqueSoldeCompte> HistoriqueSoldeCompte { get; set; }

        public DbSet<FluxBancaire> FluxBancaire { get; set; }
        public DbSet<MacroCategorie> MacroCategorie { get; set; }
        public DbSet<MicroCategorie> MicroCategorie { get; set; }

        public DbSet<PositionInvestissement> PositionInvestissement { get; set; }
        public DbSet<Actif> Actif { get; set; }
        public DbSet<CoursActif> CoursActif { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurations tables
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CairnDbContext).Assembly);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>()
                .HaveMaxLength(200);

            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 2);
        }

    }
}

