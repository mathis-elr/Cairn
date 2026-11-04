using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class FluxBancaireConfiguration : IEntityTypeConfiguration<FluxBancaire>
    {
        public void Configure(EntityTypeBuilder<FluxBancaire> builder)
        {
            builder.HasIndex(f => f.IdPowens)
                .IsUnique();

            builder.Property(f => f.Date)
                .HasColumnType("date");

            builder.HasOne(f => f.MicroCategorie)
                .WithMany(c => c.FluxBancaires)
                .HasForeignKey(f => f.MicroCategorieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.CompteBanque)
                .WithMany(f => f.FluxBancaires)
                .HasForeignKey(f => f.CompteBanqueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(f => new { f.CompteBanqueId, f.Date });
        }
    }
}
