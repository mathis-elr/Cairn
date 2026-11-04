using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cairn.Infrastructure.Configurations
{
    internal class MacroCategorieConfiguration : IEntityTypeConfiguration<MacroCategorie>
    {
        public void Configure(EntityTypeBuilder<MacroCategorie> builder)
        {
            builder.Property(m => m.Libelle)
                .HasMaxLength(50);

            builder.Property(m => m.CouleurHexa)
                .HasMaxLength(7)
                .IsFixedLength()
                .IsUnicode(false);

            builder.HasOne(m => m.Utilisateur)
                .WithMany(u => u.MacroCategories)
                .HasForeignKey(m => m.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(m => new { m.UtilisateurId, m.Libelle })
                .IsUnique();
        }
    }
}
