using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class UtilisateurConfiguration : IEntityTypeConfiguration<Utilisateur>
    {
        public void Configure(EntityTypeBuilder<Utilisateur> builder)
        {
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasOne(u => u.UtilisateurPowens)
                .WithOne(up => up.Utilisateur)
                .HasForeignKey<UtilisateurPowens>(up => up.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
