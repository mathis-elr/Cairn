using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class PositionInvestissementConfiguration : IEntityTypeConfiguration<PositionInvestissement>
    {
        public void Configure(EntityTypeBuilder<PositionInvestissement> builder)
        {
            builder.HasIndex(p => p.IdPowens)
                .IsUnique();

            builder.Property(e => e.Quantite)
                .HasPrecision(18, 8);

            builder.Property(e => e.PrixAchat)
                .HasPrecision(18, 6);

            builder.Property(e => e.PrixCourant)
                .HasPrecision(18, 6);

            builder.HasOne(p => p.Actif)
                .WithMany(a => a.PositionsInvestissement)
                .HasForeignKey(p => p.ActifId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CompteBanque)
                .WithMany(c => c.PositionsInvestissement)
                .HasForeignKey(c => c.CompteBanqueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
