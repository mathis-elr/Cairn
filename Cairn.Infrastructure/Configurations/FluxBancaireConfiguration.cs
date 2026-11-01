using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class FluxBancaireConfiguration : IEntityTypeConfiguration<FluxBancaire>
    {
        public void Configure(EntityTypeBuilder<FluxBancaire> builder)
        {
            builder.Property(h => h.Id)
                .ValueGeneratedNever();

            builder.Property(h => h.Date)
                .IsRequired();
            builder.Property(h => h.Valeur)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(h => h.Categorie)
                .WithMany()
                .HasForeignKey(h => h.IdCategorie)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
