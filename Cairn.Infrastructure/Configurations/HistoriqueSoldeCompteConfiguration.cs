using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cairn.Infrastructure.Configurations
{
    internal class HistoriqueSoldeCompteConfiguration : IEntityTypeConfiguration<HistoriqueSoldeCompte>
    {
        public void Configure(EntityTypeBuilder<HistoriqueSoldeCompte> builder)
        {
            builder.HasKey(h => new { h.CompteBanqueId, h.Date });

            builder.Property(h => h.Date)
                .HasColumnType("date");

            builder.HasOne(u => u.CompteBanque)
                .WithMany(u => u.HistoriqueSoldeComptes)
                .HasForeignKey(u => u.CompteBanqueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
