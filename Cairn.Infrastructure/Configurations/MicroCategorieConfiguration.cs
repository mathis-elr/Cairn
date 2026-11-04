using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cairn.Infrastructure.Configurations
{
    internal class MicroCategorieConfiguration : IEntityTypeConfiguration<MicroCategorie>
    {
        public void Configure(EntityTypeBuilder<MicroCategorie> builder)
        {
            builder.Property(m => m.Libelle)
                .HasMaxLength(50);

            builder.HasOne(f => f.MacroCategorie)
                .WithMany(c => c.MicroCategories)
                .HasForeignKey(f => f.MacroCategorieId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(m => new { m.MacroCategorieId, m.Libelle })
                .IsUnique();
        }
    }
}
