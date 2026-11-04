using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cairn.Infrastructure.Configurations
{ 
    internal class BanqueConfiguration : IEntityTypeConfiguration<Banque>
    {
        public void Configure(EntityTypeBuilder<Banque> builder)
        {
            builder.HasIndex(u => u.IdConnectionPowens)
                .IsUnique();

            builder.HasOne(u => u.Utilisateur)
                .WithMany(u => u.Banques)
                .HasForeignKey(u => u.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
