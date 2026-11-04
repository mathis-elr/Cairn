using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cairn.Infrastructure.Configurations
{
    internal class UtilisateurPowensConfiguration : IEntityTypeConfiguration<UtilisateurPowens>
    {
        public void Configure(EntityTypeBuilder<UtilisateurPowens> builder)
        {
            builder.HasIndex(u => u.IdPowens)
                .IsUnique();

            builder.Property(u => u.AccessTokenCrypte)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(u => u.Utilisateur)
                .WithOne(u => u.UtilisateurPowens)
                .HasForeignKey<UtilisateurPowens>(u => u.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
